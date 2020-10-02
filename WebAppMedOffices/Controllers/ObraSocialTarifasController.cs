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
    [Authorize(Roles = "Secretaria,Admin")]
    public class ObraSocialTarifasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var obraSocialTarifas = db.ObraSocialTarifas.Include(o => o.Especialidad).Include(o => o.ObraSocial);
            return View(await obraSocialTarifas.ToListAsync());
        }

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

        public ActionResult Create()
        {
            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre");
            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre");
            return View();
        }

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
