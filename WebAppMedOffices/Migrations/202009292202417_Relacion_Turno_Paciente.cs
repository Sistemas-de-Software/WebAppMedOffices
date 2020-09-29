namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relacion_Turno_Paciente : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PacienteTurnos", "PacienteId", "dbo.Pacientes");
            AddColumn("dbo.Turnos", "PacienteId", c => c.Int());
            CreateIndex("dbo.Turnos", "PacienteId");
            AddForeignKey("dbo.Turnos", "PacienteId", "dbo.Pacientes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Turnos", "PacienteId", "dbo.Pacientes");
            DropIndex("dbo.Turnos", new[] { "PacienteId" });
            DropColumn("dbo.Turnos", "PacienteId");
            AddForeignKey("dbo.PacienteTurnos", "PacienteId", "dbo.Pacientes", "Id", cascadeDelete: true);
        }
    }
}
