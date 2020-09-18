namespace eproject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zach6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contests", "content", c => c.String(nullable: false, maxLength: 3000));
            AlterColumn("dbo.Users", "expireDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "expireDate", c => c.DateTime());
            DropColumn("dbo.Contests", "content");
        }
    }
}
