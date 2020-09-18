namespace eproject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zach3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "expireDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "expireDate", c => c.DateTime(nullable: false));
        }
    }
}
