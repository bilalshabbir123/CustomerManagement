using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        { 
              modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id=2,
                    Name="Shahid Abbas",
                    Department=Dept.IT,
                    Email="shahid@gmail.com"
                }, new Employee
                {
                    Id = 3,
                    Name = "Arfeen",
                    Department = Dept.HR,
                    Email = "arfeen@gmail.com"
                }
                );
        }
    }
}
