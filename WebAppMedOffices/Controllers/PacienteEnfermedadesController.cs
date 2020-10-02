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
    public class PacienteEnfermedadesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PacienteEnfermedades
        public async Task<ActionResult> Index()
        {
            var pacienteEnfermedades = db.PacienteEnfermedades.Include(p => p.Enfermedad).Include(p => p.Paciente);
            return View(await pacienteEnfermedades.ToListAsync());
        }

        // GET: PacienteEnfermedades/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PacienteEnfermedad pacienteEnfermedad = await db.PacienteEnfermedades.FindAsync(id);
            if (pacienteEnfermedad == null)
            {
                return HttpNotFound();
            }
            return View(pacienteEnfermedad);
        }

        // GET: PacienteEnfermedades/Create
        public ActionResult Create()
        {
            ViewBag.EnfermedadId = new SelectList(db.Enfermedades, "Id", "Nombre");
            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre");
            return View();
        }

        // POST: PacienteEnfermedades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,PacienteId,EnfermedadId,Descripcion")] PacienteEnfermedad pacienteEnfermedad)
        {
            if (ModelState.IsValid)
            {
                db.PacienteEnfermedades.Add(pacienteEnfermedad);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EnfermedadId = new SelectList(db.Enfermedades, "Id", "Nombre", pacienteEnfermedad.EnfermedadId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre", pacienteEnfermedad.PacienteId);
            return View(pacienteEnfermedad);
        }

        // GET: PacienteEnfermedades/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PacienteEnfermedad pacienteEnfermedad = await db.PacienteEnfermedades.FindAsync(id);
            if (pacienteEnfermedad == null)
            {
                return HttpNotFound();
            }
            ViewBag.EnfermedadId = new SelectList(db.Enfermedades, "Id", "Nombre", pacienteEnfermedad.EnfermedadId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre", pacienteEnfermedad.PacienteId);
            return View(pacienteEnfermedad);
        }

        // POST: PacienteEnfermedades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PacienteId,EnfermedadId,Descripcion")] PacienteEnfermedad pacienteEnfermedad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pacienteEnfermedad).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EnfermedadId = new SelectList(db.Enfermedades, "Id", "Nombre", pacienteEnfermedad.EnfermedadId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre", pacienteEnfermedad.PacienteId);
            return View(pacienteEnfermedad);
        }

        // GET: PacienteEnfermedades/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PacienteEnfermedad pacienteEnfermedad = await db.PacienteEnfermedades.FindAsync(id);
            if (pacienteEnfermedad == null)
            {
                return HttpNotFound();
            }
            return View(pacienteEnfermedad);
        }

        // POST: PacienteEnfermedades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PacienteEnfermedad pacienteEnfermedad = await db.PacienteEnfermedades.FindAsync(id);
            db.PacienteEnfermedades.Remove(pacienteEnfermedad);
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
