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
using WebAppMedOffices.Shared;
using Microsoft.AspNet.Identity;
using WebAppMedOffices.Constants;
using Rotativa;

namespace WebAppMedOffices.Controllers
{
    [Authorize(Roles = "Medico")]
    public class FichaMedicaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FichaMedica
        public ActionResult Index()
        {
           
            List<SelectListItem> lst = new List<SelectListItem>();

            using (Models.ApplicationDbContext db = new Models.ApplicationDbContext())
            {
                lst = (from d in db.Especialidades
                       select new SelectListItem
                       {
                           Value = d.Id.ToString(),
                           Text = d.Nombre
                       }).ToList();
            }

            return View(lst);
        }

        //Todos los pacientes
        public async Task<ActionResult> ListarTodosPacientes()
        {
            return View(await db.Pacientes.ToListAsync());
        }

        //Turnos del Dia
        public async Task<ActionResult> ListarPacientesHoy()
        {
            var userName = User.Identity.GetUserName();
            var hoy = DateTime.Now.Date;
            var turnos = db.Turnos.Include(t => t.Medico).Include(t => t.ObraSocial)
                .Where(t =>
                    t.Estado == Estado.Reservado &&
                    DbFunctions.TruncateTime(t.FechaHora) == hoy &&
                    t.Medico.UserName == userName
                    ||
                    t.Estado == Estado.Atendido &&
                    DbFunctions.TruncateTime(t.FechaHora) == hoy &&
                    t.Medico.UserName == userName);
            
            return View(await turnos.ToListAsync());            
        }

        // NO SE USA
        public ActionResult HistoriaClinica(int idPaciente)
        {

            List<SelectListItem> lst = new List<SelectListItem>();
            var enfermedades = db.PacienteEnfermedades.Include(t => t.Enfermedad).Include(t => t.EnfermedadId).Where(t => t.PacienteId == idPaciente);
            ViewBag.detalle = db.HistoriaClinicas.Where(t => t.PacienteId == idPaciente);
            ViewBag.comentario = db.HistoriaClinicas.Where(t => t.PacienteId == idPaciente);
            return View(enfermedades);
        }

        public async Task<ActionResult> FichaMedica(int? id)
        {
            if (id == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListarTodosPacientes");
            }

            Paciente paciente = await db.Pacientes.FindAsync(id);

            if (paciente == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListarTodosPacientes");
            }

            ViewBag.TipoEnfermedades = await db.TipoEnfermedades.ToListAsync();

            return View(paciente);
        }

        public async Task<ActionResult> FichaMedicaConAgregarHistoriaClinica(int? pacienteId, int? turnoId)
        {
            if (pacienteId == null || turnoId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListarPacientesHoy");
            }

            Paciente paciente = await db.Pacientes.FindAsync(pacienteId);
            Turno turno = await db.Turnos.FirstOrDefaultAsync(t => t.Id == turnoId && t.PacienteId == pacienteId);

            if (paciente == null || turno == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListarPacientesHoy");
            }

            ViewBag.TipoEnfermedades = await db.TipoEnfermedades.ToListAsync();
            ViewBag.Turno = turno;

            return View(paciente);
        }

        // Este controlador es público, y es una copia del anterior,
        // solo para mostrar la vista que se va a descargar como PDF
        [AllowAnonymous]
        public async Task<ActionResult> FichaMedicaConAgregarHistoriaClinicaCopy(int? pacienteId, int? turnoId)
        {
            if (pacienteId == null || turnoId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListarPacientesHoy");
            }

            Paciente paciente = await db.Pacientes.FindAsync(pacienteId);
            Turno turno = await db.Turnos.FirstOrDefaultAsync(t => t.Id == turnoId && t.PacienteId == pacienteId);

            if (paciente == null || turno == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListarPacientesHoy");
            }

            ViewBag.TipoEnfermedades = await db.TipoEnfermedades.ToListAsync();
            ViewBag.Turno = turno;

            return View(paciente);
        }

        public ActionResult PrintFichaMedica(int pacienteId, int turnoId)
        {
            return new ActionAsPdf("FichaMedicaConAgregarHistoriaClinicaCopy", new { pacienteId = pacienteId, turnoId = turnoId }) { FileName = "ficha-medica.pdf" };
        }

        public async Task<ActionResult> AgregarHistoriaClinica(int? id)
        {
            if (id == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListarPacientesHoy");
            }

            Turno turno = await db.Turnos.FindAsync(id);
            
            if (turno == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListarPacientesHoy");
            }

            turno.Estado = Estado.Atendido;

            return View(turno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarHistoriaClinica(Turno turno)
        {
            if (ModelState.IsValid)
            {
                db.Entry(turno).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("FichaMedicaConAgregarHistoriaClinica", new { pacienteId = turno.PacienteId, turnoId = turno.Id });
            }

            return View(turno);
        }

        public async Task<ActionResult> PacienteEnfermedades(int? pacienteId, int? turnoId)
        {
            if (pacienteId == null || turnoId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListarPacientesHoy");
            }

            Turno turno = await db.Turnos.FirstOrDefaultAsync(t => t.Id == turnoId && t.PacienteId == pacienteId);

            if (turno == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListarPacientesHoy");
            }

            ViewBag.TurnoId = turnoId;
            ViewBag.PacienteId = pacienteId;
            var pacienteEnfermedades = db.PacienteEnfermedades.Include(p => p.Enfermedad).Include(p => p.Paciente).Where(t => t.PacienteId == pacienteId);
            return View(await pacienteEnfermedades.ToListAsync());
        }

        public async Task<ActionResult> CreatePacienteEnfermedades(int? pacienteId, int? turnoId)
        {
            if (pacienteId == null || turnoId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListarPacientesHoy");
            }

            Turno turno = await db.Turnos.FirstOrDefaultAsync(t => t.Id == turnoId && t.PacienteId == pacienteId);

            if (turno == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListarPacientesHoy");
            }

            ViewBag.EnfermedadId = new SelectList(db.Enfermedades, "Id", "Nombre");
            var pacienteEnfermedad = new PacienteEnfermedadView { PacienteId = pacienteId.Value, TurnoId = turno.Id };
            return View(pacienteEnfermedad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePacienteEnfermedades(PacienteEnfermedadView pacienteEnfermedad)
        {
            try
            {
                PacienteEnfermedad pacienteEnfermedadEncontrado = await db.PacienteEnfermedades.Where(t => t.EnfermedadId == pacienteEnfermedad.EnfermedadId && t.PacienteId == pacienteEnfermedad.PacienteId).FirstOrDefaultAsync();
                if (pacienteEnfermedadEncontrado != null)
                {
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "Ya existe el registro.",
                        MessageType = GenericMessages.warning
                    };
                    ViewBag.EnfermedadId = new SelectList(db.Enfermedades, "Id", "Nombre", pacienteEnfermedad.EnfermedadId);
                    return View(pacienteEnfermedad);
                }
                
                if (ModelState.IsValid)
                {
                    PacienteEnfermedad nuevoPacienteEnfermedad = new PacienteEnfermedad { EnfermedadId = pacienteEnfermedad.EnfermedadId, PacienteId = pacienteEnfermedad.PacienteId };
                    db.PacienteEnfermedades.Add(nuevoPacienteEnfermedad);
                    await db.SaveChangesAsync();
                    return RedirectToAction("FichaMedicaConAgregarHistoriaClinica", new { pacienteId = pacienteEnfermedad.PacienteId, turnoId = pacienteEnfermedad.TurnoId });
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
                ViewBag.EnfermedadId = new SelectList(db.Enfermedades, "Id", "Nombre", pacienteEnfermedad.EnfermedadId);
                return View(pacienteEnfermedad);
            }

            ViewBag.EnfermedadId = new SelectList(db.Enfermedades, "Id", "Nombre", pacienteEnfermedad.EnfermedadId);
            return View(pacienteEnfermedad);
        }

        public async Task<ActionResult> EditPacienteEnfermedades(int? id, int? pacienteId, int? turnoId)
        {
            if (id == null || turnoId == null || pacienteId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListarPacientesHoy");
            }

            Turno turno = await db.Turnos.FirstOrDefaultAsync(t => t.Id == turnoId && t.PacienteId == pacienteId);
            PacienteEnfermedad pacienteEnfermedadEncontrado = await db.PacienteEnfermedades.FindAsync(id);
            
            if (pacienteEnfermedadEncontrado == null || turno == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListarPacientesHoy");
            }

            ViewBag.EnfermedadId = new SelectList(db.Enfermedades, "Id", "Nombre", pacienteEnfermedadEncontrado.EnfermedadId);
            var pacienteEnfermedad = new PacienteEnfermedadView { Id = pacienteEnfermedadEncontrado.Id, PacienteId = pacienteEnfermedadEncontrado.PacienteId, TurnoId = turno.Id };
            return View(pacienteEnfermedad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPacienteEnfermedades(PacienteEnfermedadView pacienteEnfermedad)
        {
            try
            {
                PacienteEnfermedad pacienteEnfermedadEncontrado = await db.PacienteEnfermedades.Where(t => t.EnfermedadId == pacienteEnfermedad.EnfermedadId && t.PacienteId == pacienteEnfermedad.PacienteId).FirstOrDefaultAsync();
                if (pacienteEnfermedadEncontrado != null)
                {
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "Ya existe el registro.",
                        MessageType = GenericMessages.warning
                    };
                    ViewBag.EnfermedadId = new SelectList(db.Enfermedades, "Id", "Nombre", pacienteEnfermedad.EnfermedadId);
                    return View(pacienteEnfermedad);
                }

                if (ModelState.IsValid)
                {
                    PacienteEnfermedad modificadoPacienteEnfermedad = new PacienteEnfermedad { Id = pacienteEnfermedad.Id, EnfermedadId = pacienteEnfermedad.EnfermedadId, PacienteId = pacienteEnfermedad.PacienteId };
                    db.Entry(modificadoPacienteEnfermedad).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("FichaMedicaConAgregarHistoriaClinica", new { pacienteId = pacienteEnfermedad.PacienteId, turnoId = pacienteEnfermedad.TurnoId });
                }
            }
            catch (Exception ex)
            {
                var err = $"No se puede modificar el registro: {ex.Message}";
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = err,
                    MessageType = GenericMessages.danger
                };
                ViewBag.EnfermedadId = new SelectList(db.Enfermedades, "Id", "Nombre", pacienteEnfermedad.EnfermedadId);
                return View(pacienteEnfermedad);
            }

            ViewBag.EnfermedadId = new SelectList(db.Enfermedades, "Id", "Nombre", pacienteEnfermedad.EnfermedadId);
            return View(pacienteEnfermedad);
        }

        [HttpGet]
        public JsonResult Medico(int IdEspecialidad)
        {
            List<ElementJsonIntKey> lst = new List<ElementJsonIntKey>();
            using (Models.ApplicationDbContext db = new Models.ApplicationDbContext())
            {
                lst = (from d in db.DuracionTurnoEspecialidades
                       where d.EspecialidadId == IdEspecialidad
                       select new ElementJsonIntKey
                       {
                           Value = d.Medico.Id,
                           Text = d.Medico.Nombre
                       }).ToList();
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }



        public class ElementJsonIntKey
        {
            public int Value { get; set; }
            public string Text { get; set; }
        }
        // GET: FichaMedica/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistoriaClinica historiaClinica = await db.HistoriaClinicas.FindAsync(id);
            if (historiaClinica == null)
            {
                return HttpNotFound();
            }
            return View(historiaClinica);
        }

        // GET: FichaMedica/Create
        public ActionResult Create()
        {
            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre");
            ViewBag.TurnoId = new SelectList(db.Turnos, "Id", "Id");
            return View();
        }

        // POST: FichaMedica/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,PacienteId,TurnoId,Motivo,Detalle,Comentario")] HistoriaClinica historiaClinica)
        {
            if (ModelState.IsValid)
            {
                db.HistoriaClinicas.Add(historiaClinica);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre", historiaClinica.PacienteId);
            ViewBag.TurnoId = new SelectList(db.Turnos, "Id", "Id", historiaClinica.TurnoId);
            return View(historiaClinica);
        }

        // GET: FichaMedica/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistoriaClinica historiaClinica = await db.HistoriaClinicas.FindAsync(id);
            if (historiaClinica == null)
            {
                return HttpNotFound();
            }
            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre", historiaClinica.PacienteId);
            ViewBag.TurnoId = new SelectList(db.Turnos, "Id", "Id", historiaClinica.TurnoId);
            return View(historiaClinica);
        }

        // POST: FichaMedica/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PacienteId,TurnoId,Motivo,Detalle,Comentario")] HistoriaClinica historiaClinica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(historiaClinica).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre", historiaClinica.PacienteId);
            ViewBag.TurnoId = new SelectList(db.Turnos, "Id", "Id", historiaClinica.TurnoId);
            return View(historiaClinica);
        }

        // GET: FichaMedica/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistoriaClinica historiaClinica = await db.HistoriaClinicas.FindAsync(id);
            if (historiaClinica == null)
            {
                return HttpNotFound();
            }
            return View(historiaClinica);
        }

        // POST: FichaMedica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HistoriaClinica historiaClinica = await db.HistoriaClinicas.FindAsync(id);
            db.HistoriaClinicas.Remove(historiaClinica);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
