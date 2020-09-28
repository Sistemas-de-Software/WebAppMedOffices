namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebAppMedOffices.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebAppMedOffices.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "WebAppMedOffices.Models.ApplicationDbContext";
        }

        protected override void Seed(WebAppMedOffices.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Consultorios.AddOrUpdate(x => x.Id,
                new Consultorio() { Id = 1, Nombre = "Sala 1"},
                new Consultorio() { Id = 2, Nombre = "Sala 2"},
                new Consultorio() { Id = 3, Nombre = "Sala 3"}
                );
            context.Medicos.AddOrUpdate(x => x.Id,
                new Medico() { Id = 1, Nombre = "Roberto", Apellido = "Suarez", Telefono = "443285", Celular = "1160848392", Matricula = "12345" },
                new Medico() { Id = 2, Nombre = "Juan", Apellido = "Sanchez", Telefono = "446280", Celular = "1180828592", Matricula = "23456" },
                new Medico() { Id = 3, Nombre = "Kevin", Apellido = "Ferreyra", Telefono = "442133", Celular = "1130048955", Matricula = "65321" }
                );
            context.AtencionHorarios.AddOrUpdate(x => x.Id,
                new AtencionHorario() { Id = 1, ConsultorioId = 1, MedicoId = 1, TrabajoTurno = Shared.TrabajoTurno.M, Dia = Shared.Dia.LUN, HoraInicio = new DateTime(2020, 09, 01, 10, 00, 0), HoraFin = new DateTime(2020,09,01,10,20,0)},
                new AtencionHorario() { Id = 2, ConsultorioId = 1, MedicoId = 1, TrabajoTurno = Shared.TrabajoTurno.M, Dia = Shared.Dia.LUN, HoraInicio = new DateTime(2020, 09, 01, 10, 20, 0), HoraFin = new DateTime(2020, 09, 01, 10, 40, 0) },
                new AtencionHorario() { Id = 3, ConsultorioId = 1, MedicoId = 1, TrabajoTurno = Shared.TrabajoTurno.M, Dia = Shared.Dia.LUN, HoraInicio = new DateTime(2020, 09, 01, 10, 40, 0), HoraFin = new DateTime(2020, 09, 01, 11, 00, 0) }
                );

            context.Especialidades.AddOrUpdate(x => x.Id,
                new Especialidad() { Id = 1, Nombre = "Radiologia"},
                new Especialidad() { Id = 2, Nombre = "Oftalmologia" },
                new Especialidad() { Id = 3, Nombre = "Traumatologia" }
                );
            context.DuracionTurnoEspecialidades.AddOrUpdate(x => x.Id,
                new DuracionTurnoEspecialidad() { Id = 1, MedicoId = 1, EspecialidadId = 1, Duracion = 20 },
                new DuracionTurnoEspecialidad() { Id = 2, MedicoId = 2, EspecialidadId = 2, Duracion = 20 },
                new DuracionTurnoEspecialidad() { Id = 3, MedicoId = 3, EspecialidadId = 3, Duracion = 20 }
                );
            context.ObrasSociales.AddOrUpdate(x => x.Id,
                new ObraSocial() { Id = 1, Nombre = "OsFatun", Telefono = "447800", Email = "osfatun@gmail.com"},
                new ObraSocial() { Id = 2, Nombre = "OSDE", Telefono = "424500", Email = "osde@gmail.com" },
                new ObraSocial() { Id = 3, Nombre = "Swiss Medical", Telefono = "423990", Email = "swiss@gmail.com" }
                );
            context.ObraSocialTarifas.AddOrUpdate(x => x.Id,
                new ObraSocialTarifa() { Id = 1, Tarifa = new decimal(400.50), ObraSocialId = 1, EspecialidadId = 1},
                new ObraSocialTarifa() { Id = 2, Tarifa = new decimal(500.20), ObraSocialId = 1, EspecialidadId = 2 },
                new ObraSocialTarifa() { Id = 3, Tarifa = new decimal(700.60), ObraSocialId = 1, EspecialidadId = 3 }
                );
        }
    }
}
