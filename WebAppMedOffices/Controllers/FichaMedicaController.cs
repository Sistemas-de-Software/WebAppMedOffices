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

namespace WebAppMedOffices.Controllers
{
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

        //Pacientes Del Dia
        public async Task<ActionResult> ListarPacientesHoy()
        {
            var hoy = DateTime.Now.Date;
            var pacientes = db.Turnos.Include(t => t.Medico).Include(t => t.ObraSocial).Where(t => t.Estado == Estado.Reservado && DbFunctions.TruncateTime(t.FechaHora) == hoy);
            return View(await pacientes.ToListAsync());            
        }

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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Paciente paciente = await db.Pacientes.FindAsync(id);

            if (paciente == null)
            {
                return HttpNotFound();
            }

            ViewBag.TipoEnfermedades = await db.TipoEnfermedades.ToListAsync();
            ViewBag.TurnoId = db.PacienteEnfermedades.Include(t => t.Enfermedad).Where(t => t.PacienteId == paciente.Id);

            return View(paciente);
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
