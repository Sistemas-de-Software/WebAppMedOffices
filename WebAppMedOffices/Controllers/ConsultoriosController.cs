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
    public class ConsultoriosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Consultorios
        public async Task<ActionResult> Index()
        {
            return View(await db.Consultorios.ToListAsync());
        }

        // GET: Consultorios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultorio consultorio = await db.Consultorios.FindAsync(id);
            if (consultorio == null)
            {
                return HttpNotFound();
            }
            return View(consultorio);
        }

        // GET: Consultorios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Consultorios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre")] Consultorio consultorio)
        {
            if (ModelState.IsValid)
            {
                db.Consultorios.Add(consultorio);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(consultorio);
        }

        // GET: Consultorios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultorio consultorio = await db.Consultorios.FindAsync(id);
            if (consultorio == null)
            {
                return HttpNotFound();
            }
            return View(consultorio);
        }

        // POST: Consultorios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre")] Consultorio consultorio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consultorio).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(consultorio);
        }

        // GET: Consultorios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultorio consultorio = await db.Consultorios.FindAsync(id);
            if (consultorio == null)
            {
                return HttpNotFound();
            }
            return View(consultorio);
        }

        // POST: Consultorios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Consultorio consultorio = await db.Consultorios.FindAsync(id);
            db.Consultorios.Remove(consultorio);
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
