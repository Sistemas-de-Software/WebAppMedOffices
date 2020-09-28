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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebAppMedOffices.Controllers
{
    public class GestionTurnosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public JsonResult GetEspecialidades(int medicoId)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false; // para API
                var especialidades = db.DuracionTurnoEspecialidades.Where(t => t.MedicoId == medicoId).Select(i =>
                    new { i.Id, i.EspecialidadId, i.MedicoId, i.Especialidad.Nombre });
                var json = JsonConvert.SerializeObject(especialidades);
                return Json(especialidades);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET: GestionTurnos
        public async Task<ActionResult> Index()
        {
            var turnos = db.Turnos.Include(t => t.Especialidad).Include(t => t.Medico).Include(t => t.ObraSocial);
            return View(await turnos.ToListAsync());
        }

        // GET: GestionTurnos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turno turno = await db.Turnos.FindAsync(id);
            if (turno == null)
            {
                return HttpNotFound();
            }
            return View(turno);
        }

        // GET: GestionTurnos/Create
        public async Task<ActionResult> Create()
        {
            var medicos = db.Medicos.Include(t => t.DuracionTurnoEspecialidades).OrderBy(t => t.Apellido);
            ViewBag.MedicoId = new SelectList(await medicos.ToListAsync(), "Id", "NombreCompleto");
            var duracionTurnoEspecialidades = db.DuracionTurnoEspecialidades.Where(t => t.MedicoId == medicos.FirstOrDefault().Id).Include(t => t.Especialidad).OrderBy(t => t.Especialidad.Nombre);
            ViewBag.EspecialidadId = new SelectList(await duracionTurnoEspecialidades.ToListAsync(), "EspecialidadId", "Especialidad.Nombre");
            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre");
            return View();
        }

        // POST: GestionTurnos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,MedicoId,EspecialidadId,ObraSocialId,Estado,FechaHora,FechaHoraFin,Costo,Sobreturno,TieneObraSocial")] Turno turno)
        {
            if (ModelState.IsValid)
            {
                db.Turnos.Add(turno);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", turno.EspecialidadId);
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", turno.MedicoId);
            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre", turno.ObraSocialId);
            return View(turno);
        }

        // GET: GestionTurnos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turno turno = await db.Turnos.FindAsync(id);
            if (turno == null)
            {
                return HttpNotFound();
            }
            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", turno.EspecialidadId);
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", turno.MedicoId);
            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre", turno.ObraSocialId);
            return View(turno);
        }

        // POST: GestionTurnos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,MedicoId,EspecialidadId,ObraSocialId,Estado,FechaHora,FechaHoraFin,Costo,Sobreturno,TieneObraSocial")] Turno turno)
        {
            if (ModelState.IsValid)
            {
                db.Entry(turno).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", turno.EspecialidadId);
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", turno.MedicoId);
            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre", turno.ObraSocialId);
            return View(turno);
        }

        // GET: GestionTurnos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turno turno = await db.Turnos.FindAsync(id);
            if (turno == null)
            {
                return HttpNotFound();
            }
            return View(turno);
        }

        // POST: GestionTurnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Turno turno = await db.Turnos.FindAsync(id);
            db.Turnos.Remove(turno);
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
