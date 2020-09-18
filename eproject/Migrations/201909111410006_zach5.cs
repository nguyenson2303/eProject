namespace eproject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zach5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rattings",
                c => new
                    {
                        id = c.Guid(nullable: false, identity: true),
                        rate = c.Int(nullable: false),
                        own = c.Guid(nullable: false),
                        recipe_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Recipes", t => t.recipe_id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.own, cascadeDelete: true)
                .Index(t => t.own)
                .Index(t => t.recipe_id);
            
           
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rattings", "own", "dbo.Users");
            DropForeignKey("dbo.Rattings", "recipe_id", "dbo.Recipes");
            DropIndex("dbo.Rattings", new[] { "recipe_id" });
            DropIndex("dbo.Rattings", new[] { "own" });
            AlterColumn("dbo.Users", "pass", c => c.String(nullable: false, maxLength: 10));
            DropTable("dbo.Rattings");
        }
    }
}
