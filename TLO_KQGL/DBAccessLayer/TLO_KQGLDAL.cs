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
            new DbMigrationsConfiguration().AutomaticMigrationsEnabled=true;//设置自动迁移属性
        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Employee>()
        //        .HasKey(p => p.ID)
        //        .HasRequired(p => p.Dep)
        //        .WithMany();
        //    modelBuilder.Entity<Employee>().ToTable("Employee");//设置生成对应数据库表的名称

        //    modelBuilder.Entity<Department>()
        //        .HasKey(p => p.ID)
        //        .HasMany(p => p.Emp)
        //        .WithRequired(p => p.Dep);
        //    modelBuilder.Entity<Department>()
        //        .HasMany(p => p.Deptclass)
        //        .WithRequired(p => p.dep);
                
        //    modelBuilder.Entity<Department>().ToTable("Department");//设置生成对应数据库表的名称

        //    base.OnModelCreating(modelBuilder);
        //}
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Department { get; set; }
        public DbSet<ClassType> ClassType { get; set; }
        public DbSet<Dept_Class> Dept_Class { get; set; }
    }
}