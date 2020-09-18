namespace eproject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zach4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "pass", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "pass");
        }
    }
}
