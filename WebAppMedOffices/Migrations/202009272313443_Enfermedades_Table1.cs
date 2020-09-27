namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Enfermedades_Table1 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Enfermedades", "Nombre", unique: true, name: "Consultorio_Nombre_Index");
            CreateIndex("dbo.TipoEnfermedades", "Nombre", unique: true, name: "Consultorio_Nombre_Index");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TipoEnfermedades", "Consultorio_Nombre_Index");
            DropIndex("dbo.Enfermedades", "Consultorio_Nombre_Index");
        }
    }
}
