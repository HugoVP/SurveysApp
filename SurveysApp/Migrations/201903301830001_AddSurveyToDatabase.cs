namespace SurveysApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSurveyToDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Surveys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DischargeTime = c.DateTime(nullable: false),
                        Status = c.String(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        AdminId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AdminId, cascadeDelete: false)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: false)
                .Index(t => t.CategoryId)
                .Index(t => t.AdminId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Surveys", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Surveys", "AdminId", "dbo.Users");
            DropIndex("dbo.Surveys", new[] { "AdminId" });
            DropIndex("dbo.Surveys", new[] { "CategoryId" });
            DropTable("dbo.Surveys");
        }
    }
}
