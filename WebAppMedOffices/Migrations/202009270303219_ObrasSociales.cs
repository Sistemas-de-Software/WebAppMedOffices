namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObrasSociales : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ObrasSociales", "ObraSocial_Nombre_Index");
            DropTable("dbo.ObrasSociales");
        }
    }
}
