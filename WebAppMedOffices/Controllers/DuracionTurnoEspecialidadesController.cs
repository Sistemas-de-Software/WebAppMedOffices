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
    public class DuracionTurnoEspecialidadesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DuracionTurnoEspecialidades
        public async Task<ActionResult> Index()
        {
            var duracionTurnoEspecialidades = db.DuracionTurnoEspecialidades.Include(d => d.Especialidad).Include(d => d.Medico);
            return View(await duracionTurnoEspecialidades.ToListAsync());
        }

        // GET: DuracionTurnoEspecialidades/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DuracionTurnoEspecialidad duracionTurnoEspecialidad = await db.DuracionTurnoEspecialidades.FindAsync(id);
            if (duracionTurnoEspecialidad == null)
            {
                return HttpNotFound();
            }
            return View(duracionTurnoEspecialidad);
        }

        // GET: DuracionTurnoEspecialidades/Create
        public ActionResult Create()
        {
            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre");
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre");
            return View();
        }

        // POST: DuracionTurnoEspecialidades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,MedicoId,EspecialidadId,Duracion")] DuracionTurnoEspecialidad duracionTurnoEspecialidad)
        {
            if (ModelState.IsValid)
            {
                db.DuracionTurnoEspecialidades.Add(duracionTurnoEspecialidad);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", duracionTurnoEspecialidad.EspecialidadId);
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", duracionTurnoEspecialidad.MedicoId);
            return View(duracionTurnoEspecialidad);
        }

        // GET: DuracionTurnoEspecialidades/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DuracionTurnoEspecialidad duracionTurnoEspecialidad = await db.DuracionTurnoEspecialidades.FindAsync(id);
            if (duracionTurnoEspecialidad == null)
            {
                return HttpNotFound();
            }
            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", duracionTurnoEspecialidad.EspecialidadId);
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", duracionTurnoEspecialidad.MedicoId);
            return View(duracionTurnoEspecialidad);
        }

        // POST: DuracionTurnoEspecialidades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,MedicoId,EspecialidadId,Duracion")] DuracionTurnoEspecialidad duracionTurnoEspecialidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(duracionTurnoEspecialidad).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", duracionTurnoEspecialidad.EspecialidadId);
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", duracionTurnoEspecialidad.MedicoId);
            return View(duracionTurnoEspecialidad);
        }

        // GET: DuracionTurnoEspecialidades/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DuracionTurnoEspecialidad duracionTurnoEspecialidad = await db.DuracionTurnoEspecialidades.FindAsync(id);
            if (duracionTurnoEspecialidad == null)
            {
                return HttpNotFound();
            }
            return View(duracionTurnoEspecialidad);
        }

        // POST: DuracionTurnoEspecialidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DuracionTurnoEspecialidad duracionTurnoEspecialidad = await db.DuracionTurnoEspecialidades.FindAsync(id);
            db.DuracionTurnoEspecialidades.Remove(duracionTurnoEspecialidad);
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
