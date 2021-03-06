namespace SurveysApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SurveysApp.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SurveysApp.Models.AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Category.AddOrUpdate(c => c.Id,
                new Models.Category() { Id = 1,  Name = "Education" },
                new Models.Category() { Id = 2, Name = "Deportes" },
                new Models.Category() { Id = 3, Name = "Politica" }
            );
        }
    }
}
