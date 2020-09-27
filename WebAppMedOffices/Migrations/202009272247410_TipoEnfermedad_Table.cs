namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoEnfermedad_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TipoEnfermedades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TipoEnfermedades");
        }
    }
}
