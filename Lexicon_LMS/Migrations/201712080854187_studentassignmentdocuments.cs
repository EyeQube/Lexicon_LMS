namespace Lexicon_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class studentassignmentdocuments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudentDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Graded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ActivityTypes", "AllowStudentDocuments", c => c.Boolean(nullable: false));
            AddColumn("dbo.Documents", "StudentDocumentId", c => c.Int());
            CreateIndex("dbo.Documents", "StudentDocumentId");
            AddForeignKey("dbo.Documents", "StudentDocumentId", "dbo.StudentDocuments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "StudentDocumentId", "dbo.StudentDocuments");
            DropIndex("dbo.Documents", new[] { "StudentDocumentId" });
            DropColumn("dbo.Documents", "StudentDocumentId");
            DropColumn("dbo.ActivityTypes", "AllowStudentDocuments");
            DropTable("dbo.StudentDocuments");
        }
    }
}
