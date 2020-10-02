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
    public class PacientesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pacientes
        public async Task<ActionResult> Index()
        {
            return View(await db.Pacientes.ToListAsync());
        }

        // GET: Pacientes/Details/5
        public async Task<ActionResult> Details(int? id)
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
            return View(paciente);
        }

        public ActionResult Create()
        {
            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                db.Pacientes.Add(paciente);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(paciente);
        }

        public async Task<ActionResult> Edit(int? id)
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

            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre", paciente.ObraSocialId);
            return View(paciente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paciente).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre", paciente.ObraSocialId);
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Paciente paciente = await db.Pacientes.FindAsync(id);
            db.Pacientes.Remove(paciente);
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
