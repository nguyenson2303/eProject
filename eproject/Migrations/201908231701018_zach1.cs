namespace eproject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zach1 : DbMigration
    {
        public override void Up()
        {
            //migra file for generate table
            CreateTable(
                "dbo.Contests",
                c => new
                    {
                        id = c.Guid(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 30),
                        createAt = c.DateTime(nullable: false),
                        startDate = c.DateTime(nullable: false),
                        endDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.FeedBacks",
                c => new
                    {
                        id = c.Guid(nullable: false, identity: true),
                        content = c.String(nullable: false, maxLength: 150),
                        createAt = c.DateTime(nullable: false),
                        own = c.Guid(nullable: false),
                        recipe_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Recipes", t => t.recipe_id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.own, cascadeDelete: true)
                .Index(t => t.own)
                .Index(t => t.recipe_id);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        id = c.Guid(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 50),
                        category = c.String(nullable: false),
                        content = c.String(nullable: false),
                        type = c.String(nullable: false),
                        createAt = c.DateTime(nullable: false),
                        enabled = c.Boolean(nullable: false),
                        image = c.String(),
                        manager = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.manager, cascadeDelete: false)
                .Index(t => t.manager);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Guid(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 30),
                        email = c.String(nullable: false, maxLength: 30),
                        gender = c.String(nullable: false),
                        phone = c.String(nullable: false),
                        role = c.String(),
                        expireDate = c.DateTime(nullable: false),
                        enabled = c.Boolean(nullable: false),
                        emailVerify = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FeedBacks", "own", "dbo.Users");
            DropForeignKey("dbo.FeedBacks", "recipe_id", "dbo.Recipes");
            DropForeignKey("dbo.Recipes", "manager", "dbo.Users");
            DropIndex("dbo.Recipes", new[] { "manager" });
            DropIndex("dbo.FeedBacks", new[] { "recipe_id" });
            DropIndex("dbo.FeedBacks", new[] { "own" });
            DropTable("dbo.Users");
            DropTable("dbo.Recipes");
            DropTable("dbo.FeedBacks");
            DropTable("dbo.Contests");
        }
    }
}
