using RESTful_Api_Exp2.DtoParameters;
using RESTful_Api_Exp2.Entities;
using RESTful_Api_Exp2.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Services
{
    public interface ICompanyRepository
    {
        Task<bool> CompanyExistAsync(Guid companyId);
        Task<IEnumerable<Company>> GetCompaniesAsync(CompanyDtoParameters parameters);
        Task<PagedList<Company>> GetCompaniesAsyncWithPL(CompanyDtoParameters parameters);
        Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds);
        //Task<IEnumerable<Company>> GetCompaniesAsync(string FirstName, string LastName);
        Task<Employee> GetEmployeesAsync(Guid companyId, Guid employeeId);
        Task<Company> GetCompaniesAsync(Guid companyId);
        //void AddCompany(Company company);
        //void UpdateCompany(Company company);
        // void DeleteCompany(Company company);

        void AddCompany(Company company);
        void AddCompany(Guid companyId, Company company);
        void UpdateCompany(Company company);
        void DeleteCompany(Company company);

        Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, string genderDisplay, string q);
        Task<bool> SaveAsync();
    }
}
