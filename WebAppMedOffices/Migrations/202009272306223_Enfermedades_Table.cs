namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Enfermedades_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Enfermedades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        TipoEnfermedadId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipoEnfermedades", t => t.TipoEnfermedadId, cascadeDelete: true)
                .Index(t => t.TipoEnfermedadId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Enfermedades", "TipoEnfermedadId", "dbo.TipoEnfermedades");
            DropIndex("dbo.Enfermedades", new[] { "TipoEnfermedadId" });
            DropTable("dbo.Enfermedades");
        }
    }
}
