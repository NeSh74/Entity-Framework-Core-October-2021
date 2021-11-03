using EfCoreDemo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCoreDemo.ModelBuilding
{
    public class EmployeeConfiguration:IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            //builder.HasKey(x => new
            //{
            //    x.Id, //EID
            //    x.Egn
            //});

            //builder.Property(x => x.FirstName);
            //.IsRequired();//not null

            //builder.Property(x => x.FirstName);
             //   .HasMaxLength(20);

             //builder
             //    .Property(x => x.LastName);
             //   .IsRequired()
               // .HasMaxLength(30);//not null

            //builder.Ignore(x => x.FullName);

            builder.Property(x => x.Salary).ValueGeneratedOnAddOrUpdate();

           //builder
           //     .Property(x => x.StartWorkDay)
           //  //   .HasColumnName("StartedOn")
           //     .HasColumnType("date"); //avoid this

           builder
                .HasOne(x => x.Department) //required
                .WithMany(x => x.Employees) //optional (inverse)
              ///  .HasForeignKey(x => x.DepartmentId) //db column name (optional)
              //  .HasForeignKey(x => x.DID) //db column name (optional)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
