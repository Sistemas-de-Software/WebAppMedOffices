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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAppMedOffices.Shared;

namespace WebAppMedOffices.Controllers
{
    public class GestionTurnosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public JsonResult GetEspecialidades(int medicoId)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false; // para API
                var especialidades = db.DuracionTurnoEspecialidades.Where(t => t.MedicoId == medicoId).Select(i =>
                    new { i.Id, i.EspecialidadId, i.MedicoId, i.Especialidad.Nombre });
                var json = JsonConvert.SerializeObject(especialidades);
                return Json(especialidades);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // GET: GestionTurnos
        public async Task<ActionResult> Index()
        {
            var turnos = db.Turnos.Include(t => t.Especialidad).Include(t => t.Medico).Include(t => t.ObraSocial);
            return View(await turnos.ToListAsync());
        }

        // GET: GestionTurnos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turno turno = await db.Turnos.FindAsync(id);
            if (turno == null)
            {
                return HttpNotFound();
            }
            return View(turno);
        }

        // GET: GestionTurnos/Create
        public async Task<ActionResult> Create()
        {
            var medicos = db.Medicos.Include(t => t.DuracionTurnoEspecialidades).OrderBy(t => t.Apellido);
            ViewBag.MedicoId = new SelectList(await medicos.ToListAsync(), "Id", "NombreCompleto");
            var duracionTurnoEspecialidades = db.DuracionTurnoEspecialidades.Where(t => t.MedicoId == medicos.FirstOrDefault().Id).Include(t => t.Especialidad).OrderBy(t => t.Especialidad.Nombre);
            ViewBag.EspecialidadId = new SelectList(await duracionTurnoEspecialidades.ToListAsync(), "EspecialidadId", "Especialidad.Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,MedicoId,EspecialidadId,FechaDesde,FechaHasta")] TurnoView turnoView)
        {
            if (ModelState.IsValid)
            {
                if (turnoView.FechaDesde.Date > turnoView.FechaHasta.Date)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var duracion = db.DuracionTurnoEspecialidades.Where(t => t.MedicoId == turnoView.MedicoId && t.EspecialidadId == turnoView.EspecialidadId).FirstOrDefault();
                if (duracion == null)
                {
                    return HttpNotFound();
                }

                var diasHorarios = db.AtencionHorarios.Where(t => t.MedicoId == turnoView.MedicoId).ToList();
                if (diasHorarios == null)
                {
                    return HttpNotFound();
                }

                //creamos el ámbito de la transacción
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        
                        TimeSpan diferencia = turnoView.FechaHasta - turnoView.FechaDesde;
                        for (int i = 0; i < diferencia.TotalDays; i++)
                        {
                            DateTime fechaActual = new DateTime();
                            fechaActual = turnoView.FechaDesde.AddDays(i);

                            foreach (var diaHorario in diasHorarios)
                            {
                                if ((int) diaHorario.Dia == (int) fechaActual.DayOfWeek)
                                {
                                    DateTime horarioActual = diaHorario.HoraInicio;
                                    while (horarioActual + TimeSpan.FromMinutes(duracion.Duracion) < diaHorario.HoraFin)
                                    {

                                        //una consulta
                                        Turno turno = new Turno();
                                        turno.MedicoId = turnoView.MedicoId;
                                        turno.EspecialidadId = turnoView.EspecialidadId;
                                        turno.Estado = Estado.Disponible;
                                        turno.FechaHora = fechaActual.Date.AddHours(horarioActual.Hour).AddMinutes(horarioActual.Minute); // Horario que comienza a atender

                                        horarioActual += TimeSpan.FromMinutes(duracion.Duracion); // Sumamos minutos

                                        turno.FechaHoraFin = fechaActual.Date.AddHours(horarioActual.Hour).AddMinutes(horarioActual.Minute); // OJO: Posible error a futuro...

                                        //agregamos el elemento
                                        db.Turnos.Add(turno);
                                    }
                                }
                            }
                            
                        }

                        //guardamos en la base de datos
                        await db.SaveChangesAsync();

                        ////hacemos algo extra a manipulación de datos
                        ////como enviar un mail, suponiendo que regresa true si es exitoso
                        //if (!EnviaUnMail())
                        //{
                        //    //hacemos rollback si fallo el envio del mail
                        //    dbContextTransaction.Rollback();
                        //}

                        //Hacemos commit de todos los datos
                        dbContextTransaction.Commit();

                    }
                    catch (Exception)
                    {
                        //hacemos rollback si hay excepción
                        dbContextTransaction.Rollback();
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                }

                return RedirectToAction("Index");
            }

            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", turnoView.EspecialidadId);
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", turnoView.MedicoId);
            return View(turnoView);
        }

        // GET: GestionTurnos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turno turno = await db.Turnos.FindAsync(id);
            if (turno == null)
            {
                return HttpNotFound();
            }
            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", turno.EspecialidadId);
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", turno.MedicoId);
            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre", turno.ObraSocialId);
            return View(turno);
        }

        // POST: GestionTurnos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,MedicoId,EspecialidadId,ObraSocialId,Estado,FechaHora,FechaHoraFin,Costo,Sobreturno,TieneObraSocial")] Turno turno)
        {
            if (ModelState.IsValid)
            {
                db.Entry(turno).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", turno.EspecialidadId);
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", turno.MedicoId);
            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre", turno.ObraSocialId);
            return View(turno);
        }

        // GET: GestionTurnos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Turno turno = await db.Turnos.FindAsync(id);
            if (turno == null)
            {
                return HttpNotFound();
            }
            return View(turno);
        }

        // POST: GestionTurnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Turno turno = await db.Turnos.FindAsync(id);
            db.Turnos.Remove(turno);
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
