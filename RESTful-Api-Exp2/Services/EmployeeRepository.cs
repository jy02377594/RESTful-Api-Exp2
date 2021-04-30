﻿using Microsoft.EntityFrameworkCore;
using RESTful_Api_Exp2.Data;
using RESTful_Api_Exp2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Services
{
    public class EmployeeRepository: IEmployeeRepository
    {
        private readonly Restful_DbContext _context;

        public EmployeeRepository(Restful_DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        //get all employees depends on firstname and lastname, name could be same
        public async Task<IEnumerable<Employee>> GetEmployeesAsync(string FirstName, string LastName)
        {
            if (FirstName == null)
            {
                throw new ArgumentNullException(nameof(FirstName));
            }
            if (LastName == null)
            {
                throw new ArgumentNullException(nameof(LastName));
            }
            return await _context.Employees
                .Where(x => x.FirstName == FirstName && x.LastName == LastName).ToListAsync();
        }

        //get an employee by id
        public async Task<Employee> GetEmployeesAsync(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            return await _context.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);
        }

        public void AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            employee.Id = Guid.NewGuid();
            _context.Employees.Add(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            _context.Employees.Remove(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            // the reason why I can comment this line of core is EF Core realtimely trace entity。
            // _context.Entry(employee).State = EntityState.Modified;
        }

        public async Task<bool> EmployeeExistAsync(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }
            return await _context.Employees
                .AnyAsync(x => x.Id == employeeId);
        }

        

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) >= 0;
        }
    }
}
