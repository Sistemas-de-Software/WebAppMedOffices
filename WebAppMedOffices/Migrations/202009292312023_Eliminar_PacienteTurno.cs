namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Eliminar_PacienteTurno : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PacienteTurnos", "PacienteId", "dbo.Pacientes");
            DropForeignKey("dbo.PacienteTurnos", "TurnoId", "dbo.Turnos");
            DropIndex("dbo.PacienteTurnos", new[] { "PacienteId" });
            DropIndex("dbo.PacienteTurnos", "PacienteTurno_TurnoId_Index");
            DropTable("dbo.PacienteTurnos");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PacienteTurnos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PacienteId = c.Int(nullable: false),
                        TurnoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.PacienteTurnos", "TurnoId", unique: true, name: "PacienteTurno_TurnoId_Index");
            CreateIndex("dbo.PacienteTurnos", "PacienteId");
            AddForeignKey("dbo.PacienteTurnos", "TurnoId", "dbo.Turnos", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PacienteTurnos", "PacienteId", "dbo.Pacientes", "Id", cascadeDelete: true);
        }
    }
}
