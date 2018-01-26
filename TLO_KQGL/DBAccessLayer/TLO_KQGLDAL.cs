using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using TLO_KQGL.Models;

namespace TLO_KQGL.DBAccessLayer
{
    public class TLO_KQGLDAL : DbContext
    {
        public TLO_KQGLDAL()
            : base("KQGLContext")
        {
            this.Configuration.ProxyCreationEnabled = true;
            new DbMigrationsConfiguration().AutomaticMigrationsEnabled = true;//设置自动迁移属性
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().Property(p => p.lontitude).HasPrecision(10, 7);
            modelBuilder.Entity<Department>().Property(p => p.latitude).HasPrecision(10, 7);
            modelBuilder.Entity<Attendance>().Property(p => p.LeaveHours).HasPrecision(4, 2);
            modelBuilder.Entity<Attendance>().Property(p => p.WorkOverTime).HasPrecision(5, 2);

        }
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Department { get; set; }
        public DbSet<ClassType> ClassType { get; set; }
        public DbSet<Dept_Class> Dept_Class { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<SignCheck> SignCheck { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Sys_Application> Sys_Application { get; set; }
        public DbSet<Sys_Button> Sys_Button { get; set; }
        public DbSet<Sys_Menu> Sys_Menu { get; set; }
        public DbSet<Privilege> Privilege { get; set; }

        public DbSet<Leave> leave { get; set; }

        public DbSet<Dictionary> dictionary { get; set; }
        public DbSet<calendar> calendar { get; set; }
        public DbSet<News> news { get; set; }
        public DbSet<Suggest> suggest { get; set; }
    }
}