namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_deleteAt_medico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Medicos", "DeleteAt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Medicos", "DeleteAt");
        }
    }
}
