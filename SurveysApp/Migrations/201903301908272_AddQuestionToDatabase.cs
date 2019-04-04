namespace SurveysApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuestionToDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyId = c.Int(nullable: false),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Surveys", t => t.SurveyId, cascadeDelete: true)
                .Index(t => t.SurveyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "SurveyId", "dbo.Surveys");
            DropIndex("dbo.Questions", new[] { "SurveyId" });
            DropTable("dbo.Questions");
        }
    }
}
