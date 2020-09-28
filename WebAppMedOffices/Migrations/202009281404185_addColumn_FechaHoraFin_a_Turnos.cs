namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumn_FechaHoraFin_a_Turnos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Turnos", "FechaHoraFin", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Turnos", "FechaHoraFin");
        }
    }
}
