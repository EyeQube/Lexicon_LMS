namespace Lexicon_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedModuleFormViewModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Module_Id", "dbo.Modules");
            DropIndex("dbo.AspNetUsers", new[] { "Module_Id" });
            DropColumn("dbo.AspNetUsers", "Module_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Module_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Module_Id");
            AddForeignKey("dbo.AspNetUsers", "Module_Id", "dbo.Modules", "Id");
        }
    }
}
