using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(a => a.Account)
                .WithOne(b => b.Employee)
                .HasForeignKey<Account>(b => b.NIK);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Profiling)
                .WithOne(b => b.Account)
                .HasForeignKey<Profiling>(b => b.NIK);

            modelBuilder.Entity<Education>()
                .HasMany(a => a.Profilings)
                .WithOne(b => b.Education);

            modelBuilder.Entity<University>()
                .HasMany(a => a.Educations)
                .WithOne(b => b.University);

            modelBuilder.Entity<AccountRole>()
                .HasKey(bc => new { bc.NIK, bc.RoleID });

            modelBuilder.Entity<AccountRole>()
                .HasOne(a => a.Account)
                .WithMany(b => b.AccountRoles)
                .HasForeignKey(a => a.NIK);

            modelBuilder.Entity<AccountRole>()
                .HasOne(a => a.Role)
                .WithMany(b => b.AccountRoles)
                .HasForeignKey(a => a.RoleID);
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<Profiling> Profilings { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<University> Universities { get; set; }
    }
}
