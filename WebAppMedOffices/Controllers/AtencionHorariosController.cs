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
using WebAppMedOffices.Constants;

namespace WebAppMedOffices.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AtencionHorariosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AtencionHorarios
        public async Task<ActionResult> Index()
        {
            var atencionHorarios = db.AtencionHorarios.Include(a => a.Consultorio).Include(a => a.Medico);
            return View(await atencionHorarios.ToListAsync());
        }

        // GET: AtencionHorarios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AtencionHorario atencionHorario = await db.AtencionHorarios.FindAsync(id);
            if (atencionHorario == null)
            {
                return HttpNotFound();
            }
            return View(atencionHorario);
        }

        // GET: AtencionHorarios/Create
        public ActionResult Create()
        {
            ViewBag.ConsultorioId = new SelectList(db.Consultorios, "Id", "Nombre");
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "NombreCompleto");
            return View();
        }

        // POST: AtencionHorarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ConsultorioId,MedicoId,Dia,HoraInicio,HoraFin")] AtencionHorario atencionHorario)
        {
            var todosHorarios = db.AtencionHorarios.Include(a => a.Consultorio).Include(a => a.Medico)
                .Where(a => a.Dia == atencionHorario.Dia && a.ConsultorioId == atencionHorario.ConsultorioId &&
                a.MedicoId == atencionHorario.MedicoId &&
                a.HoraInicio.Hour <= atencionHorario.HoraInicio.Hour && a.HoraFin.Hour>atencionHorario.HoraInicio.Hour ||
                a.MedicoId == atencionHorario.MedicoId &&
                a.Dia == atencionHorario.Dia && a.ConsultorioId == atencionHorario.ConsultorioId && 
                a.HoraInicio.Hour <= atencionHorario.HoraFin.Hour && a.HoraFin.Hour>=atencionHorario.HoraFin.Hour);

            List<AtencionHorario> horas = await todosHorarios.ToListAsync();

            if (ModelState.IsValid)
            {
                if(horas.Count == 0)
                {
                    db.AtencionHorarios.Add(atencionHorario);
                    await db.SaveChangesAsync();
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "Registrado agregado a la base de datos.",
                        MessageType = GenericMessages.success
                    };
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "El modelo no es válido",
                        MessageType = GenericMessages.danger
                    };
                    return RedirectToAction("Create");
                }
            }

            ViewBag.ConsultorioId = new SelectList(db.Consultorios, "Id", "Nombre", atencionHorario.ConsultorioId);
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", atencionHorario.MedicoId);
            return View(atencionHorario);
        }

        // GET: AtencionHorarios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AtencionHorario atencionHorario = await db.AtencionHorarios.FindAsync(id);
            if (atencionHorario == null)
            {
                return HttpNotFound();
            }
            ViewBag.ConsultorioId = new SelectList(db.Consultorios, "Id", "Nombre", atencionHorario.ConsultorioId);
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", atencionHorario.MedicoId);
            return View(atencionHorario);
        }

        // POST: AtencionHorarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ConsultorioId,MedicoId,Dia,HoraInicio,HoraFin")] AtencionHorario atencionHorario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(atencionHorario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ConsultorioId = new SelectList(db.Consultorios, "Id", "Nombre", atencionHorario.ConsultorioId);
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", atencionHorario.MedicoId);
            return View(atencionHorario);
        }

        // GET: AtencionHorarios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AtencionHorario atencionHorario = await db.AtencionHorarios.FindAsync(id);
            if (atencionHorario == null)
            {
                return HttpNotFound();
            }
            return View(atencionHorario);
        }

        // POST: AtencionHorarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AtencionHorario atencionHorario = await db.AtencionHorarios.FindAsync(id);
            db.AtencionHorarios.Remove(atencionHorario);
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
