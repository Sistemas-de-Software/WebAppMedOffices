namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Turno : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Turnos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MedicoId = c.Int(nullable: false),
                        EspecialidadId = c.Int(nullable: false),
                        ObraSocialId = c.Int(),
                        Estado = c.Int(nullable: false),
                        FechaHora = c.DateTime(nullable: false),
                        Costo = c.Decimal(precision: 18, scale: 2),
                        Sobreturno = c.Boolean(),
                        TieneObraSocial = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Especialidades", t => t.EspecialidadId, cascadeDelete: true)
                .ForeignKey("dbo.Medicos", t => t.MedicoId, cascadeDelete: true)
                .ForeignKey("dbo.ObrasSociales", t => t.ObraSocialId)
                .Index(t => t.MedicoId)
                .Index(t => t.EspecialidadId)
                .Index(t => t.ObraSocialId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Turnos", "ObraSocialId", "dbo.ObrasSociales");
            DropForeignKey("dbo.Turnos", "MedicoId", "dbo.Medicos");
            DropForeignKey("dbo.Turnos", "EspecialidadId", "dbo.Especialidades");
            DropIndex("dbo.Turnos", new[] { "ObraSocialId" });
            DropIndex("dbo.Turnos", new[] { "EspecialidadId" });
            DropIndex("dbo.Turnos", new[] { "MedicoId" });
            DropTable("dbo.Turnos");
        }
    }
}
