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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Medico medico)
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
                            Message = "El modelo no es válido",
                            MessageType = GenericMessages.danger
                        };
                        return View(medico);
                    }

                    Medico existeMedico = await db.Medicos.FirstOrDefaultAsync(t => t.UserName == medico.UserName);

                    if (existeMedico != null)
                    {
                        TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                        {
                            Message = "No se puede agregar el registro, El campo E-mail ya existe.",
                            MessageType = GenericMessages.danger
                        };
                        return View(medico);
                    }
                    
                    // Crear usuario y asignar rol
                    var store = new UserStore<ApplicationUser>(db);
                    var userManager = new UserManager<ApplicationUser>(store);
                    var user = new ApplicationUser { UserName = medico.UserName };
                    userManager.Create(user, "Medico1234@");
                    userManager.AddToRole(user.Id, "Medico");

                    // Crear nuevo médico 
                    db.Medicos.Add(medico);
                    await db.SaveChangesAsync();

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
                    return View(medico);
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

            try
            {
                Medico medico = await db.Medicos.FindAsync(id);

                //Paciente paciente = await db.Pacientes.FirstOrDefaultAsync(t => t.ObraSocialId == obraSocial.Id);
                //if (paciente != null)
                //{
                //    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                //    {
                //        Message = "No se puede eliminar el registro relacionado.",
                //        MessageType = GenericMessages.danger
                //    };
                //    return RedirectToAction("Index");
                //}

                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "Registro eliminado exitosamente.",
                    MessageType = GenericMessages.success
                };

                db.Medicos.Remove(medico);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var err = $"No se puede eliminar el registro: {ex.Message}";
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = err,
                    MessageType = GenericMessages.danger
                };
                return RedirectToAction("Index");
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
