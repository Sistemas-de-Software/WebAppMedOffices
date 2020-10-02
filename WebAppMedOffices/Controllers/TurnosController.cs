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

namespace WebAppMedOffices.Controllers
{
    public class TurnosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Turnos
        public async Task<ActionResult> Index()
        {
            var turnos = db.Turnos.Include(t => t.Especialidad).Include(t => t.Medico).Include(t => t.ObraSocial);
            return View(await turnos.ToListAsync());
        }

        // GET: Turnos/Details/5
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

        // GET: Turnos/Create
        public ActionResult Create()
        {
            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre");
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre");
            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre");
            return View();
        }

        // POST: Turnos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,MedicoId,EspecialidadId,ObraSocialId,Estado,FechaHora,Costo,Sobreturno,TieneObraSocial")] Turno turno)
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

        // GET: Turnos/Edit/5
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

        // POST: Turnos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,MedicoId,EspecialidadId,ObraSocialId,Estado,FechaHora,Costo,Sobreturno,TieneObraSocial")] Turno turno)
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

        // GET: Turnos/Delete/5
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

        // POST: Turnos/Delete/5
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
