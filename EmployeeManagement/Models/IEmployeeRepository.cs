using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
   public  interface IEmployeeRepository
    {
        Employee Get(int Id);
        IEnumerable<Employee> GetAllEmployees();
        Employee Add(Employee employee);
        Employee Delete(int Id);
        Employee Update(Employee employeeChanges);


    }
}
