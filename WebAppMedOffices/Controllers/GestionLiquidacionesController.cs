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
    public class GestionLiquidacionesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult CrearLiquidacionPacientesParticulares()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LiquidacionPacientesParticulares(LiquidacionPacienteViewModel liquidacionPacientes)
        {
            if (ModelState.IsValid != true)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "Los campos no son válidos",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("CrearLiquidacionPacientesParticulares");
            }

            if (liquidacionPacientes.FechaDesde.Date > liquidacionPacientes.FechaHasta.Date)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "La fecha Desde no puede ser mayor que la fecha Hasta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("CrearLiquidacionPacientesParticulares");
            }

            var hoy = DateTime.Now.Date;
            //if (liquidacionPacientes.FechaDesde.Date < hoy)
            //{
            //    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
            //    {
            //        Message = "La fecha Desde no puede ser menor que la fecha de Hoy.",
            //        MessageType = GenericMessages.warning
            //    };
            //    return RedirectToAction("CrearLiquidacionPacientesParticulares");
            //}

            try
            {
                var turnos = db.Turnos.Include(t => t.Paciente)
                                                     .Where(t => t.ObraSocialId == 1 && 
                                                                t.Estado == Shared.Estado.Atendido &&
                                                                DbFunctions.TruncateTime(t.FechaHora) >= liquidacionPacientes.FechaDesde &&
                                                                DbFunctions.TruncateTime(t.FechaHora) <= liquidacionPacientes.FechaHasta)
                                                     .GroupBy(t => t.PacienteId).
                                                     Select(g => new LiquidacionPacienteViewModel
                                                     {
                                                         PacienteId = g.Key.Value,
                                                         Turnos = g.ToList(),
                                                         SubTotal = g.Sum(p => p.Costo.Value),
                                                         Fecha = hoy
                                                     });

                if (turnos.Count() == 0)
                {
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "No hay datos para generar liquidación.",
                        MessageType = GenericMessages.warning
                    };
                    return RedirectToAction("CrearLiquidacionPacientesParticulares");
                }
                
                ViewBag.Total = await turnos.SumAsync(t => t.SubTotal);

                return View(await turnos.ToListAsync());
            }
            catch (Exception ex)
            {
                var err = $"Error al cargar datos: {ex.Message}";
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = err,
                    MessageType = GenericMessages.danger
                };
                return RedirectToAction("CrearLiquidacionPacientesParticulares");
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
