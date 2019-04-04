using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace SurveysApp.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(): base("name=AppDbContext")
        {

        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Survey> Survey { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<Answer> Answer { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Question>()
            //    .HasRequired<Survey>(question => question.Survey)
            //    .WithMany(survey => survey.Questions)
            //    .HasForeignKey<int>(question => question.SurveyId);

            //modelBuilder.Entity<Survey>()
            //    .HasMany<Question>(s => s.Questions)
            //    .WithRequired(q => q.Survey)
            //    .HasForeignKey<int>(q => q.SurveyId);

            //modelBuilder.Entity<Category>().HasIndex(m => m.Name).IsUnique();

            //modelBuilder.Entity<User>().HasIndex(m => m.Email).IsUnique();
        }
    }
}