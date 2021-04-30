using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RESTful_Api_Exp2.DtoParameters;
using RESTful_Api_Exp2.Models;
using RESTful_Api_Exp2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Controllers
{
    // Controller class inherits from ControllerBase class, it supports the View
    // Controller class is used for Web Mvc and Web Api, ControllerBase support web api only
    [ApiController]
    [Route(template:"api/companies")]
    public class CompaniesController : ControllerBase
    {
        // services
        public readonly IEmployeeRepository _employeeRepository;
        public readonly ICompanyRepository _companyRepository;
        public readonly IMapper _mapper;

        /// <summary>
        /// 依赖注入：容器全权负责组件的装配，它会把符合依赖关系的对象通过构造函数传递给需要的对象。
        /// 符合依赖倒置原则，高层模块不应该依赖低层模块，两者都应该依赖其抽象
        /// </summary>
        /// <param name="companyRepository"></param>
        public CompaniesController(ICompanyRepository companyRepository, IMapper mapper, IEmployeeRepository employeeRepository)
        {
            //dependency injection           
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }
        //测试git
        [HttpGet]
        //查询参数，当需要对数据进行各种条件查询时需要参数，以后如果需要查询的参数多了，不需要改这里的代码，只用在CompanyDtoParameters类里添加就行了
        //不指定来源是query的话会被apicontroller默认为是来自于请求的Body里，所以这里要注明不然返回415错误
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies([FromQuery]CompanyDtoParameters parameters)
        {
            var companies = await _companyRepository.GetCompaniesAsync(parameters);
            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return Ok(companyDtos);
        }

        [HttpGet(template: "{companyId}")]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompany(Guid companyId)
        {
            var company = await _companyRepository.GetCompaniesAsync(companyId);
            if (company == null) return NotFound();
            //var companyDtos = new List<CompanyDto>();
            //companyDtos.Add(new CompanyDto
            //{
            //    Id = company.Id,
            //    Name = company.Name
            //});
            var companyDtos = _mapper.Map<CompanyDto>(company);

            return Ok(companyDtos);
        }

        [HttpGet]
        [Route(template: "{companyId}/{employeeId}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeForCompanyAndId(Guid companyId, Guid employeeId)
        {
            if (!await _companyRepository.CompanyExistAsync(companyId)) return NotFound();
            if (!await _employeeRepository.EmployeeExistAsync(employeeId)) return NotFound();

            var employee = await _companyRepository.GetEmployeesAsync(companyId, employeeId);
            var employeeDtos = _mapper.Map<EmployeeDto>(employee);
            return Ok(employeeDtos);
        }
    }
}
