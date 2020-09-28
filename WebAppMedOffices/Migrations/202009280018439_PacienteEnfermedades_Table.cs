namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PacienteEnfermedades_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PacienteEnfermedades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PacienteId = c.Int(nullable: false),
                        EnfermedadId = c.Int(nullable: false),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Enfermedades", t => t.EnfermedadId, cascadeDelete: true)
                .ForeignKey("dbo.Pacientes", t => t.PacienteId, cascadeDelete: true)
                .Index(t => new { t.PacienteId, t.EnfermedadId }, unique: true, name: "PacienteEnfermedad_PacienteId_EnfermedadId_Index");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PacienteEnfermedades", "PacienteId", "dbo.Pacientes");
            DropForeignKey("dbo.PacienteEnfermedades", "EnfermedadId", "dbo.Enfermedades");
            DropIndex("dbo.PacienteEnfermedades", "PacienteEnfermedad_PacienteId_EnfermedadId_Index");
            DropTable("dbo.PacienteEnfermedades");
        }
    }
}
