﻿using System;
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
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Metadata.Edm;
using WebAppMedOffices.Constants;

namespace WebAppMedOffices.Controllers
{
    [Authorize(Roles = "Secretaria,Admin")]
    public class GestionTurnosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public JsonResult GetEspecialidades(int medicoId)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
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

        // Turnos de hoy
        public async Task<ActionResult> Index()
        {
            var hoy = DateTime.Now.Date;
            var turnos = db.Turnos.Include(t => t.Especialidad).Include(t => t.Medico).Include(t => t.ObraSocial).Where(t => t.Estado == Estado.Reservado && DbFunctions.TruncateTime(t.FechaHora) == hoy);
            return View(await turnos.ToListAsync());
        }

        public ActionResult TurnosDisponiblesInicio()
        {
            return View();
        }

        public ActionResult TurnosReservadosInicio()
        {
            return View();
        }
        public async Task<ActionResult> TurnosDisponiblesListaDeMedicos()
        {
            return View(await db.Medicos.ToListAsync());
        }

        public async Task<ActionResult> TurnosDisponiblesListaDeEspecialidades()
        {
            return View(await db.Especialidades.ToListAsync());
        }

        public async Task<ActionResult> TurnosDisponiblesPorMedico(int? medicoId)
        {
            if (medicoId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListaDeMedicos");
            }

            var turnos = db.Turnos.Include(t => t.Especialidad).Include(t => t.Medico).Include(t => t.ObraSocial).Where(t => t.Estado == Estado.Disponible && t.MedicoId == medicoId);
            return View(await turnos.ToListAsync());
        }

        public async Task<ActionResult> TurnosDisponiblesPorEspecialidad(int? especialidadId)
        {
            if (especialidadId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListaDeEspecialidades");
            }

            var turnos = db.Turnos.Include(t => t.Especialidad).Include(t => t.Medico).Include(t => t.ObraSocial).Where(t => t.Estado == Estado.Disponible && t.EspecialidadId == especialidadId);
            return View(await turnos.ToListAsync());
        }

        public async Task<ActionResult> ListaDeMedicos()
        {
            return View(await db.Medicos.ToListAsync());
        }

        public async Task<ActionResult> ListaDeEspecialidades()
        {
            return View(await db.Especialidades.ToListAsync());
        }

        public async Task<ActionResult> ListaDePacientes()
        {
            return View(await db.Pacientes.ToListAsync());
        }

        public async Task<ActionResult> TurnosReservadosPorMedico(int? medicoId)
        {
            if (medicoId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListaDeMedicos");
            }

            ViewBag.medicoId = medicoId;
            var turnos = db.Turnos.Include(t => t.Especialidad).Include(t => t.Medico).Include(t => t.ObraSocial).Where(t => t.Estado == Estado.Reservado && t.MedicoId == medicoId);
            return View(await turnos.ToListAsync());
        }

        public async Task<ActionResult> TurnosReservadosPorEspecialidad(int? especialidadId)
        {
            if (especialidadId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListaDeEspecialidades");
            }

            var turnos = db.Turnos.Include(t => t.Especialidad).Include(t => t.Medico).Include(t => t.ObraSocial).Where(t => t.Estado == Estado.Reservado && t.EspecialidadId == especialidadId);
            return View(await turnos.ToListAsync());
        }

        public async Task<ActionResult> TurnosReservadosPorPaciente(int? pacienteId)
        {
            if (pacienteId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("ListaDePacientes");
            }

            var turnos = db.Turnos.Include(t => t.Especialidad).Include(t => t.Medico).Include(t => t.ObraSocial).Where(t => t.Estado == Estado.Reservado && t.PacienteId == pacienteId);
            return View(await turnos.ToListAsync());
        }

        public async Task<ActionResult> Comprobante(int? pacienteId, int? turnoId)
        {
            if (pacienteId == null || turnoId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("TurnosReservadosInicio");
            }


            Turno turno = await db.Turnos.FirstOrDefaultAsync(t => t.Id == turnoId && t.PacienteId == pacienteId);

            if (turno == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("TurnosReservadosInicio");
            }

            return View(turno);
        }

        public async Task<ActionResult> ListaCambiarTurno(int? turnoId)
        {
            if (turnoId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            var turnos = db.Turnos.Include(t => t.Especialidad).Include(t => t.Medico).Include(t => t.ObraSocial).Where(t => t.Estado == Estado.Disponible);
            Turno turnoAntes = await db.Turnos.FindAsync(turnoId);

            if (turnos == null || turnoAntes == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            ViewBag.PacienteId = turnoAntes.PacienteId;
            ViewBag.TurnoAntesId = turnoAntes.Id;

            return View(await turnos.ToListAsync());
        }

        public async Task<ActionResult> CambiarTurno(int? turnoAntesId, int? turnoDespuesId, int? pacienteId)
        {
            if (turnoAntesId == null || turnoDespuesId == null || pacienteId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            Turno turnoAntes = await db.Turnos.FindAsync(turnoAntesId);
            Turno turnoDespues = await db.Turnos.FindAsync(turnoDespuesId);
            Paciente paciente = await db.Pacientes.FindAsync(pacienteId);

            if (turnoAntes == null || turnoDespues == null || paciente == null )
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }


            TurnoCambiarView turnoAntesYDespues = new TurnoCambiarView();
            turnoAntesYDespues.TurnoAntes = turnoAntes;
            turnoAntesYDespues.TurnoDespues = turnoDespues;

            ViewBag.TieneOS = turnoAntes.TieneOS;
            
            return View(turnoAntesYDespues);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CambiarTurno(TurnoCambiarView turno)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Turno turnoAntes = await db.Turnos.FindAsync(turno.TurnoAntes.Id);
                    Turno turnoDespues = await db.Turnos.FindAsync(turno.TurnoDespues.Id);

                    if (turnoAntes == null || turnoDespues == null)
                    {
                        TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                        {
                            Message = "No se encontró turno.",
                            MessageType = GenericMessages.danger
                        };
                        return RedirectToAction("Index");
                    }


                    turnoDespues.ObraSocialId = turnoAntes.Paciente.ObraSocial.Id; // lo saco del turno anterior
                    turnoDespues.PacienteId = turnoAntes.PacienteId; // lo saco del turno anterior
                    turnoDespues.Estado = Estado.Reservado;
                    turnoDespues.Costo = turnoAntes.Paciente.ObraSocial.Tarifas.Where(t => t.EspecialidadId == turnoDespues.EspecialidadId).FirstOrDefault().Tarifa;


                    db.Entry(turnoDespues).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    turnoAntes.ObraSocialId = null;
                    turnoAntes.PacienteId = null;
                    turnoAntes.Estado = Estado.Disponible;
                    turnoAntes.Costo = null;
                    turnoAntes.Sobreturno = null;
                    turnoAntes.TieneObraSocial = null;



                    db.Entry(turnoAntes).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "Turno cambiado exitosamante.",
                        MessageType = GenericMessages.success
                    };
                    
                    return RedirectToAction("TurnosReservadosInicio");
                }
                else
                {
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "Error al validar los campos.",
                        MessageType = GenericMessages.danger
                    };
                    return View(turno);
                }
            }
            catch (Exception ex)
            {
                var err = $"Error al cambiar turno: {ex.Message}";
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = err,
                    MessageType = GenericMessages.danger
                };
                return View(turno);
            }
        }


        public async Task<ActionResult> BuscarTurnos(int? id)
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

            var turnos = db.Turnos.Include(t => t.Especialidad).Include(t => t.Medico).Include(t => t.ObraSocial).Where(t => t.Estado == Estado.Disponible);
            
            if (turnos == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            ViewBag.PacienteId = id;
            
            return View(await turnos.ToListAsync());
        }

        public async Task<ActionResult> BuscarSobreturnos(int? id)
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

            var turnos = db.Turnos.Include(t => t.Especialidad).Include(t => t.Medico).Include(t => t.ObraSocial).Where(t => t.Estado == Estado.Disponible || t.Estado == Estado.Reservado);

            if (turnos == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            ViewBag.PacienteId = id;

            return View(await turnos.ToListAsync());
        }

        public async Task<ActionResult> AsignarTurno(int? id, int? pacienteId)
        {
            if (id == null || pacienteId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            Turno turno = await db.Turnos.FindAsync(id);
            Paciente paciente = await db.Pacientes.FindAsync(pacienteId);
            ObraSocial obraSocial = new ObraSocial();
            if(paciente.ObraSocial == null)
            {
                obraSocial = await db.ObrasSociales.FindAsync(1);
                paciente.ObraSocial = obraSocial;
            }
            
            if (turno == null || paciente == null)
            {
                return HttpNotFound();
            }

            Turno nuevoTurno = new Turno();
            nuevoTurno.Id = turno.Id;
            nuevoTurno.MedicoId = turno.MedicoId;
            nuevoTurno.EspecialidadId = turno.EspecialidadId;
            nuevoTurno.ObraSocialId = paciente.ObraSocial.Id;
            nuevoTurno.PacienteId = paciente.Id;
            nuevoTurno.Estado = Estado.Reservado;
            nuevoTurno.FechaHora = turno.FechaHora;
            nuevoTurno.FechaHoraFin = turno.FechaHoraFin;
            nuevoTurno.Costo = paciente.ObraSocial.Tarifas.Where(t => t.EspecialidadId == turno.EspecialidadId).FirstOrDefault().Tarifa;
            nuevoTurno.Sobreturno = false;
            nuevoTurno.TieneObraSocial = false;
            nuevoTurno.Medico = turno.Medico;
            nuevoTurno.Especialidad = turno.Especialidad;
            nuevoTurno.Paciente = paciente;
            nuevoTurno.ObraSocial = paciente.ObraSocial;

            return View(nuevoTurno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AsignarTurno([Bind(Include = "Id,MedicoId,EspecialidadId,ObraSocialId,PacienteId,Estado,FechaHora,FechaHoraFin,Costo,Sobreturno,TieneObraSocial")] Turno turno)
        {
            if (ModelState.IsValid)
            {
                db.Entry(turno).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "Turno reservado exitosamante.",
                    MessageType = GenericMessages.success
                };
                return RedirectToAction("TurnosReservadosInicio");
            }

            return View(turno);
        }

        public async Task<ActionResult> AsignarSobreturno(int? id, int? pacienteId)
        {
            if (id == null || pacienteId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            Turno turno = await db.Turnos.FindAsync(id);
            Paciente paciente = await db.Pacientes.FindAsync(pacienteId);

            if (turno == null || paciente == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("Index");
            }

            Turno nuevoTurno = new Turno();
            nuevoTurno.MedicoId = turno.MedicoId;
            nuevoTurno.EspecialidadId = turno.EspecialidadId;
            nuevoTurno.ObraSocialId = paciente.ObraSocial.Id;
            nuevoTurno.PacienteId = paciente.Id;
            nuevoTurno.Estado = Estado.Reservado;
            nuevoTurno.FechaHora = turno.FechaHora;
            nuevoTurno.FechaHoraFin = turno.FechaHoraFin;
            nuevoTurno.Costo = paciente.ObraSocial.Tarifas.Where(t => t.EspecialidadId == turno.EspecialidadId).FirstOrDefault().Tarifa;
            nuevoTurno.Sobreturno = true;
            nuevoTurno.TieneObraSocial = false;
            nuevoTurno.Medico = turno.Medico;
            nuevoTurno.Especialidad = turno.Especialidad;
            nuevoTurno.Paciente = paciente;
            nuevoTurno.ObraSocial = paciente.ObraSocial;

            return View(nuevoTurno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AsignarSobreturno([Bind(Include = "MedicoId,EspecialidadId,ObraSocialId,PacienteId,Estado,FechaHora,FechaHoraFin,Costo,Sobreturno,TieneObraSocial")] Turno turno)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Turnos.Add(turno);
                    await db.SaveChangesAsync();
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "Sobreturno reservado exitosamante.",
                        MessageType = GenericMessages.success
                    };
                    return RedirectToAction("TurnosReservadosInicio");
                }
            }
            catch (Exception ex)
            {
                var err = $"Error al asignar el sobreturno: {ex.Message}";
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = err,
                    MessageType = GenericMessages.danger
                };
            }

            return View(turno);
        }

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
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "La fecha Desde no puede ser mayor que la fecha Hasta.",
                        MessageType = GenericMessages.warning
                    };
                    ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", turnoView.EspecialidadId);
                    ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", turnoView.MedicoId);
                    return View(turnoView);
                }

                var hoy = DateTime.Now.Date;
                if (turnoView.FechaDesde.Date < hoy)
                {
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "La fecha Desde no puede ser menor que la fecha de Hoy.",
                        MessageType = GenericMessages.warning
                    };
                    ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", turnoView.EspecialidadId);
                    ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", turnoView.MedicoId);
                    return View(turnoView);
                }

                var duracion = db.DuracionTurnoEspecialidades.Where(t => t.MedicoId == turnoView.MedicoId && t.EspecialidadId == turnoView.EspecialidadId).FirstOrDefault();
                if (duracion == null)
                {
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "No existe la especialidad del Médico.",
                        MessageType = GenericMessages.warning
                    };
                    ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", turnoView.EspecialidadId);
                    ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", turnoView.MedicoId);
                    return View(turnoView);
                }

                var diasHorarios = db.AtencionHorarios.Where(t => t.MedicoId == turnoView.MedicoId).ToList();
                if (diasHorarios == null)
                {
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "El Médico no tiene horario de atención cargados.",
                        MessageType = GenericMessages.warning
                    };
                    ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", turnoView.EspecialidadId);
                    ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", turnoView.MedicoId);
                    return View(turnoView);
                }

                Turno hayTurnoCreado = await db.Turnos.Where(t => DbFunctions.TruncateTime(t.FechaHora) >= turnoView.FechaDesde && DbFunctions.TruncateTime(t.FechaHora) <= turnoView.FechaHasta && t.MedicoId == turnoView.MedicoId && t.EspecialidadId == turnoView.EspecialidadId).FirstOrDefaultAsync();
                if (hayTurnoCreado != null)
                {
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "Ya hay turnos cargados dentro del rango de fechas elegido.",
                        MessageType = GenericMessages.warning
                    };
                    ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", turnoView.EspecialidadId);
                    ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", turnoView.MedicoId);
                    return View(turnoView);
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

                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "Turno creados exitosamante.",
                    MessageType = GenericMessages.success
                };

                return RedirectToAction("TurnosDisponiblesInicio");
            }

            ViewBag.EspecialidadId = new SelectList(db.Especialidades, "Id", "Nombre", turnoView.EspecialidadId);
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "Nombre", turnoView.MedicoId);
            return View(turnoView);
        }

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Turno turno = await db.Turnos.FindAsync(id);
            db.Turnos.Remove(turno);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ListaPacientes()
        {
            return View(await db.Pacientes.ToListAsync());
        }

        public async Task<ActionResult> DetailsPaciente(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Paciente paciente = await db.Pacientes.FindAsync(id);

            if (paciente == null)
            {
                return HttpNotFound();
            }

            return View(paciente);
        }

        public ActionResult CancelarTurno(int? medicoId)
        {
            if (medicoId == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("TurnosReservadosInicio");
            }

            DateTime hoy = DateTime.Now.Date;
            var turnos = db.Turnos.Include(t => t.Especialidad).Include(t => t.Medico).Include(t => t.ObraSocial).Where(t => t.Estado == Estado.Reservado && t.MedicoId == medicoId && DbFunctions.TruncateTime(t.FechaHora) == hoy);
            List<Turno> nuevoTurno = new List<Turno>();
            if (turnos == null)
            {
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "No existe la ruta.",
                    MessageType = GenericMessages.warning
                };
                return RedirectToAction("TurnosReservadosInicio");
            }
            foreach (var item in turnos)
            {
                Turno t = new Turno();
                t.Id = item.Id;
                t.MedicoId = item.MedicoId;
                t.EspecialidadId = item.EspecialidadId;
                    //t.ObraSocialId = item.Paciente.ObraSocialId;
                    //t.PacienteId = item.PacienteId;
                t.Estado = Estado.CANCELADOXMEDICO;
                t.FechaHora = item.FechaHora;
                t.FechaHoraFin = item.FechaHoraFin;
                    //t.Costo = item.Paciente.ObraSocial.Tarifas.Where(a => a.EspecialidadId == item.EspecialidadId).FirstOrDefault().Tarifa;
                t.Sobreturno = false;
                t.TieneObraSocial = false;
                t.Medico = item.Medico;
                t.Especialidad = item.Especialidad;
                    //t.Paciente = item.Paciente;
                    //t.ObraSocial = item.Paciente.ObraSocial;
                nuevoTurno.Add(t);
            }
           
            ViewBag.MedicoId = medicoId;
            return View(nuevoTurno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelarTurno(List<Turno> turno)
        {
            if (ModelState.IsValid)
            {                
                for(int t=0; t < turno.Count; t++)
                {
                    db.Entry(turno[t]).State = EntityState.Modified;                  
                }
                
                await db.SaveChangesAsync();
                TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                {
                    Message = "Turno cancelado exitosamante.",
                    MessageType = GenericMessages.success
                };                 
            }            
                 
            return View(turno);
        }
        public ActionResult CreatePaciente()
        {
            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePaciente(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                var hoy = DateTime.Now.Date;
                if (paciente.FechaNacimiento.Date > hoy)
                {
                    TempData[Application.MessageViewBagName] = new GenericMessageViewModel
                    {
                        Message = "La fecha de nacimiento no puede ser menor que la fecha de Hoy.",
                        MessageType = GenericMessages.warning
                    };
                    return RedirectToAction("CreatePaciente");
                }
                db.Pacientes.Add(paciente);
                await db.SaveChangesAsync();
                return RedirectToAction("ListaPacientes");
            }

            return View(paciente);
        }

        public async Task<ActionResult> EditPaciente(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Paciente paciente = await db.Pacientes.FindAsync(id);

            if (paciente == null)
            {
                return HttpNotFound();
            }

            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre", paciente.ObraSocialId);
            return View(paciente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPaciente(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paciente).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("ListaPacientes");
            }

            ViewBag.ObraSocialId = new SelectList(db.ObrasSociales, "Id", "Nombre", paciente.ObraSocialId);
            return View(paciente);
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
