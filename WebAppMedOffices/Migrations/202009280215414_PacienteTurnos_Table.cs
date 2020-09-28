namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PacienteTurnos_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PacienteTurnos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PacienteId = c.Int(nullable: false),
                        TurnoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pacientes", t => t.PacienteId, cascadeDelete: true)
                .ForeignKey("dbo.Turnos", t => t.TurnoId, cascadeDelete: true)
                .Index(t => t.PacienteId)
                .Index(t => t.TurnoId, unique: true, name: "PacienteTurno_TurnoId_Index");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PacienteTurnos", "TurnoId", "dbo.Turnos");
            DropForeignKey("dbo.PacienteTurnos", "PacienteId", "dbo.Pacientes");
            DropIndex("dbo.PacienteTurnos", "PacienteTurno_TurnoId_Index");
            DropIndex("dbo.PacienteTurnos", new[] { "PacienteId" });
            DropTable("dbo.PacienteTurnos");
        }
    }
}
