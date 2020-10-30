namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AtencionHorarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConsultorioId = c.Int(nullable: false),
                        MedicoId = c.Int(nullable: false),
                        Dia = c.Int(nullable: false),
                        HoraInicio = c.DateTime(nullable: false),
                        HoraFin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Consultorios", t => t.ConsultorioId)
                .ForeignKey("dbo.Medicos", t => t.MedicoId)
                .Index(t => t.ConsultorioId)
                .Index(t => t.MedicoId);
            
            CreateTable(
                "dbo.Consultorios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 30),
                        BaseEstado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Nombre, unique: true, name: "Consultorio_Nombre_Index");
            
            CreateTable(
                "dbo.Medicos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 30),
                        Apellido = c.String(nullable: false, maxLength: 30),
                        UserName = c.String(nullable: false, maxLength: 30),
                        Telefono = c.String(nullable: false, maxLength: 30),
                        Celular = c.String(nullable: false, maxLength: 30),
                        Matricula = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "Medico_UserName_Index")
                .Index(t => t.Matricula, unique: true, name: "Medico_Matricula_Index");
            
            CreateTable(
                "dbo.DuracionTurnoEspecialidades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MedicoId = c.Int(nullable: false),
                        EspecialidadId = c.Int(nullable: false),
                        Duracion = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Especialidades", t => t.EspecialidadId)
                .ForeignKey("dbo.Medicos", t => t.MedicoId)
                .Index(t => new { t.MedicoId, t.EspecialidadId }, unique: true, name: "DuracionTurnoEspecialidad_MedicoId_EspecialidadId_Index");
            
            CreateTable(
                "dbo.Especialidades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ObraSocialTarifas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tarifa = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ObraSocialId = c.Int(nullable: false),
                        EspecialidadId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Especialidades", t => t.EspecialidadId)
                .ForeignKey("dbo.ObrasSociales", t => t.ObraSocialId)
                .Index(t => t.ObraSocialId)
                .Index(t => t.EspecialidadId);
            
            CreateTable(
                "dbo.ObrasSociales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Telefono = c.String(nullable: false, maxLength: 30),
                        Email = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Nombre, unique: true, name: "ObraSocial_Nombre_Index");
            
            CreateTable(
                "dbo.Pacientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Apellido = c.String(nullable: false, maxLength: 50),
                        Documento = c.String(nullable: false, maxLength: 20),
                        FechaNacimiento = c.DateTime(nullable: false),
                        Direccion = c.String(nullable: false, maxLength: 120),
                        Telefono = c.String(nullable: false, maxLength: 30),
                        Celular = c.String(nullable: false, maxLength: 30),
                        ObraSocialId = c.Int(),
                        NroAfiliado = c.String(maxLength: 30),
                        NombreContactoEmergencia = c.String(maxLength: 50),
                        TelefonoContactoEmergencia = c.String(maxLength: 30),
                        Mail = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ObrasSociales", t => t.ObraSocialId)
                .Index(t => t.Documento, unique: true, name: "Consultorio_Nombre_Index")
                .Index(t => t.ObraSocialId);
            
            CreateTable(
                "dbo.PacienteEnfermedades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PacienteId = c.Int(nullable: false),
                        EnfermedadId = c.Int(nullable: false),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Enfermedades", t => t.EnfermedadId)
                .ForeignKey("dbo.Pacientes", t => t.PacienteId)
                .Index(t => new { t.PacienteId, t.EnfermedadId }, unique: true, name: "PacienteEnfermedad_PacienteId_EnfermedadId_Index");
            
            CreateTable(
                "dbo.Enfermedades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        TipoEnfermedadId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipoEnfermedades", t => t.TipoEnfermedadId)
                .Index(t => t.Nombre, unique: true, name: "Consultorio_Nombre_Index")
                .Index(t => t.TipoEnfermedadId);
            
            CreateTable(
                "dbo.TipoEnfermedades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Nombre, unique: true, name: "Consultorio_Nombre_Index");
            
            CreateTable(
                "dbo.Turnos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MedicoId = c.Int(nullable: false),
                        EspecialidadId = c.Int(nullable: false),
                        PacienteId = c.Int(),
                        ObraSocialId = c.Int(),
                        Estado = c.Int(nullable: false),
                        FechaHora = c.DateTime(nullable: false),
                        FechaHoraFin = c.DateTime(nullable: false),
                        Costo = c.Decimal(precision: 18, scale: 2),
                        Sobreturno = c.Boolean(),
                        TieneObraSocial = c.Boolean(),
                        Comentario = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Especialidades", t => t.EspecialidadId)
                .ForeignKey("dbo.Medicos", t => t.MedicoId)
                .ForeignKey("dbo.ObrasSociales", t => t.ObraSocialId)
                .ForeignKey("dbo.Pacientes", t => t.PacienteId)
                .Index(t => t.MedicoId)
                .Index(t => t.EspecialidadId)
                .Index(t => t.PacienteId)
                .Index(t => t.ObraSocialId);
            
            CreateTable(
                "dbo.HistoriaClinicas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PacienteId = c.Int(nullable: false),
                        TurnoId = c.Int(nullable: false),
                        Motivo = c.Int(nullable: false),
                        Detalle = c.String(nullable: false, maxLength: 50),
                        Comentario = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pacientes", t => t.PacienteId)
                .ForeignKey("dbo.Turnos", t => t.TurnoId)
                .Index(t => t.PacienteId)
                .Index(t => t.TurnoId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.HistoriaClinicas", "TurnoId", "dbo.Turnos");
            DropForeignKey("dbo.HistoriaClinicas", "PacienteId", "dbo.Pacientes");
            DropForeignKey("dbo.DuracionTurnoEspecialidades", "MedicoId", "dbo.Medicos");
            DropForeignKey("dbo.ObraSocialTarifas", "ObraSocialId", "dbo.ObrasSociales");
            DropForeignKey("dbo.Turnos", "PacienteId", "dbo.Pacientes");
            DropForeignKey("dbo.Turnos", "ObraSocialId", "dbo.ObrasSociales");
            DropForeignKey("dbo.Turnos", "MedicoId", "dbo.Medicos");
            DropForeignKey("dbo.Turnos", "EspecialidadId", "dbo.Especialidades");
            DropForeignKey("dbo.Pacientes", "ObraSocialId", "dbo.ObrasSociales");
            DropForeignKey("dbo.PacienteEnfermedades", "PacienteId", "dbo.Pacientes");
            DropForeignKey("dbo.Enfermedades", "TipoEnfermedadId", "dbo.TipoEnfermedades");
            DropForeignKey("dbo.PacienteEnfermedades", "EnfermedadId", "dbo.Enfermedades");
            DropForeignKey("dbo.ObraSocialTarifas", "EspecialidadId", "dbo.Especialidades");
            DropForeignKey("dbo.DuracionTurnoEspecialidades", "EspecialidadId", "dbo.Especialidades");
            DropForeignKey("dbo.AtencionHorarios", "MedicoId", "dbo.Medicos");
            DropForeignKey("dbo.AtencionHorarios", "ConsultorioId", "dbo.Consultorios");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.HistoriaClinicas", new[] { "TurnoId" });
            DropIndex("dbo.HistoriaClinicas", new[] { "PacienteId" });
            DropIndex("dbo.Turnos", new[] { "ObraSocialId" });
            DropIndex("dbo.Turnos", new[] { "PacienteId" });
            DropIndex("dbo.Turnos", new[] { "EspecialidadId" });
            DropIndex("dbo.Turnos", new[] { "MedicoId" });
            DropIndex("dbo.TipoEnfermedades", "Consultorio_Nombre_Index");
            DropIndex("dbo.Enfermedades", new[] { "TipoEnfermedadId" });
            DropIndex("dbo.Enfermedades", "Consultorio_Nombre_Index");
            DropIndex("dbo.PacienteEnfermedades", "PacienteEnfermedad_PacienteId_EnfermedadId_Index");
            DropIndex("dbo.Pacientes", new[] { "ObraSocialId" });
            DropIndex("dbo.Pacientes", "Consultorio_Nombre_Index");
            DropIndex("dbo.ObrasSociales", "ObraSocial_Nombre_Index");
            DropIndex("dbo.ObraSocialTarifas", new[] { "EspecialidadId" });
            DropIndex("dbo.ObraSocialTarifas", new[] { "ObraSocialId" });
            DropIndex("dbo.DuracionTurnoEspecialidades", "DuracionTurnoEspecialidad_MedicoId_EspecialidadId_Index");
            DropIndex("dbo.Medicos", "Medico_Matricula_Index");
            DropIndex("dbo.Medicos", "Medico_UserName_Index");
            DropIndex("dbo.Consultorios", "Consultorio_Nombre_Index");
            DropIndex("dbo.AtencionHorarios", new[] { "MedicoId" });
            DropIndex("dbo.AtencionHorarios", new[] { "ConsultorioId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.HistoriaClinicas");
            DropTable("dbo.Turnos");
            DropTable("dbo.TipoEnfermedades");
            DropTable("dbo.Enfermedades");
            DropTable("dbo.PacienteEnfermedades");
            DropTable("dbo.Pacientes");
            DropTable("dbo.ObrasSociales");
            DropTable("dbo.ObraSocialTarifas");
            DropTable("dbo.Especialidades");
            DropTable("dbo.DuracionTurnoEspecialidades");
            DropTable("dbo.Medicos");
            DropTable("dbo.Consultorios");
            DropTable("dbo.AtencionHorarios");
        }
    }
}
