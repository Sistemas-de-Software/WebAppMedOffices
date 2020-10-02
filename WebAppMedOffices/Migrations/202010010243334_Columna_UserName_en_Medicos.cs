namespace WebAppMedOffices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Columna_UserName_en_Medicos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Medicos", "UserName", c => c.String(nullable: false, maxLength: 30));
            CreateIndex("dbo.Medicos", "UserName", unique: true, name: "Medico_UserName_Index");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Medicos", "Medico_UserName_Index");
            DropColumn("dbo.Medicos", "UserName");
        }
    }
}
