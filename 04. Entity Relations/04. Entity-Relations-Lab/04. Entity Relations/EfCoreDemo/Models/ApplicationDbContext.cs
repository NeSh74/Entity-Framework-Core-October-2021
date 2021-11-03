using System.Security.Cryptography.X509Certificates;
using EfCoreDemo.ModelBuilding;
using Microsoft.EntityFrameworkCore;

namespace EfCoreDemo.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions options)
        : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Club> Clubs { get; set; }

        public DbSet<EmployeeInClub> EmployeesInClubs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=EfCoreDemo;Integrated Security=true");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().Property(x => x.Name).IsRequired().HasMaxLength(50);

            // modelBuilder.Entity<Department>().

            //modelBuilder.Entity<Employee>().HasKey(x => new
            //{
            //    x.EID,
            //    x.Egn
            //});

            //modelBuilder.Entity<Employee>().Property(x => x.FirstName).IsRequired();//not null

            //modelBuilder.Entity<Employee>().Property(x => x.FirstName)
            //    .HasMaxLength(20);

            //modelBuilder.Entity<Employee>()
            //    .Property(x => x.LastName)
            //    .IsRequired()
            //    .HasMaxLength(30);//not null

            //modelBuilder.Entity<Employee>().Ignore(x => x.FullName);   

            //modelBuilder.Entity<Employee>().Property(x => x.Salary).ValueGeneratedOnAddOrUpdate();

            //modelBuilder
            //    .Entity<Employee>()
            //    //  .ToTable("People", "company");
            //    .Property(x => x.StartWorkDay)
            //    .HasColumnName("StartedOn")
            //    .HasColumnType("date"); //avoid this
            //modelBuilder.Entity<Employee>()
            //    .HasOne(x => x.Department) //required
            //    .WithMany(x => x.Employees) //optional (inverse)
            //    .HasForeignKey(x => x.DepartmentId) //db column name (optional)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());

            //modelBuilder.Entity<Address>()
            //    .HasOne(x => x.Employee)
            //    .WithOne(x => x.Address);

            //modelBuilder.Entity<Employee>()
            //    .HasOne(x => x.Department)
            //    .WithMany(x => x.Employees)
            //    .HasForeignKey(x => x.DepartmentId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Department>()
            //    .HasMany(x => x.Employees)
            //    .WithOne(x => x.Department)
            //    .HasForeignKey(x => x.DepartmentId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<EmployeeInClub>().HasKey(x => new {x.EmployeeId, x.ClubId});

            base.OnModelCreating(modelBuilder);
        }
    }
}
