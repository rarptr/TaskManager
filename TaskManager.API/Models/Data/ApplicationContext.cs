using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TaskManager.Common.Models;

namespace TaskManager.API.Models.Data
{
    public class ApplicationContext : DbContext
    {
        // Таблицы
        public DbSet<User> Users { get; set; }
        public DbSet<ProjectAdmin> ProjectAdmins { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Desk> Desks { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();

            // TODO: убрать заглушку
            // Если нет ни одного админа
            if (Users.Any(u => u.Status == UserStatus.Admin) == false)
            {
                var admin = new User("admin", "24", "admin@mail.ru", "12345", UserStatus.Admin);
                Users.Add(admin);
                SaveChanges();
            }
        }
    }
}