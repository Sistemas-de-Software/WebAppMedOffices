namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtencionHorarios : DbMigration
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
                        TrabajoTurno = c.Int(nullable: false),
                        Dia = c.Int(nullable: false),
                        HoraInicio = c.DateTime(nullable: false),
                        HoraFin = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AtencionHorarios");
        }
    }
}
