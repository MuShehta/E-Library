using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Library.Model
{
    public class appContext : DbContext
    {
        //public appContext(DbContextOptions<appContext> options)
        //: base(options)
        //{
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder option)
        {
            option.UseSqlServer("Data Source=DESKTOP-ELJAV78\\SQLEXPRESS;Initial Catalog=E_Library;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.user_name).IsUnique();


            modelBuilder.Entity<Buy>().HasOne("user").WithMany("buy");
            modelBuilder.Entity<Buy>().HasOne("book").WithMany("buy");
        }

        public DbSet<User> users { get; set; }
        public DbSet<Book> books { get; set; }
        public DbSet<Buy> buy { get; set; }
    }
}
