using RESTful_Api_Exp2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Services
{
    public interface IEmployeeRepository
    {
        // get all employees' info 
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<IEnumerable<Employee>> GetEmployeesAsync(string FirstName, string LastName);
        Task<Employee> GetEmployeesAsync(Guid EmployeeId);
        void AddEmployee(Guid companyId, Employee entity);
       // void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
        Task<bool> EmployeeExistAsync(Guid? employeeId);
        Task<bool> SaveAsync();
    }
}
