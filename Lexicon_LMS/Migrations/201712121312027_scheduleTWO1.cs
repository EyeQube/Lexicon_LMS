namespace Lexicon_LMS.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class scheduleTWO1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "start_date", c => c.DateTime());
            AlterColumn("dbo.Events", "end_date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "end_date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Events", "start_date", c => c.DateTime(nullable: false));
        }
    }
}
