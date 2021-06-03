using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RESTful_Api_Exp2.DtoParameters;
using RESTful_Api_Exp2.Entities;
using RESTful_Api_Exp2.Helpers;
using RESTful_Api_Exp2.Models;
using RESTful_Api_Exp2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Controllers
{
    // Controller class inherits from ControllerBase class, it supports the View
    // Controller class is used for Web Mvc and Web Api, ControllerBase support web api only
    [ApiController]
    [Route(template: "api/companies")]
    public class CompaniesController : ControllerBase
    {
        // container of services, 容器
        public readonly IEmployeeRepository _employeeRepository;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;
        public readonly ICompanyRepository _companyRepository;
        public readonly IMapper _mapper;

        /// <summary>
        /// 依赖注入：容器全权负责组件的装配，它会把符合依赖关系的对象通过构造函数传递给需要的对象。
        /// 符合依赖倒置原则，高层模块不应该依赖低层模块，两者都应该依赖其抽象
        /// </summary>
        /// <param name="companyRepository"></param>
        public CompaniesController(ICompanyRepository companyRepository, IMapper mapper, IEmployeeRepository employeeRepository,
            IPropertyMappingService propertyMappingService, IPropertyCheckerService propertyCheckerService)
        {
            //dependency injection           
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
            _propertyCheckerService = propertyCheckerService ?? throw new ArgumentNullException(nameof(propertyCheckerService));
        }

        [HttpGet(Name = nameof(GetCompanies))]
        //查询参数，当需要对数据进行各种条件查询时需要参数，以后如果需要查询的参数多了，不需要改这里的代码，只用在CompanyDtoParameters类里添加就行了
        //不指定来源是query的话会被apicontroller默认为是来自于请求的Body里，所以这里要注明不然返回415错误
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies([FromQuery] CompanyDtoParameters parameters)
        {
            var companies = await _companyRepository.GetCompaniesAsync(parameters);
            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return Ok(companyDtos);
        }

        [HttpGet("GetCompanyWithPage", Name = nameof(GetCompaniesWithPage))]
        [HttpHead]
        public async Task<IActionResult> GetCompaniesWithPage([FromQuery] CompanyDtoParameters parameters)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<CompanyDto, Company>(parameters.OrderBy)) return BadRequest();

            if (!_propertyCheckerService.TypeHasProperties<CompanyDto>(parameters.Fields)) return BadRequest();
            var companies = await _companyRepository.GetCompaniesAsyncWithPL(parameters);
            //var previousLink = companies.HasPrevious ? CreateCompaniesResourceUri(parameters, ResourceUriType.PreviousPage) : null;
            //var nextLink = companies.HasNext ? CreateCompaniesResourceUri(parameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = companies.TotalCount,
                pageSize = companies.PageSize,
                currentPage = companies.CurrentPage,
                totalPages = companies.TotalPages,
                //previousLink,
                //nextLink
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));

            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            var shapedData = companyDtos.ShapeData(parameters.Fields);
            var links = CreateLinksForCompany(parameters, companies.HasPrevious, companies.HasNext);
            //{value:[xxx], links}

            var shapedCompaniesWithLinks = shapedData.Select(c =>
            {
                var companyDict = c as IDictionary<string, object>;
                var companyLinks = CreateLinksForCompany((Guid)companyDict["Id"], null);
                companyDict.Add("links", companyLinks);
                return companyDict;
            });

            var linkedCollectionResource = new
            {
                value = shapedCompaniesWithLinks,
                links
            };
            
            return Ok(linkedCollectionResource);
        }

        [HttpGet(template: "{companyId}", Name = nameof(GetCompany))]
        public async Task<IActionResult> GetCompany(Guid companyId, string fields)
        {
            if (!_propertyCheckerService.TypeHasProperties<CompanyDto>(fields)) return BadRequest();
            var company = await _companyRepository.GetCompaniesAsync(companyId);
            if (company == null) return NotFound();
            //var companyDtos = new List<CompanyDto>();
            //companyDtos.Add(new CompanyDto
            //{
            //    Id = company.Id,
            //    Name = company.Name
            //});
            var links = CreateLinksForCompany(companyId, fields);
            var companyDtos = _mapper.Map<CompanyDto>(company);
            var linkedDict = companyDtos.ShapeData(fields) as IDictionary<string, object>;

            linkedDict.Add("links", links);
            return Ok(linkedDict);
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
        //http请求不加Name就获取不到nameof,结果"href": null
        [HttpPost(Name = nameof(CreateCompany))]
        public async Task<ActionResult<CompanyDto>> CreateCompany([FromBody] CompanyAddDto company)
        {
            //.net core 2.0 没有api controller，需要下面这段代码
            if (company == null) return BadRequest();

            var entity = _mapper.Map<Company>(company);
            _companyRepository.AddCompany(entity);
            await _companyRepository.SaveAsync();

            var returnDto = _mapper.Map<CompanyDto>(entity);

            var links = CreateLinksForCompany(returnDto.Id, null);
            var linkedDict = returnDto.ShapeData(null) as IDictionary<string, object>;
            linkedDict.Add("links", links);

            //返回响应带地址的header
            return CreatedAtRoute(nameof(GetCompany), routeValues: new { companyId = linkedDict["Id"] }, value: linkedDict);
        }

        [HttpPut(template: "{companyId}")]
        public async Task<IActionResult> CreateCompany(Guid companyId, [FromBody] CompanyUpdateDto company)
        {
            //if (!await _companyRepository.CompanyExistAsync(companyId)) return NotFound();
            if (company == null) return NotFound();
            var CompanyEntity = await _companyRepository.GetCompaniesAsync(companyId);

            //如果有这个数据就执行下面的更新，没有就走if里的添加
            if (CompanyEntity == null)
            {
                var companyAddDtoEntity = _mapper.Map<Company>(company);

                _companyRepository.AddCompany(companyId, companyAddDtoEntity);
                await _companyRepository.SaveAsync();
                var companyAddDto = _mapper.Map<CompanyDto>(companyAddDtoEntity);
                return CreatedAtRoute(nameof(GetCompany), routeValues: new { companyId = companyId }, value: companyAddDto);
            }
            _mapper.Map(company, CompanyEntity);


            _companyRepository.UpdateCompany(CompanyEntity);
            await _companyRepository.SaveAsync();

            //更新后展示新的数据
            var companyDto = _mapper.Map<CompanyDto>(CompanyEntity);
            return CreatedAtRoute(nameof(GetCompany), routeValues: new { companyId = companyId }, value: companyDto);
            //也可以啥都不展示,返回的是204
            //return NoContent();
        }

        //根据companyId批量添加company
        [HttpPost(template: "companycollections")]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> CreateCompanyCollection(IEnumerable<CompanyAddDto> companyCollection)
        {
            if (companyCollection == null) return BadRequest();
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _companyRepository.AddCompany(company);
            }

            await _companyRepository.SaveAsync();

            //这里批量添加完company有两种返回展示的方法，一种是直接返回之前的查询出所有companies的方法GetCompanies，一种是返回添加的这些companies，需要添加绑定器
            var companyDto = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            //return CreatedAtRoute(nameof(GetCompanies), value: companyDto);
            //带绑定器的返回
            var idsString = string.Join(",", companyDto.Select(x => x.Id));
            return CreatedAtRoute(nameof(GetCompanyCollection), new { ids = idsString }, companyDto);
        }

        //Key 可以写成 1,2,3 或者 Ke1 = Value1, Key2 = Value2, Key3 = Value3
        [HttpGet(template: "companycollections/{ids}", Name = nameof(GetCompanyCollection))]
        public async Task<IActionResult> GetCompanyCollection([FromRoute]
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null) return BadRequest();

            var entities = await _companyRepository.GetCompaniesAsync(ids);

            if (ids.Count() != entities.Count()) return NotFound();

            var dtosToReturn = _mapper.Map<IEnumerable<CompanyDto>>(entities);
            return Ok(dtosToReturn);
        }

        [HttpDelete("{CompanyId}", Name = nameof(DeleteCompany))]
        public async Task<IActionResult> DeleteCompany(Guid companyId)
        {
            var companyEntity = await _companyRepository.GetCompaniesAsync(companyId);
            if (companyEntity == null) return NotFound();

            await _employeeRepository.GetEmployeesAsync(companyId, null);
            _companyRepository.DeleteCompany(companyEntity);
            await _companyRepository.SaveAsync();

            return NoContent();
        }

        //option请求可以获取针对某个webapi的通信选项的信息,不需要异步，不操作数据库
        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, OPTIONS");
            return Ok();
        }

        //生成翻页后的前后页uri
        private string CreateCompaniesResourceUri(CompanyDtoParameters parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetCompaniesWithPage), new
                    {
                        fields = parameters.Fields,
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm,
                        orderBy = parameters.OrderBy
                    });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetCompaniesWithPage), new
                    {
                        fields = parameters.Fields,
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm,
                        orderBy = parameters.OrderBy
                    });

                case ResourceUriType.CurrentPage:
                default:
                    return Url.Link(nameof(GetCompaniesWithPage), new
                    {
                        fields = parameters.Fields,
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm,
                        orderBy = parameters.OrderBy
                    });
            }
        }

        //HATEOAS级别的传输，先创建一个link
        private IEnumerable<LinkDto> CreateLinksForCompany(Guid companyId, string fields)
        {
            var links = new List<LinkDto>();
            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new LinkDto(Url.Link(nameof(GetCompany), new { companyId }), "self", "GET"));
            }
            else
            {
                links.Add(new LinkDto(Url.Link(nameof(GetCompany), new { companyId, fields }), "self", "GET"));
            }

            links.Add(new LinkDto(Url.Link(nameof(DeleteCompany), new { companyId }), "delete_company", "DELETE"));
            links.Add(new LinkDto(Url.Link(nameof(EmployeesController.CreateEmployeeForCompany),new { companyId}), "create_employee_for_company","POST"));
            links.Add(new LinkDto(Url.Link(nameof(EmployeesController.GetEmployeesWithOrder), new { companyId }), "employee", "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForCompany(CompanyDtoParameters parameters, bool hasPrevious, bool hasNext)
        {  
            var links = new List<LinkDto>();

            links.Add(new LinkDto(CreateCompaniesResourceUri(parameters, ResourceUriType.CurrentPage), "self", "GET"));

            if (hasPrevious)
            {
                links.Add(new LinkDto(CreateCompaniesResourceUri(parameters, ResourceUriType.PreviousPage), "previousPage", "GET"));
            }

            if (hasNext)
            {
                links.Add(new LinkDto(CreateCompaniesResourceUri(parameters, ResourceUriType.NextPage), "nextPage", "GET"));
            }

            return links;
        }
    }
}
