using Microsoft.EntityFrameworkCore;
using RESTful_Api_Exp2.Data;
using RESTful_Api_Exp2.DtoParameters;
using RESTful_Api_Exp2.Entities;
using RESTful_Api_Exp2.Helpers;
using RESTful_Api_Exp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Services
{
    public class EmployeeRepository: IEmployeeRepository
    {
        private readonly Restful_DbContext _context;
        private readonly IPropertyMappingService _propertyMappingService;

        public EmployeeRepository(Restful_DbContext context, IPropertyMappingService propertyMappingService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
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

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, EmployeeDtoParameter parameters)
        {
            if (companyId == Guid.Empty) throw new ArgumentNullException(nameof(companyId));
            //if (string.IsNullOrWhiteSpace(parameters.Gender) && string.IsNullOrWhiteSpace(parameters.Q))
            //{
            //    return await _context.Employees
            //        .Where(x => x.CompanyId == companyId)
            //        .OrderBy(x => x.EmployeeNo)
            //        .ToListAsync();
            //}

            var items = _context.Employees.Where(x => x.CompanyId == companyId);

            if (!string.IsNullOrWhiteSpace(parameters.Q))
            {
                parameters.Q = parameters.Q.Trim();
                items = items.Where(x => x.EmployeeNo.Contains(parameters.Q)
                || x.FirstName.Contains(parameters.Q)
                || x.LastName.Contains(parameters.Q));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Gender))
            {
                parameters.Gender = parameters.Gender.Trim();
                //替换成枚举里的gender
                var gender = Enum.Parse<Gender>(parameters.Gender);
                items = items.Where(x => x.Gender == gender);
            }

            /*if (!string.IsNullOrWhiteSpace(parameters.OrderBy))
            {
                if (parameters.OrderBy.ToLowerInvariant() == "employeename")
                {
                    items = items.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);
                }
            }*/

            //把dictionary取出来
            var mappingDictionary = _propertyMappingService.GetPropertyMapping<EmployeeDto, Employee>();
            //使用属性名的字符串来按属性进行排序，有的属性名是lambda表达, 属性映射服务，可以映射Entity多个属性，映射可能翻转顺序。(age asc: dateofbirth desc)
            items = items.ApplySort(parameters.OrderBy, mappingDictionary);

            //return await items.OrderBy(x => x.EmployeeNo).ToListAsync();
            return await items.ToListAsync();
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

        public void AddEmployee(Guid companyId, Employee entity)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //添加新的employee, 没有给新的id, 下面的Add方法会自动生成一个id
            //entity.Id = Guid.NewGuid();
            entity.CompanyId = companyId;
            if (entity.Tasklist != null)
            {
                foreach (var task in entity.Tasklist)
                {
                    task.taskId = Guid.NewGuid();
                }
            }
            _context.Employees.Add(entity);
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

        public async Task<bool> EmployeeExistAsync(Guid? employeeId)
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
