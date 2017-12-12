namespace Lexicon_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntitiesDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "start_date", c => c.DateTime());
            AlterColumn("dbo.Events", "end_date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "end_date", c => c.DateTime());
            AlterColumn("dbo.Events", "start_date", c => c.DateTime());
        }
    }
}
