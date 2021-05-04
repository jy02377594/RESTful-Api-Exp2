using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RESTful_Api_Exp2.Entities;
using RESTful_Api_Exp2.Models;
using RESTful_Api_Exp2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Controllers
{
    [ApiController]
    [Route(template: "api/employees")]
    //[Route(template:"api/employees")]
    //[Route(template:"api/[controller]")] //这种写法相当于把EmployeesController的controller去掉剩下的部分，这样写以后改的话路由也会变，合约是不应该随便改的
    public class EmployeesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        //这里要用到automapper做映射，所以要依赖注入IMapper接口
        public EmployeesController(IEmployeeRepository employeeRepository, IMapper mapper, ICompanyRepository companyRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }
        /// <summary>
        /// 如果返回类型可知,更清晰,能用ActionResult尽量不用IActionResult
        /// </summary>
        /// <returns></returns>
        /// 
        /*public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var employees = await _employeeRepository.GetEmployeesAsync();
            var employeDtos = new List<EmployeeDto>();
            foreach (var employee in employees)
            {
                employeDtos.Add(new EmployeeDto
                {
                    Id = employee.Id,
                    EmployeeName = employee.FirstName + employee.LastName,
                    EmployeeNo = employee.EmployeeNo,
                    CompanyId = employee.CompanyId,
                    HiredDate = employee.HiredDate
                });
            }
            //因为知道返回类型，可以直接返回Ok
            //return Ok();
            return employeDtos;
        }*/

        //public async Task<IActionResult> GetEmployees()
        //{
        //    var employees = await _employeeRepository.GetEmployeesAsync();
        //    // 404 NotFound();
        //    //return NotFound();
        //    //return new JsonResult(employees);

        //    var employeDtos = new List<EmployeeDto>();
        //    //后面这里用automapper对象映射，就不用每次一个一个写entity对应的model了
        //    foreach (var employee in employees)
        //    {
        //        employeDtos.Add(new EmployeeDto
        //        {
        //            Id = employee.Id,
        //            EmployeeName = employee.FirstName + employee.LastName,
        //            EmployeeNo = employee.EmployeeNo,
        //            CompanyId = employee.CompanyId,
        //            HiredDate = employee.HiredDate
        //        });
        //    }
        //    return Ok(employeDtos);
        //}

        //用automapper

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var employees = await _employeeRepository.GetEmployeesAsync();
            var employeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            
            return Ok(employeDtos);
        }

        //[HttpGet(template:"{employeeId}")] // api//Employess/{employeeId}
        [HttpGet(template: "{employeeId}")]
        //另外一种写法等同于 [HttpGet(template: "{employeeId}")]
        /*[HttpGet]
        [Route(template:"{employeeId}")]*/
        public async Task<ActionResult<EmployeeDto>> GetEmployee(Guid employeeId)
        {
            //高并发的时候，下面的查询存在，查询的时候另外的请求删掉了，结果应该是不存在导致查询结果不匹配
            //var exist = await _employeeRepository.EmployeeExistAsync(employeeId);
            //if (!exist) return NotFound();

            var employees = await _employeeRepository.GetEmployeesAsync(employeeId);
            if (employees == null) return NotFound();
            //return Ok(employees);
            return Ok(_mapper.Map<EmployeeDto>(employees));
        }

        //[HttpHead], head跟get的区别就是不传响应的body
        [HttpGet]
        [Route(template: "company/{companyId}", Name = nameof(GetEmployeesForCompany))]
        //gender没有来自route，所以只能来自query,用来过滤, (Name = "gender") 可以不加，加了代表请求的时候指定query参数名, q表示搜索，查询
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesForCompany(Guid companyId,[FromQuery(Name = "gender")] string genderDisplay, string q)
        {
            if (!await _companyRepository.CompanyExistAsync(companyId)) return NotFound();
            var employees = await _companyRepository.GetEmployeesAsync(companyId, genderDisplay, q);

            var employessDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(employessDtos);
        }

        //直接加companyId会和上面的[HttpGet(template: "{employeeId}")]冲突
        //[HttpPost(template:"{companyId}")]
        [HttpPost(template: "company/{companyId}")]
        public async Task<ActionResult<EmployeeDto>> CreateEmployeeForCompany(Guid companyId, EmployeeAddDto employee)
        {
            if (!await _companyRepository.CompanyExistAsync(companyId)) return NotFound();

            var entity = _mapper.Map<Employee>(employee);

            _employeeRepository.AddEmployee(companyId, entity);
            await _employeeRepository.SaveAsync();

            var returnDto = _mapper.Map<EmployeeDto>(entity);

            return CreatedAtRoute(nameof(GetEmployeesForCompany), routeValues: new
            { 
                companyId = companyId,
                employeeId = returnDto.Id
            }, value: returnDto);
        }

        //不更新的字段要把原值填上，不然就设成默认值了
        [HttpPut(template: "{employeeId}")]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid employeeId, EmployeePutDto employee)
        {
            if (!await _employeeRepository.EmployeeExistAsync(employeeId)) return NotFound();
            var employeeEntity = await _employeeRepository.GetEmployeesAsync(employeeId);
            if (employeeEntity == null) return NotFound();

            //entity转化成PutDto
            //传进来的employee更新到PutDto，把PutDto映射回entity去Profiles设置
            _mapper.Map(employee, employeeEntity);
            _employeeRepository.UpdateEmployee(employeeEntity);

            await _employeeRepository.SaveAsync();

            return NoContent();
        }
    }
}
