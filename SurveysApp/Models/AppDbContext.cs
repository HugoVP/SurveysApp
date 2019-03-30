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
    }
}