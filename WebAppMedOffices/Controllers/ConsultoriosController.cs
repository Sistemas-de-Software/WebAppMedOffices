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
    public class ConsultoriosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var consultorios = await db.Consultorios.Where(t => t.BaseEstado == Shared.BaseEstado.CREADO).ToListAsync();
            return View(consultorios);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            Consultorio consultorio = await db.Consultorios.FindAsync(id);

            if (consultorio == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            return View(consultorio);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nombre")] Consultorio consultorio)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    consultorio.BaseEstado = Shared.BaseEstado.CREADO;
                    db.Consultorios.Add(consultorio);
                    await db.SaveChangesAsync();
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "Consultorio creado exitosamente.",
                        MessageType = GenericMessages.success
                    };
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "Faltan campos de Consultorio.",
                        MessageType = GenericMessages.warning
                    };
                    return View(consultorio);
                }
            }
            catch (Exception ex)
            {

                var err = $"Error al crear consultorio: {ex.Message}";
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = err,
                    MessageType = GenericMessages.danger
                };
                return View(consultorio);
            }
        }

        // GET: Consultorios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            Consultorio consultorio = await db.Consultorios.FindAsync(id);
            
            if (consultorio == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            return View(consultorio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nombre")] Consultorio consultorio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consultorio).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "Consultorio editado exitosamente.",
                    MessageType = GenericMessages.success
                };
                return RedirectToAction("Index");
            }
            return View(consultorio);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            Consultorio consultorio = await db.Consultorios.FindAsync(id);
            
            if (consultorio == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }
            
            return View(consultorio);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                AtencionHorario consultorioAsociado = await db.AtencionHorarios.FirstOrDefaultAsync(t => t.ConsultorioId == id);

                if (consultorioAsociado != null)
                {
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "No se puede eliminar consultorio, está asignado a un Médico.",
                        MessageType = GenericMessages.warning
                    };
                    return RedirectToAction("Index");
                }

                Consultorio consultorio = await db.Consultorios.FindAsync(id);
                //db.Consultorios.Remove(consultorio);
                consultorio.BaseEstado = Shared.BaseEstado.ELIMINADO;
                await db.SaveChangesAsync();
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "Consultorio eliminado exitosamente.",
                    MessageType = GenericMessages.success
                };
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                var err = $"Error al eliminar consultorio: {ex.Message}";
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = err,
                    MessageType = GenericMessages.danger
                };
                return RedirectToAction("Index");
            }
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
