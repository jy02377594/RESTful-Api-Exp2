using Microsoft.EntityFrameworkCore;
using RESTful_Api_Exp2.Data;
using RESTful_Api_Exp2.DtoParameters;
using RESTful_Api_Exp2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly Restful_DbContext _context;

        public CompanyRepository(Restful_DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //用查询参数来模糊查询有关的公司，这里只根据Name和Introduction来查询，如果这两个字段包含查询参数就返回相应的公司数据
        public async Task<IEnumerable<Company>> GetCompaniesAsync(CompanyDtoParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (string.IsNullOrWhiteSpace(parameters.CompanyName) && string.IsNullOrWhiteSpace(parameters.SearchTerm))
            { return await _context.Companies.ToListAsync(); }

            var queryExpression = _context.Companies as IQueryable<Company>;
            if (!string.IsNullOrWhiteSpace(parameters.CompanyName))
            {
                parameters.CompanyName = parameters.CompanyName.Trim();
                queryExpression = queryExpression.Where(x => x.Name == parameters.CompanyName);
            }

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                parameters.SearchTerm = parameters.SearchTerm.Trim();
                queryExpression = queryExpression.Where(x => x.Name.Contains(parameters.SearchTerm) || x.Introduction.Contains(parameters.SearchTerm));
            }
            return await queryExpression.ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds)
        {
            if (companyIds == null) throw new ArgumentNullException(nameof(companyIds));

            return await _context.Companies
                .Where(x => companyIds.Contains(x.Id))
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<bool> CompanyExistAsync(Guid companyId)
        {
            if (companyId == Guid.Empty) throw new ArgumentNullException(nameof(companyId));
            return await _context.Companies.AnyAsync(x => x.Id == companyId);
        }

        public async Task<Company> GetCompaniesAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            return await _context.Companies.FirstOrDefaultAsync(x => x.Id == companyId);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, string genderDisplay, string q)
        {
            if (companyId == Guid.Empty) throw new ArgumentNullException(nameof(companyId));
            
            //过滤和搜索为空直接返回结果
            if(string.IsNullOrWhiteSpace(genderDisplay) && string.IsNullOrWhiteSpace(q)) 
            return await _context.Employees.Where(x => x.CompanyId == companyId).OrderBy(x => x.EmployeeNo).ToListAsync();

            //var items = _context.Employees as IQueryable<Employee>;
            //相当于拼接sql语句查询出来指定companyId的数据itemms, 然后对items进行操作来过滤和搜索
            var items = _context.Employees.Where(x => x.CompanyId == companyId);


            //性别过滤
            if (!string.IsNullOrWhiteSpace(genderDisplay))
            {
                genderDisplay = genderDisplay.Trim();
                var gender = Enum.Parse<Gender>(genderDisplay); // 字符串转换成自定义的Gender类型

                items = items.Where(x => x.Gender == gender);
            }

            //搜索
            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                //员工号，姓名里模糊搜索一下
                items = items.Where(x => x.EmployeeNo.Contains(q) || x.FirstName.Contains(q) || x.LastName.Contains(q));
            }

            return await items
                .OrderBy(x => x.EmployeeNo).ToListAsync();
        }

        //get an employee by id
        public async Task<Employee> GetEmployeesAsync(Guid companyId, Guid employeeId)
        {
            if (employeeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }

            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            return await _context.Employees.Where(x => x.Id == employeeId && x.CompanyId == companyId).FirstOrDefaultAsync();
        }

        public void AddCompany(Company company)
        {
            if (company == null) throw new ArgumentNullException(nameof(company));

            company.Id = Guid.NewGuid();
            //Company实体里有ICollection<Employee>，为一对多关系，所以遍历这个Company实体看有多少Employee,每个Employee创建id
            //这里的添加是对Company实体操作
            if (company.Employees != null)
            {
                foreach (var employee in company.Employees)
                {
                    employee.Id = Guid.NewGuid();
                }
            }

            _context.Companies.Add(company);
        }

        public void AddCompany(Guid companyId, Company company)
        {
            if (company == null) throw new ArgumentNullException(nameof(company));
            company.Id = companyId;
            if (company.Employees != null)
            {
                foreach (var employee in company.Employees) employee.Id = Guid.NewGuid();
            }

            _context.Companies.Add(company);
        }

        public void UpdateCompany(Company company)
        { 
           //这里不用谢，EF core 会追踪字段变化，最后执行SaveAsync保存
        }
        public void DeleteCompany()
        {
            
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

    }
}
