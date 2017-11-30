namespace Lexicon_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_DbSet_CourseModule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ModuleFormViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Course_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .Index(t => t.Course_Id);
            
            AddColumn("dbo.Modules", "ModuleFormViewModel_Id", c => c.Int());
            CreateIndex("dbo.Modules", "ModuleFormViewModel_Id");
            AddForeignKey("dbo.Modules", "ModuleFormViewModel_Id", "dbo.ModuleFormViewModels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Modules", "ModuleFormViewModel_Id", "dbo.ModuleFormViewModels");
            DropForeignKey("dbo.ModuleFormViewModels", "Course_Id", "dbo.Courses");
            DropIndex("dbo.Modules", new[] { "ModuleFormViewModel_Id" });
            DropIndex("dbo.ModuleFormViewModels", new[] { "Course_Id" });
            DropColumn("dbo.Modules", "ModuleFormViewModel_Id");
            DropTable("dbo.ModuleFormViewModels");
        }
    }
}
