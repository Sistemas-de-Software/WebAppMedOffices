using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppMedOffices.Models;
using WebAppMedOffices.Constants;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebAppMedOffices.Shared;

namespace WebAppMedOffices.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GestionMedicosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public async Task<ActionResult> Index()
        {
            return View(await db.Medicos.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            Medico medico = await db.Medicos.FindAsync(id);
            
            if (medico == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            return View(medico);
        }

        public ActionResult Create()
        {
            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MedicoEspecialidadViewModel medicoEspecialidad)
        {
            //creamos el ámbito de la transacción
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                        {
                            Message = "Error al validar los campos",
                            MessageType = GenericMessages.danger
                        };
                        ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre");
                        return View(medicoEspecialidad);
                    }

                    if (medicoEspecialidad.DuracionTurnoEspecialidad.Duracion <= 9)
                    {
                        TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                        {
                            Message = "Error al validar duración del turno, la duración debe ser mayor o igual a 10 minutos",
                            MessageType = GenericMessages.danger
                        };
                        ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre");
                        return View(medicoEspecialidad);
                    }

                    Medico existeMedico = await db.Medicos.FirstOrDefaultAsync(t => t.UserName == medicoEspecialidad.Medico.UserName);

                    if (existeMedico != null)
                    {
                        TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                        {
                            Message = "No se puede agregar el registro, El campo E-mail ya existe.",
                            MessageType = GenericMessages.danger
                        };
                        ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre");
                        return View(medicoEspecialidad);
                    }

                    // Crear nuevo médico 
                    db.Medicos.Add(medicoEspecialidad.Medico);
                    await db.SaveChangesAsync();

                    Medico medicoCreado = await db.Medicos.FirstOrDefaultAsync(t => t.UserName == medicoEspecialidad.Medico.UserName);
                    Especialidad especialidad = await db.Especialidades.FirstOrDefaultAsync(t => t.Id == medicoEspecialidad.DuracionTurnoEspecialidad.EspecialidadId);
                    if (medicoCreado == null || especialidad == null)
                    {
                        TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                        {
                            Message = "Error al crear nuevo médico.",
                            MessageType = GenericMessages.danger
                        };
                        //hacemos rollback
                        dbContextTransaction.Rollback();
                        ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre");
                        return View(medicoEspecialidad);
                    }

                    DuracionTurnoEspecialidad duracionTurnoEspecialidad = new DuracionTurnoEspecialidad
                    {
                        MedicoId = medicoCreado.Id,
                        EspecialidadId = especialidad.Id,
                        Duracion = medicoEspecialidad.DuracionTurnoEspecialidad.Duracion
                    };

                    db.DuracionTurnoEspecialidades.Add(duracionTurnoEspecialidad);
                    await db.SaveChangesAsync();

                    // Crear usuario y asignar rol
                    var store = new UserStore<ApplicationUser>(db);
                    var userManager = new UserManager<ApplicationUser>(store);
                    var user = new ApplicationUser { 
                        UserName = medicoEspecialidad.Medico.UserName, 
                        FirstName = medicoEspecialidad.Medico.Nombre,
                        LastName = medicoEspecialidad.Medico.Apellido
                    };
                    userManager.Create(user, "Medico1234@");
                    userManager.AddToRole(user.Id, "Medico");

                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "Registro agregado exitosamente.",
                        MessageType = GenericMessages.success
                    };

                    //Hacemos commit de todos los datos
                    dbContextTransaction.Commit();
                    return RedirectToAction("Index");
                } 
                catch (Exception ex)
                {

                    var err = $"No se puede agregar el registro: {ex.Message}";
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = err,
                        MessageType = GenericMessages.danger
                    };

                    //hacemos rollback si hay excepción
                    dbContextTransaction.Rollback();
                    ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre");
                    return View(medicoEspecialidad);
                }
            }
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            Medico medico = await db.Medicos.FindAsync(id);

            if (medico == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }
           
            return View(medico);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Medico medico)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(medico).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "Registro editado exitosamante.",
                        MessageType = GenericMessages.success
                    };
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "El modelo no es válido",
                        MessageType = GenericMessages.danger
                    };

                    return View(medico);
                }
            }
            catch (Exception ex)
            {

                var err = $"No se puede editar el registro: {ex.Message}";
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = err,
                    MessageType = GenericMessages.danger
                };

                return View(medico);
            }
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            Medico medico = await db.Medicos.FindAsync(id);

            if (medico == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            return View(medico);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            //creamos el ámbito de la transacción
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    Medico medico = await db.Medicos.FindAsync(id);
                    if (medico == null)
                    {
                        TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                        {
                            Message = "El médico no existe.",
                            MessageType = GenericMessages.danger
                        };
                        return RedirectToAction("Index");
                    }

                    var hoy = DateTime.Now.Date;
                    Turno existeTurnoCreado = await db.Turnos.FirstOrDefaultAsync(t => t.MedicoId == medico.Id && DbFunctions.TruncateTime(t.FechaHora) >= hoy);
                    if (existeTurnoCreado != null)
                    {
                        TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                        {
                            Message = "El médico no se puede dar de baja, tiene habilitada la agenda.",
                            MessageType = GenericMessages.danger
                        };
                        return RedirectToAction("Index");
                    }

                    Turno existeTurnoAntiguo = await db.Turnos.FirstOrDefaultAsync(t => t.MedicoId == medico.Id && DbFunctions.TruncateTime(t.FechaHora) < hoy);
                    if (existeTurnoAntiguo != null)
                    {
                        //medico.DeleteAt = DateTime.Now;
                        //db.Entry(medico).State = EntityState.Modified;
                        //await db.SaveChangesAsync();

                        TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                        {
                            Message = "Ups, hay problemas al dar de baja un médico con turnos antiguos, No te preocupes!! estamos trabajando para solucionarlo.",
                            //MessageType = GenericMessages.success
                            MessageType = GenericMessages.danger
                        };
                        return RedirectToAction("Index");
                    }

                    // primero eliminamos las especialidades DuracionTurnoEspecialidad asociadas
                    IEnumerable<DuracionTurnoEspecialidad> especialidades = await db.DuracionTurnoEspecialidades.Where(t => t.MedicoId == medico.Id).ToListAsync();
                    if (especialidades.Count() > 0)
                    {
                        db.DuracionTurnoEspecialidades.RemoveRange(especialidades);
                        //await db.SaveChangesAsync();
                    }

                    // luego eliminamos los consultorios AtencionHorario asociados
                    IEnumerable<AtencionHorario> consultorios = await db.AtencionHorarios.Where(t => t.MedicoId == medico.Id).ToListAsync();
                    if (consultorios.Count() > 0)
                    {
                        db.AtencionHorarios.RemoveRange(consultorios);
                        //await db.SaveChangesAsync();
                    }


                    db.Medicos.Remove(medico);
                    await db.SaveChangesAsync();

                    //Hacemos commit de todos los datos
                    dbContextTransaction.Commit();

                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "Baja de médico exitosa. Eliminado",
                        MessageType = GenericMessages.success
                    };
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //hacemos rollback si hay excepción
                    dbContextTransaction.Rollback();
                    var err = $"Error al eliminar Médico: {ex.Message}";
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = err,
                        MessageType = GenericMessages.danger
                    };
                    return RedirectToAction("Index");
                }
            }
        }

        public async Task<ActionResult> EspecialidadesDeMedico(int? medicoId)
        {
            if (medicoId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            Medico medico = await db.Medicos.FindAsync(medicoId);

            if (medico == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            var duracionTurnoEspecialidades = db.DuracionTurnoEspecialidades.Include(d => d.Especialidad)
                                                                            .Include(d => d.Medico)
                                                                            .Where(t => t.MedicoId == medicoId);
            return View(await duracionTurnoEspecialidades.ToListAsync());
        }

        public async Task<ActionResult> CreateDuracionTurnoEspecialidad(int? medicoId)
        {
            if (medicoId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            Medico medico = await db.Medicos.FindAsync(medicoId);

            if (medico == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre");
            DuracionTurnoEspecialidad dte = new DuracionTurnoEspecialidad { MedicoId = medico.Id };
            return View(dte);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDuracionTurnoEspecialidad([Bind(Include = "Id,MedicoId,EspecialidadId,Duracion")] DuracionTurnoEspecialidad duracionTurnoEspecialidad)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DuracionTurnoEspecialidad dte = await db.DuracionTurnoEspecialidades.FirstOrDefaultAsync(t => t.MedicoId == duracionTurnoEspecialidad.MedicoId &&
                                                                                                                  t.EspecialidadId == duracionTurnoEspecialidad.EspecialidadId);

                    if (dte != null)
                    {
                        TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                        {
                            Message = "El médico ya tiene la especialidad elegida.",
                            MessageType = GenericMessages.warning
                        };
                        ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", duracionTurnoEspecialidad.EspecialidadId);
                        return View(duracionTurnoEspecialidad);
                    }

                    db.DuracionTurnoEspecialidades.Add(duracionTurnoEspecialidad);
                    await db.SaveChangesAsync();
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "Registro agregado a la base de datos.",
                        MessageType = GenericMessages.success
                    };
                    return RedirectToAction("EspecialidadesDeMedico", new {medicoId = duracionTurnoEspecialidad.MedicoId });
                }
                else
                {
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "El modelo no es válido",
                        MessageType = GenericMessages.danger
                    };

                    ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", duracionTurnoEspecialidad.EspecialidadId);
                    return View(duracionTurnoEspecialidad);
                }
            }
            catch (Exception ex)
            {

                var err = $"No se puede agregar el registro: {ex.Message}";
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = err,
                    MessageType = GenericMessages.danger
                };

                ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", duracionTurnoEspecialidad.EspecialidadId);
                return View(duracionTurnoEspecialidad);
            }

        }

        public async Task<ActionResult> EditDuracionTurnoEspecialidad(int? id)
        {
            if (id == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            DuracionTurnoEspecialidad duracionTurnoEspecialidad = await db.DuracionTurnoEspecialidades.FindAsync(id);
            
            if (duracionTurnoEspecialidad == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", duracionTurnoEspecialidad.EspecialidadId);
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", duracionTurnoEspecialidad.MedicoId);

            return View(duracionTurnoEspecialidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDuracionTurnoEspecialidad([Bind(Include = "Id,MedicoId,EspecialidadId,Duracion")] DuracionTurnoEspecialidad duracionTurnoEspecialidad)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (duracionTurnoEspecialidad.Duracion <= 9)
                    {
                        TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                        {
                            Message = "Error al validar duración del turno, la duración debe ser mayor o igual a 10 minutos",
                            MessageType = GenericMessages.danger
                        };
                        ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", duracionTurnoEspecialidad.EspecialidadId);
                        ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", duracionTurnoEspecialidad.MedicoId);
                        return View(duracionTurnoEspecialidad);
                    }

                    db.Entry(duracionTurnoEspecialidad).State = EntityState.Modified;

                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "Registro editado exitosamante.",
                        MessageType = GenericMessages.success
                    };

                    await db.SaveChangesAsync();
                    return RedirectToAction("EspecialidadesDeMedico", new { medicoId = duracionTurnoEspecialidad.MedicoId });
                }
                else
                {
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "Error al validar algunos de los campos.",
                        MessageType = GenericMessages.danger
                    };

                    ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", duracionTurnoEspecialidad.EspecialidadId);
                    return View(duracionTurnoEspecialidad);
                }
            }
            catch (Exception ex)
            {
                var err = $"No se puede agregar el registro: {ex.Message}";
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = err,
                    MessageType = GenericMessages.danger
                };

                ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", duracionTurnoEspecialidad.EspecialidadId);
                return View(duracionTurnoEspecialidad);
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
