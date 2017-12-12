namespace Lexicon_LMS.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddKeyToEventTwo : DbMigration
    {
        
        public override void Up()
        {
            return;

            RenameColumn(table: "dbo.Events", name: "Course_Id", newName: "CourseId");
            RenameIndex(table: "dbo.Events", name: "IX_Course_Id", newName: "IX_CourseId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Events", name: "IX_CourseId", newName: "IX_Course_Id");
            RenameColumn(table: "dbo.Events", name: "CourseId", newName: "Course_Id");
        }
    }
}
