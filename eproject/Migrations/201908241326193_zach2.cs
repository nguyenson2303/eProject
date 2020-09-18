namespace eproject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zach2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Users", "email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "email" });
        }
    }
}
