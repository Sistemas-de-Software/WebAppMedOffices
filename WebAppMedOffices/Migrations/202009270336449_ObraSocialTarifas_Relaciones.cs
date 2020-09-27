namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObraSocialTarifas_Relaciones : DbMigration
    {
        public override void Up()
        {
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
                .ForeignKey("dbo.Especialidades", t => t.EspecialidadId, cascadeDelete: true)
                .ForeignKey("dbo.ObrasSociales", t => t.ObraSocialId, cascadeDelete: true)
                .Index(t => t.ObraSocialId)
                .Index(t => t.EspecialidadId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ObraSocialTarifas", "ObraSocialId", "dbo.ObrasSociales");
            DropForeignKey("dbo.ObraSocialTarifas", "EspecialidadId", "dbo.Especialidades");
            DropIndex("dbo.ObraSocialTarifas", new[] { "EspecialidadId" });
            DropIndex("dbo.ObraSocialTarifas", new[] { "ObraSocialId" });
            DropTable("dbo.ObraSocialTarifas");
        }
    }
}
