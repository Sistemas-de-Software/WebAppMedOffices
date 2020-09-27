namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pacientes_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pacientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Apellido = c.String(nullable: false, maxLength: 50),
                        Documento = c.String(nullable: false, maxLength: 20),
                        FechaNacimiento = c.DateTime(nullable: false),
                        Direccion = c.String(nullable: false, maxLength: 120),
                        Telefono = c.String(nullable: false, maxLength: 30),
                        Celular = c.String(nullable: false, maxLength: 30),
                        NombreContactoEmergencia = c.String(nullable: false, maxLength: 50),
                        TelefonoContactoEmergencia = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Documento, unique: true, name: "Consultorio_Nombre_Index");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Pacientes", "Consultorio_Nombre_Index");
            DropTable("dbo.Pacientes");
        }
    }
}
