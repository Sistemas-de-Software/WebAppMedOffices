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
    public class ObrasSocialesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            return View(await db.ObrasSociales.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraSocial obraSocial = await db.ObrasSociales.FindAsync(id);
            if (obraSocial == null)
            {
                return HttpNotFound();
            }
            return View(obraSocial);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre,Telefono,Email")] ObraSocial obraSocial)
        {
            if (ModelState.IsValid)
            {
                db.ObrasSociales.Add(obraSocial);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(obraSocial);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraSocial obraSocial = await db.ObrasSociales.FindAsync(id);
            if (obraSocial == null)
            {
                return HttpNotFound();
            }
            return View(obraSocial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre,Telefono,Email")] ObraSocial obraSocial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obraSocial).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(obraSocial);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObraSocial obraSocial = await db.ObrasSociales.FindAsync(id);
            if (obraSocial == null)
            {
                return HttpNotFound();
            }
            return View(obraSocial);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ObraSocial obraSocial = await db.ObrasSociales.FindAsync(id);
            db.ObrasSociales.Remove(obraSocial);
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
