namespace SurveysApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetSurveyStatusTypeAsEStatus : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Surveys", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Surveys", "Status", c => c.String(nullable: false));
        }
    }
}
