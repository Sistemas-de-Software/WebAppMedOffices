namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Agregar_Comentario_en_Turno : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Turnos", "Comentario", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Turnos", "Comentario");
        }
    }
}
