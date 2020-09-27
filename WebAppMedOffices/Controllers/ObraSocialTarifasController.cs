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
    public class ObraSocialTarifasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ObraSocialTarifas
        public async Task<ActionResult> Index()
        {
            var obraSocialTarifas = db.ObraSocialTarifas.Include(o => o.Especialidad).Include(o => o.ObraSocial);
            return View(await obraSocialTarifas.ToListAsync());
        }

        // GET: ObraSocialTarifas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraSocialTarifa obraSocialTarifa = await db.ObraSocialTarifas.FindAsync(id);
            if (obraSocialTarifa == null)
            {
                return HttpNotFound();
            }
            return View(obraSocialTarifa);
        }

        // GET: ObraSocialTarifas/Create
        public ActionResult Create()
        {
            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre");
            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre");
            return View();
        }

        // POST: ObraSocialTarifas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Tarifa,ObraSocialId,EspecialidadId")] ObraSocialTarifa obraSocialTarifa)
        {
            if (ModelState.IsValid)
            {
                db.ObraSocialTarifas.Add(obraSocialTarifa);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", obraSocialTarifa.EspecialidadId);
            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre", obraSocialTarifa.ObraSocialId);
            return View(obraSocialTarifa);
        }

        // GET: ObraSocialTarifas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraSocialTarifa obraSocialTarifa = await db.ObraSocialTarifas.FindAsync(id);
            if (obraSocialTarifa == null)
            {
                return HttpNotFound();
            }
            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", obraSocialTarifa.EspecialidadId);
            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre", obraSocialTarifa.ObraSocialId);
            return View(obraSocialTarifa);
        }

        // POST: ObraSocialTarifas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Tarifa,ObraSocialId,EspecialidadId")] ObraSocialTarifa obraSocialTarifa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obraSocialTarifa).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", obraSocialTarifa.EspecialidadId);
            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre", obraSocialTarifa.ObraSocialId);
            return View(obraSocialTarifa);
        }

        // GET: ObraSocialTarifas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraSocialTarifa obraSocialTarifa = await db.ObraSocialTarifas.FindAsync(id);
            if (obraSocialTarifa == null)
            {
                return HttpNotFound();
            }
            return View(obraSocialTarifa);
        }

        // POST: ObraSocialTarifas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ObraSocialTarifa obraSocialTarifa = await db.ObraSocialTarifas.FindAsync(id);
            db.ObraSocialTarifas.Remove(obraSocialTarifa);
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
