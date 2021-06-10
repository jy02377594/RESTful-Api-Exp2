using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RESTful_Api_Exp2.DtoParameters;
using RESTful_Api_Exp2.Entities;
using RESTful_Api_Exp2.Models;
using RESTful_Api_Exp2.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Controllers
{
    [ApiController]
    [Route(template: "api/employees")]
    //总结一下，employee的外键是companyId,且companyId不可为空，这里就高度依赖company这个实体了，所以在设计路由的时候应该体现出来
    //既然每个employee必须得有个companyId,这里的路由最好是先传companyId体现两个表的关系"api/company/{companyId}/employees"
    //这里如果路由设计的不好，后面的逻辑就会更复杂
    //[Route(template:"api/employees")]
    //[Route(template:"api/[controller]")] //这种写法相当于把EmployeesController的controller去掉剩下的部分，这样写以后改的话路由也会变，合约是不应该随便改的
    public class EmployeesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeTaskRepository _employeeTaskRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        //这里要用到automapper做映射，所以要依赖注入IMapper接口
        public EmployeesController(IEmployeeRepository employeeRepository, IMapper mapper, ICompanyRepository companyRepository, IEmployeeTaskRepository employeeTaskRepository, IWebHostEnvironment env)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _employeeTaskRepository = employeeTaskRepository ?? throw new ArgumentNullException(nameof(employeeTaskRepository));
            _env = env ?? throw new ArgumentNullException(nameof(env));
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

        //直接加companyId会和上面的[HttpGet(template: "{employeeId}")]冲突
        //[HttpHead], head跟get的区别就是不传响应的body
        [HttpGet]
        [ResponseCache(Duration = 120)]
        [Route(template: "company/{companyId}", Name = nameof(GetEmployeesForCompany))]
        //gender没有来自route，所以只能来自query,用来过滤, (Name = "gender") 可以不加，加了代表请求的时候指定query参数名, q表示搜索，查询
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesForCompany(Guid companyId, [FromQuery(Name = "gender")] string genderDisplay, string q)
        {
            if (!await _companyRepository.CompanyExistAsync(companyId)) return NotFound();
            var employees = await _companyRepository.GetEmployeesAsync(companyId, genderDisplay, q);

            var employessDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(employessDtos);
        }

        [HttpGet("company/{companyId}/order", Name = nameof(GetEmployeesWithOrder))]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesWithOrder(Guid companyId, [FromQuery] EmployeeDtoParameter parameters)
        {
            if (!await _companyRepository.CompanyExistAsync(companyId)) return NotFound();
            var employees = await _employeeRepository.GetEmployeesAsync(companyId, parameters);

            var employeesDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(employeesDtos);
        }


        //[HttpPost(template:"{companyId}")]
        [HttpPost(template: "{companyId}", Name = nameof(CreateEmployeeForCompany))]
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

        //为什么路由改成这样就400错误
        //[HttpPatch("company/{companyId}/{employeeId}")]
        //public async Task<IActionResult> UpdateEmployeeForCompany([FromRoute]Guid companyId, [FromRoute]Guid employeeId, JsonPatchDocument<EmployeePutDto> patchDocument)
        //{
        //    if (!await _companyRepository.CompanyExistAsync(companyId)) return NotFound();
        //    var employeeEntity = await _employeeRepository.GetEmployeesAsync(employeeId);
        //    if (employeeEntity == null) return NotFound();

        //    //根据字段查出来的数据格式是employee类型，但是我们要修改的时候是employeePutDto类型，所以要去做一下employee到employeePutDto类型的映射，类型映射
        //    var dtoToPatch = _mapper.Map<EmployeePutDto>(employeeEntity);
        //    //需要处理验证错误
        //    patchDocument.ApplyTo(dtoToPatch);

        //    //两个对象的映射
        //    _mapper.Map(dtoToPatch, employeeEntity);
        //    _employeeRepository.UpdateEmployee(employeeEntity);
        //    await _employeeRepository.SaveAsync();

        //    return NoContent();
        //}

        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> UpdateEmployeeForCompany([FromRoute] Guid employeeId, JsonPatchDocument<EmployeePutDto> patchDocument)
        {
            var employeeEntity = await _employeeRepository.GetEmployeesAsync(employeeId);
            //如果没有这个employee不能更新就插入新的employee
            if (employeeEntity == null)
            {
                var employeeDto = new EmployeePutDto();
                patchDocument.ApplyTo(employeeDto, ModelState);

                if (!TryValidateModel(employeeDto))
                {
                    return ValidationProblem(ModelState);
                }

                var employeeToAdd = _mapper.Map<Employee>(employeeDto);
                employeeToAdd.Id = employeeId;
                //这里因为外键约束不能为空或不存在,所以这里插了一个存在的companyId
                //Guid companyId = Guid.Empty;
                Guid companyId = Guid.Parse("a3a461ea-e692-6f54-2f3e-f076a08dda14");
                _employeeRepository.AddEmployee(companyId, employeeToAdd);
                await _employeeRepository.SaveAsync();

                var dtoToReturn = _mapper.Map<EmployeeDto>(employeeToAdd);
                return CreatedAtRoute(nameof(GetEmployeesForCompany), routeValues: new
                {
                    companyId = companyId,
                    employeeId = dtoToReturn.Id
                }, value: dtoToReturn);

            }

            //根据字段查出来的数据格式是employee类型，但是我们要修改的时候是employeePutDto类型，所以要去做一下employee到employeePutDto类型的映射，类型映射
            var dtoToPatch = _mapper.Map<EmployeePutDto>(employeeEntity);

            //patchDocument是客户端传过来的数据，这时要用上json操作数组来对dtoToPatch操作, ModelState会返回严重false如果验证有错误
            patchDocument.ApplyTo(dtoToPatch, ModelState);

            //处理验证错误
            if (!TryValidateModel(dtoToPatch))
            {
                return ValidationProblem(ModelState);
            };

            //两个对象的映射
            _mapper.Map(dtoToPatch, employeeEntity);
            _employeeRepository.UpdateEmployee(employeeEntity);
            await _employeeRepository.SaveAsync();

            return NoContent();
        }

        //DbContext里已经设置成级联删除了
        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployeeWithTask(Guid employeeId)
        {
            var employeeEntity = await _employeeRepository.GetEmployeesAsync(employeeId);
            if (employeeEntity == null) return NotFound();

            //虽然级联删除设置了，但是这里要查询一下employee名下的task，加载到dbcontext里
            await _employeeTaskRepository.GetTasksAsync(employeeId);

            _employeeRepository.DeleteEmployee(employeeEntity);
            await _employeeRepository.SaveAsync();

            return NoContent();
        }

        //upload file to Service with a path
        [HttpPost("SaveFile")]
        public async Task<IActionResult> SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/EmployeePhoto/" + fileName;
                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                await _employeeRepository.SaveAsync();
                return Ok(fileName);
            }
            catch (Exception e)
            {
                if (e.GetType().Name != "")
                {
                    throw new Exception(nameof(e));
                }
                else return BadRequest();
            }
        }
        //return filename
        /*public JsonResult SaveFile()
        {
            try {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/EmployeePhoto/" + filename;
                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }*/


        public override ActionResult ValidationProblem(ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
