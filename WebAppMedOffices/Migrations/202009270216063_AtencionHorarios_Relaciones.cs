namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtencionHorarios_Relaciones : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.AtencionHorarios", "ConsultorioId");
            CreateIndex("dbo.AtencionHorarios", "MedicoId");
            AddForeignKey("dbo.AtencionHorarios", "ConsultorioId", "dbo.Consultorios", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AtencionHorarios", "MedicoId", "dbo.Medicos", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AtencionHorarios", "MedicoId", "dbo.Medicos");
            DropForeignKey("dbo.AtencionHorarios", "ConsultorioId", "dbo.Consultorios");
            DropIndex("dbo.AtencionHorarios", new[] { "MedicoId" });
            DropIndex("dbo.AtencionHorarios", new[] { "ConsultorioId" });
        }
    }
}
