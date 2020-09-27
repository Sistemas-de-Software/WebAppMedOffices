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
    public class EnfermedadesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Enfermedades
        public async Task<ActionResult> Index()
        {
            var enfermedades = db.Enfermedades.Include(e => e.TipoEnfermedad);
            return View(await enfermedades.ToListAsync());
        }

        // GET: Enfermedades/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enfermedad enfermedad = await db.Enfermedades.FindAsync(id);
            if (enfermedad == null)
            {
                return HttpNotFound();
            }
            return View(enfermedad);
        }

        // GET: Enfermedades/Create
        public ActionResult Create()
        {
            ViewBag.TipoEnfermedadId = new SelectList(db.TipoEnfermedades, "Id", "Nombre");
            return View();
        }

        // POST: Enfermedades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre,TipoEnfermedadId")] Enfermedad enfermedad)
        {
            if (ModelState.IsValid)
            {
                db.Enfermedades.Add(enfermedad);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TipoEnfermedadId = new SelectList(db.TipoEnfermedades, "Id", "Nombre", enfermedad.TipoEnfermedadId);
            return View(enfermedad);
        }

        // GET: Enfermedades/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enfermedad enfermedad = await db.Enfermedades.FindAsync(id);
            if (enfermedad == null)
            {
                return HttpNotFound();
            }
            ViewBag.TipoEnfermedadId = new SelectList(db.TipoEnfermedades, "Id", "Nombre", enfermedad.TipoEnfermedadId);
            return View(enfermedad);
        }

        // POST: Enfermedades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,TipoEnfermedadId")] Enfermedad enfermedad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enfermedad).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TipoEnfermedadId = new SelectList(db.TipoEnfermedades, "Id", "Nombre", enfermedad.TipoEnfermedadId);
            return View(enfermedad);
        }

        // GET: Enfermedades/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enfermedad enfermedad = await db.Enfermedades.FindAsync(id);
            if (enfermedad == null)
            {
                return HttpNotFound();
            }
            return View(enfermedad);
        }

        // POST: Enfermedades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Enfermedad enfermedad = await db.Enfermedades.FindAsync(id);
            db.Enfermedades.Remove(enfermedad);
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
