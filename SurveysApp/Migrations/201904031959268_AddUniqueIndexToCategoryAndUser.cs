namespace SurveysApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueIndexToCategoryAndUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Categories", "Name", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.Users", "Email", unique: true);
            CreateIndex("dbo.Categories", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Categories", new[] { "Name" });
            DropIndex("dbo.Users", new[] { "Email" });
            AlterColumn("dbo.Categories", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false));
        }
    }
}
