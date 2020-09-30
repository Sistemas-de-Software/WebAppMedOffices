namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HistoriaClinica_Table : DbMigration
    {
        public override void Up()
        {
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
                .ForeignKey("dbo.Pacientes", t => t.PacienteId, cascadeDelete: true)
                .ForeignKey("dbo.Turnos", t => t.TurnoId, cascadeDelete: true)
                .Index(t => t.PacienteId)
                .Index(t => t.TurnoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HistoriaClinicas", "TurnoId", "dbo.Turnos");
            DropForeignKey("dbo.HistoriaClinicas", "PacienteId", "dbo.Pacientes");
            DropIndex("dbo.HistoriaClinicas", new[] { "TurnoId" });
            DropIndex("dbo.HistoriaClinicas", new[] { "PacienteId" });
            DropTable("dbo.HistoriaClinicas");
        }
    }
}
