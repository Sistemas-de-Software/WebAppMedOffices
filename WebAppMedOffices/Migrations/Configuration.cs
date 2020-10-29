namespace WebAppMedOffices.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
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
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "WebAppMedOffices.Models.ApplicationDbContext";
        }

        protected override void Seed(WebAppMedOffices.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            try
            {
                if (!context.Roles.Any(r => r.Name == "Admin"))
                {
                    var store = new RoleStore<IdentityRole>(context);
                    var manager = new RoleManager<IdentityRole>(store);
                    var role = new IdentityRole { Name = "Admin" };

                    manager.Create(role);
                }

                if (!context.Roles.Any(r => r.Name == "Secretaria"))
                {
                    var store = new RoleStore<IdentityRole>(context);
                    var manager = new RoleManager<IdentityRole>(store);
                    var role = new IdentityRole { Name = "Secretaria" };

                    manager.Create(role);
                }

                if (!context.Roles.Any(r => r.Name == "Medico"))
                {
                    var store = new RoleStore<IdentityRole>(context);
                    var manager = new RoleManager<IdentityRole>(store);
                    var role = new IdentityRole { Name = "Medico" };

                    manager.Create(role);
                }

                if (!context.Users.Any(u => u.UserName == "admin@medoffices.com"))
                {
                    var store = new UserStore<ApplicationUser>(context);
                    var manager = new UserManager<ApplicationUser>(store);
                    var user = new ApplicationUser { UserName = "admin@medoffices.com" };

                    manager.Create(user, "Admin1234@");
                    manager.AddToRole(user.Id, "Admin");
                }

                if (!context.Users.Any(u => u.UserName == "secretaria@medoffices.com"))
                {
                    var store = new UserStore<ApplicationUser>(context);
                    var manager = new UserManager<ApplicationUser>(store);
                    var user = new ApplicationUser { UserName = "secretaria@medoffices.com" };

                    manager.Create(user, "Secretaria1234@");
                    manager.AddToRole(user.Id, "Secretaria");
                }

                if (!context.Users.Any(u => u.UserName == "medico1@medoffices.com"))
                {
                    var store = new UserStore<ApplicationUser>(context);
                    var manager = new UserManager<ApplicationUser>(store);
                    var user = new ApplicationUser { UserName = "medico1@medoffices.com" };

                    manager.Create(user, "Medico11234@");
                    manager.AddToRole(user.Id, "Medico");
                }

                if (!context.Users.Any(u => u.UserName == "medico2@medoffices.com"))
                {
                    var store = new UserStore<ApplicationUser>(context);
                    var manager = new UserManager<ApplicationUser>(store);
                    var user = new ApplicationUser { UserName = "medico2@medoffices.com" };

                    manager.Create(user, "Medico21234@");
                    manager.AddToRole(user.Id, "Medico");
                }

                if (!context.Users.Any(u => u.UserName == "medico3@medoffices.com"))
                {
                    var store = new UserStore<ApplicationUser>(context);
                    var manager = new UserManager<ApplicationUser>(store);
                    var user = new ApplicationUser { UserName = "medico3@medoffices.com" };

                    manager.Create(user, "Medico31234@");
                    manager.AddToRole(user.Id, "Medico");
                }

                context.Consultorios.AddOrUpdate(x => x.Id,
                    new Consultorio() { Id = 1, Nombre = "Sala 1", BaseEstado = Shared.BaseEstado.CREADO },
                    new Consultorio() { Id = 2, Nombre = "Sala 2", BaseEstado = Shared.BaseEstado.CREADO },
                    new Consultorio() { Id = 3, Nombre = "Sala 3", BaseEstado = Shared.BaseEstado.CREADO }
                    );
                context.Medicos.AddOrUpdate(x => x.Id,
                    new Medico() { Id = 1, Nombre = "Roberto", Apellido = "Suarez", Telefono = "443285", Celular = "1160848392", Matricula = "12345", UserName = "medico1@medoffices.com" },
                    new Medico() { Id = 2, Nombre = "Juan", Apellido = "Sanchez", Telefono = "446280", Celular = "1180828592", Matricula = "23456", UserName = "medico2@medoffices.com" },
                    new Medico() { Id = 3, Nombre = "Kevin", Apellido = "Ferreyra", Telefono = "442133", Celular = "1130048955", Matricula = "65321", UserName = "medico3@medoffices.com" }
                    );
                context.AtencionHorarios.AddOrUpdate(x => x.Id,
                    new AtencionHorario() { Id = 1, ConsultorioId = 1, MedicoId = 1, Dia = Shared.Dia.LUN, HoraInicio = new DateTime(2020, 09, 01, 08, 00, 0), HoraFin = new DateTime(2020, 09, 01, 12, 00, 0) },
                    new AtencionHorario() { Id = 2, ConsultorioId = 1, MedicoId = 1, Dia = Shared.Dia.MAR, HoraInicio = new DateTime(2020, 09, 01, 08, 00, 0), HoraFin = new DateTime(2020, 09, 01, 12, 00, 0) },
                    new AtencionHorario() { Id = 3, ConsultorioId = 1, MedicoId = 1, Dia = Shared.Dia.MIE, HoraInicio = new DateTime(2020, 09, 01, 08, 00, 0), HoraFin = new DateTime(2020, 09, 01, 12, 00, 0) },
                    new AtencionHorario() { Id = 4, ConsultorioId = 1, MedicoId = 1, Dia = Shared.Dia.JUE, HoraInicio = new DateTime(2020, 09, 01, 08, 00, 0), HoraFin = new DateTime(2020, 09, 01, 12, 00, 0) },
                    new AtencionHorario() { Id = 5, ConsultorioId = 1, MedicoId = 1, Dia = Shared.Dia.VIE, HoraInicio = new DateTime(2020, 09, 01, 08, 00, 0), HoraFin = new DateTime(2020, 09, 01, 12, 00, 0) },
                    new AtencionHorario() { Id = 6, ConsultorioId = 2, MedicoId = 2, Dia = Shared.Dia.LUN, HoraInicio = new DateTime(2020, 09, 01, 13, 00, 0), HoraFin = new DateTime(2020, 09, 01, 19, 00, 0) },
                    new AtencionHorario() { Id = 7, ConsultorioId = 2, MedicoId = 2, Dia = Shared.Dia.MAR, HoraInicio = new DateTime(2020, 09, 01, 13, 00, 0), HoraFin = new DateTime(2020, 09, 01, 19, 00, 0) },
                    new AtencionHorario() { Id = 8, ConsultorioId = 2, MedicoId = 2, Dia = Shared.Dia.MIE, HoraInicio = new DateTime(2020, 09, 01, 13, 00, 0), HoraFin = new DateTime(2020, 09, 01, 19, 00, 0) },
                    new AtencionHorario() { Id = 9, ConsultorioId = 2, MedicoId = 2, Dia = Shared.Dia.JUE, HoraInicio = new DateTime(2020, 09, 01, 13, 00, 0), HoraFin = new DateTime(2020, 09, 01, 19, 00, 0) },
                    new AtencionHorario() { Id = 10, ConsultorioId = 2, MedicoId = 2, Dia = Shared.Dia.VIE, HoraInicio = new DateTime(2020, 09, 01, 13, 00, 0), HoraFin = new DateTime(2020, 09, 01, 19, 00, 0) },
                    new AtencionHorario() { Id = 11, ConsultorioId = 3, MedicoId = 3, Dia = Shared.Dia.SAB, HoraInicio = new DateTime(2020, 09, 01, 09, 00, 0), HoraFin = new DateTime(2020, 09, 01, 12, 00, 0) },
                    new AtencionHorario() { Id = 12, ConsultorioId = 3, MedicoId = 3, Dia = Shared.Dia.DOM, HoraInicio = new DateTime(2020, 09, 01, 09, 00, 0), HoraFin = new DateTime(2020, 09, 01, 12, 00, 0) }
                    );

                context.Especialidades.AddOrUpdate(x => x.Id,
                    new Especialidad() { Id = 1, Nombre = "Radiologia" },
                    new Especialidad() { Id = 2, Nombre = "Oftalmologia" },
                    new Especialidad() { Id = 3, Nombre = "Traumatologia" }
                    );
                context.DuracionTurnoEspecialidades.AddOrUpdate(x => x.Id,
                    new DuracionTurnoEspecialidad() { Id = 1, MedicoId = 1, EspecialidadId = 1, Duracion = 20 },
                    new DuracionTurnoEspecialidad() { Id = 2, MedicoId = 2, EspecialidadId = 2, Duracion = 20 },
                    new DuracionTurnoEspecialidad() { Id = 3, MedicoId = 3, EspecialidadId = 3, Duracion = 20 }
                    );
                context.ObrasSociales.AddOrUpdate(x => x.Id,
                    new ObraSocial() { Id = 1, Nombre = "Sin Obra Social", Telefono = "0000", Email = "clinica@gmail.com" },
                    new ObraSocial() { Id = 2, Nombre = "OsFatun", Telefono = "447800", Email = "osfatun@gmail.com" },
                    new ObraSocial() { Id = 3, Nombre = "OSDE", Telefono = "424500", Email = "osde@gmail.com" },
                    new ObraSocial() { Id = 4, Nombre = "Swiss Medical", Telefono = "423990", Email = "swiss@gmail.com" }
                    );
                context.ObraSocialTarifas.AddOrUpdate(x => x.Id,
                    new ObraSocialTarifa() { Id = 1, Tarifa = new decimal(4200.00), ObraSocialId = 1, EspecialidadId = 1 },
                    new ObraSocialTarifa() { Id = 2, Tarifa = new decimal(3000.00), ObraSocialId = 1, EspecialidadId = 2 },
                    new ObraSocialTarifa() { Id = 3, Tarifa = new decimal(5500.50), ObraSocialId = 1, EspecialidadId = 3 },
                    new ObraSocialTarifa() { Id = 4, Tarifa = new decimal(500.20), ObraSocialId = 2, EspecialidadId = 1 },
                    new ObraSocialTarifa() { Id = 5, Tarifa = new decimal(500.20), ObraSocialId = 2, EspecialidadId = 2 },
                    new ObraSocialTarifa() { Id = 6, Tarifa = new decimal(500.20), ObraSocialId = 2, EspecialidadId = 3 },
                    new ObraSocialTarifa() { Id = 7, Tarifa = new decimal(700.60), ObraSocialId = 3, EspecialidadId = 1 },
                    new ObraSocialTarifa() { Id = 8, Tarifa = new decimal(700.60), ObraSocialId = 3, EspecialidadId = 2 },
                    new ObraSocialTarifa() { Id = 9, Tarifa = new decimal(700.60), ObraSocialId = 3, EspecialidadId = 3 },
                    new ObraSocialTarifa() { Id = 10, Tarifa = new decimal(700.60), ObraSocialId = 4, EspecialidadId = 1 },
                    new ObraSocialTarifa() { Id = 11, Tarifa = new decimal(700.60), ObraSocialId = 4, EspecialidadId = 2 },
                    new ObraSocialTarifa() { Id = 12, Tarifa = new decimal(700.60), ObraSocialId = 4, EspecialidadId = 3 }
                    );
                context.TipoEnfermedades.AddOrUpdate(x => x.Id,
                new TipoEnfermedad() { Id = 1, Nombre = "Patologías" },
                new TipoEnfermedad() { Id = 2, Nombre = "Alergias" }
                );
                context.Pacientes.AddOrUpdate(x => x.Id,
                new Paciente() { Id = 1, Nombre = "Franco", Apellido = "Baez", Documento = "40345671", FechaNacimiento = new DateTime(1998, 10, 01), Direccion = "Oribe 892", Telefono = "442342", Celular = "1143234267", NombreContactoEmergencia = "Fulanito1", TelefonoContactoEmergencia = "4325803453", ObraSocialId = 1, NroAfiliado = "00000001", Mail = "paciente1@test.com"},
                new Paciente() { Id = 2, Nombre = "Soledad", Apellido = "Ibarra", Documento = "30428916", FechaNacimiento = new DateTime(1999, 05, 20), Direccion = "Alvear 1213", Telefono = "425154", Celular = "1165423452", NombreContactoEmergencia = "Fulanito2", TelefonoContactoEmergencia = "425432", ObraSocialId = 2, NroAfiliado = "00000002", Mail = "paciente2@test.com" },
                new Paciente() { Id = 3, Nombre = "Jesica", Apellido = "Amodil", Documento = "40314424", FechaNacimiento = new DateTime(2000, 01, 01), Direccion = "Atahualpa 703", Telefono = "443285", Celular = "1162808393", NombreContactoEmergencia = "Fulanito3", TelefonoContactoEmergencia = "44124", ObraSocialId = 3, NroAfiliado = "00000003", Mail = "paciente3@test.com" },
                new Paciente() { Id = 4, Nombre = "Juana", Apellido = "Amodil", Documento = "40456790", FechaNacimiento = new DateTime(2001, 01, 01), Direccion = "Lafinur 3403", Telefono = "243622345", Celular = "1162345345", NombreContactoEmergencia = "Fulanito4", TelefonoContactoEmergencia = "4412", ObraSocialId = 1, NroAfiliado = null, Mail = "paciente4@test.com" },
                new Paciente() { Id = 5, Nombre = "Analía", Apellido = "Pereyra", Documento = "41459376", FechaNacimiento = new DateTime(2002, 03, 01), Direccion = "Latorre 3254", Telefono = "3453245", Celular = "1163453544", NombreContactoEmergencia = "Fulanito5", TelefonoContactoEmergencia = "44134", ObraSocialId = 2, NroAfiliado = "00000005", Mail = "paciente5@test.com" },
                new Paciente() { Id = 6, Nombre = "Natalia", Apellido = "Murua", Documento = "43314426", FechaNacimiento = new DateTime(1998, 04, 01), Direccion = "Mareo 7287", Telefono = "3453434534", Celular = "1162567576", NombreContactoEmergencia = "Fulanito6", TelefonoContactoEmergencia = "44124234", ObraSocialId = 3, NroAfiliado = "00000006", Mail = "paciente6@test.com" },
                new Paciente() { Id = 7, Nombre = "Brenda", Apellido = "Solís", Documento = "44957838", FechaNacimiento = new DateTime(2002, 01, 01), Direccion = "Las Torres 74", Telefono = "23423525", Celular = "1162345643", NombreContactoEmergencia = "Fulanito7", TelefonoContactoEmergencia = "44124236", ObraSocialId = 1, NroAfiliado = null, Mail = "paciente7@test.com" },
                new Paciente() { Id = 8, Nombre = "Aracelli", Apellido = "Maidana", Documento = "43098900", FechaNacimiento = new DateTime(1994, 03, 24), Direccion = "Jilgero 2332", Telefono = "443281", Celular = "116123455", NombreContactoEmergencia = "Fulanito8", TelefonoContactoEmergencia = "44124267", ObraSocialId = 1, NroAfiliado = null, Mail = "paciente8@test.com" }
                );
                context.Enfermedades.AddOrUpdate(x => x.Id,
                    new Enfermedad() { Id = 1, Nombre = "ANEMIA", TipoEnfermedadId = 1 },
                    new Enfermedad() { Id = 2, Nombre = "ARTROSIS", TipoEnfermedadId = 1 },
                    new Enfermedad() { Id = 3, Nombre = "AUTISMO", TipoEnfermedadId = 1 },
                    new Enfermedad() { Id = 4, Nombre = "DIABETES TIPO 1", TipoEnfermedadId = 1 },
                    new Enfermedad() { Id = 5, Nombre = "ALERGIA A FÁRMACOS", TipoEnfermedadId = 2 },
                    new Enfermedad() { Id = 6, Nombre = "ALERGIA AL POLEN", TipoEnfermedadId = 2 },
                    new Enfermedad() { Id = 7, Nombre = "ALERGIA A ÁCAROS", TipoEnfermedadId = 2 },
                    new Enfermedad() { Id = 8, Nombre = "PENICILINA", TipoEnfermedadId = 2 },
                    new Enfermedad() { Id = 9, Nombre = "ASPIRINA", TipoEnfermedadId = 2 }
                    );
                context.PacienteEnfermedades.AddOrUpdate(x => x.Id,
                new PacienteEnfermedad() { Id = 1, PacienteId = 1, EnfermedadId = 1, Descripcion = "Sin descripción" },
                new PacienteEnfermedad() { Id = 2, PacienteId = 2, EnfermedadId = 9, Descripcion = "Detectada en el año 2015" },
                new PacienteEnfermedad() { Id = 3, PacienteId = 2, EnfermedadId = 7, Descripcion = "Sin descripción" },
                new PacienteEnfermedad() { Id = 4, PacienteId = 2, EnfermedadId = 8, Descripcion = "Sin descripción" }
                );
                /*
                context.PacienteTurnos.AddOrUpdate(x => x.Id,
                    new PacienteTurno() { Id = 1, PacienteId = 1, TurnoId = 1})

                context.Turnos.AddOrUpdate(x => x.Id,
                new Models.Turno() { Id = 1, MedicoId = 1, EspecialidadId = 1, ObraSocialId = 1, Estado = Shared.Estado.Disponible, FechaHora = new DateTime(), Costo = new decimal(), Sobreturno = false, TieneObraSocial = true}    
                );*/
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var sb = new System.Text.StringBuilder();
                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new Exception(sb.ToString());
            }
        }
    }
}
