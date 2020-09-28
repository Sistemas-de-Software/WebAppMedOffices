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
    public class PacienteTurnosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PacienteTurnos
        public async Task<ActionResult> Index()
        {
            var pacienteTurnos = db.PacienteTurnos.Include(p => p.Paciente).Include(p => p.Turno);
            return View(await pacienteTurnos.ToListAsync());
        }

        // GET: PacienteTurnos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PacienteTurno pacienteTurno = await db.PacienteTurnos.FindAsync(id);
            if (pacienteTurno == null)
            {
                return HttpNotFound();
            }
            return View(pacienteTurno);
        }

        // GET: PacienteTurnos/Create
        public ActionResult Create()
        {
            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre");
            ViewBag.TurnoId = new SelectList(db.Turnos, "Id", "Id");
            return View();
        }

        // POST: PacienteTurnos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,PacienteId,TurnoId")] PacienteTurno pacienteTurno)
        {
            if (ModelState.IsValid)
            {
                db.PacienteTurnos.Add(pacienteTurno);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre", pacienteTurno.PacienteId);
            ViewBag.TurnoId = new SelectList(db.Turnos, "Id", "Id", pacienteTurno.TurnoId);
            return View(pacienteTurno);
        }

        // GET: PacienteTurnos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PacienteTurno pacienteTurno = await db.PacienteTurnos.FindAsync(id);
            if (pacienteTurno == null)
            {
                return HttpNotFound();
            }
            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre", pacienteTurno.PacienteId);
            ViewBag.TurnoId = new SelectList(db.Turnos, "Id", "Id", pacienteTurno.TurnoId);
            return View(pacienteTurno);
        }

        // POST: PacienteTurnos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PacienteId,TurnoId")] PacienteTurno pacienteTurno)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pacienteTurno).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre", pacienteTurno.PacienteId);
            ViewBag.TurnoId = new SelectList(db.Turnos, "Id", "Id", pacienteTurno.TurnoId);
            return View(pacienteTurno);
        }

        // GET: PacienteTurnos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PacienteTurno pacienteTurno = await db.PacienteTurnos.FindAsync(id);
            if (pacienteTurno == null)
            {
                return HttpNotFound();
            }
            return View(pacienteTurno);
        }

        // POST: PacienteTurnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PacienteTurno pacienteTurno = await db.PacienteTurnos.FindAsync(id);
            db.PacienteTurnos.Remove(pacienteTurno);
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
