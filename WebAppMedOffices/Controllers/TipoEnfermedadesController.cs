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
    public class TipoEnfermedadesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TipoEnfermedades
        public async Task<ActionResult> Index()
        {
            return View(await db.TipoEnfermedades.ToListAsync());
        }

        // GET: TipoEnfermedades/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEnfermedad tipoEnfermedad = await db.TipoEnfermedades.FindAsync(id);
            if (tipoEnfermedad == null)
            {
                return HttpNotFound();
            }
            return View(tipoEnfermedad);
        }

        // GET: TipoEnfermedades/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoEnfermedades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre")] TipoEnfermedad tipoEnfermedad)
        {
            if (ModelState.IsValid)
            {
                db.TipoEnfermedades.Add(tipoEnfermedad);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tipoEnfermedad);
        }

        // GET: TipoEnfermedades/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEnfermedad tipoEnfermedad = await db.TipoEnfermedades.FindAsync(id);
            if (tipoEnfermedad == null)
            {
                return HttpNotFound();
            }
            return View(tipoEnfermedad);
        }

        // POST: TipoEnfermedades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre")] TipoEnfermedad tipoEnfermedad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoEnfermedad).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tipoEnfermedad);
        }

        // GET: TipoEnfermedades/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoEnfermedad tipoEnfermedad = await db.TipoEnfermedades.FindAsync(id);
            if (tipoEnfermedad == null)
            {
                return HttpNotFound();
            }
            return View(tipoEnfermedad);
        }

        // POST: TipoEnfermedades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TipoEnfermedad tipoEnfermedad = await db.TipoEnfermedades.FindAsync(id);
            db.TipoEnfermedades.Remove(tipoEnfermedad);
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

        // Enfermedades

        public async Task<ActionResult> CreateEnfermedad(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TipoEnfermedad tipoEnfermedad = await db.TipoEnfermedades.FindAsync(id);
            
            if (tipoEnfermedad == null)
            {
                return HttpNotFound();
            }

            var view = new Enfermedad { TipoEnfermedadId = tipoEnfermedad.Id };
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateEnfermedad(Enfermedad enfermedad)
        {
            if (ModelState.IsValid)
            {
                db.Enfermedades.Add(enfermedad);
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("Details/{0}", enfermedad.TipoEnfermedadId));
            }

            return View(enfermedad);
        }
    }
}
