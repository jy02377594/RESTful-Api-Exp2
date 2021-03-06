using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
using System.Web;

namespace RESTful_Api_Exp2.Controllers
{
    [ApiController]
    //这里的业务逻辑，task是不应该完全依赖employee的，因为业务需求很可能先创建任务但是没有想好分配给谁，所以会创建一个空employeeId的task
    //因为employeeId可能会为空，所以这里的路由不应该先把employeeId加上，避免后面的逻辑复杂
    //[Route(template:"api/employees/{employeeId}/employeetask")]
    [Route("api/employeetask")]
    public class EmployeesTaskController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeTaskRepository _employeeTaskRepository;
        private readonly IPropertyMappingServiceForTask _propertyMappingServiceForTask;

        public EmployeesTaskController(IEmployeeRepository employeeRepository, IMapper mapper, IEmployeeTaskRepository employeeTaskRepository, IPropertyMappingServiceForTask propertyMappingServiceForTask)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _employeeTaskRepository = employeeTaskRepository ?? throw new ArgumentNullException(nameof(employeeTaskRepository));
            _propertyMappingServiceForTask = propertyMappingServiceForTask ?? throw new ArgumentNullException(nameof(propertyMappingServiceForTask));
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeTaskDto>>> GetAllEmployeesTasks()
        {
            var employeesTasks = await _employeeTaskRepository.GetTasksAsync();
            var employeeTaskDtos = _mapper.Map<IEnumerable<EmployeeTaskDto>>(employeesTasks);
            return Ok(employeeTaskDtos);
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<EmployeeTaskDto>>> GetTasksByEmployeeId(Guid employeeId)
        {
            if (!await _employeeRepository.EmployeeExistAsync(employeeId)) return NotFound();

            var employeeTasks = await _employeeTaskRepository.GetTasksAsync(employeeId);
            var employeeTasksDtos = _mapper.Map<IEnumerable<EmployeeTaskDto>>(employeeTasks);

            return Ok(employeeTasksDtos);
        }

        //这里路由加TaskName因为会获取employeeId进入api/employees/{employeeId}/employeetask，会导致multiple endpoints和上面的GetTasksByEmployeeId方法冲突。
        [HttpGet(template: "SelectByTaskName")]
        public async Task<ActionResult<EmployeeTaskDto>> GetTaskByTaskName([FromQuery(Name = "TaskName")] string TaskName)
        {
            if (String.IsNullOrWhiteSpace(TaskName)) return NotFound();

            var employeeTask = await _employeeTaskRepository.GetOneTaskAsync(TaskName);
            if (employeeTask == null) return NotFound();
            var employeeTaskDtos = _mapper.Map<EmployeeTaskDto>(employeeTask);

            return Ok(employeeTaskDtos);
        }

        [HttpGet(template: "{taskId}", Name = nameof(GetTaskByTaskId))]
        public async Task<ActionResult<EmployeeTaskDto>> GetTaskByTaskId(Guid taskId)
        {
            var employeeTask = await _employeeTaskRepository.GetOneTaskAsync(taskId);
            if (employeeTask == null) return NotFound();

            var employeeTaskDot = _mapper.Map<EmployeeTaskDto>(employeeTask);
            return Ok(employeeTaskDot);
        }

        [HttpGet(template: "SelectByTaskList")]
        public async Task<ActionResult<IEnumerable<EmployeeTaskDto>>> GetTaskByTaskList([FromQuery(Name = "TaskList")] string TaskList)
        {
            if (String.IsNullOrWhiteSpace(TaskList)) return NotFound();

            var employeeTasks = await _employeeTaskRepository.GetTasksAsync(TaskList);
            if (employeeTasks == null) return NotFound();
            var employeeTaskDtos = _mapper.Map<IEnumerable<EmployeeTaskDto>>(employeeTasks);

            return Ok(employeeTaskDtos);
        }

        [HttpPost(template: "employee/{employeeId}/CreateTaskWithEmployeeId")]
        public async Task<ActionResult<EmployeeTaskDto>> CreateNewTaskForEmployee(Guid employeeId, [FromBody] EmployeeTaskAddDto employeeTask)
        {
            //EmployeeExistAsync返回类型是task<bool>,这里是异步的，所以要await
            if (!await _employeeRepository.EmployeeExistAsync(employeeId)) return NotFound();
            var entity = _mapper.Map<EmployeeTask>(employeeTask);

            _employeeTaskRepository.AddTask(employeeId, entity);
            await _employeeTaskRepository.SaveAsync();

            var employeeTaskDto = _mapper.Map<EmployeeTaskDto>(entity);

            return CreatedAtRoute(nameof(GetTaskByTaskId), routeValues: new
            {
                //employeeId = employeeId,
                taskId = employeeTaskDto.taskId
            }, value: employeeTaskDto);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeTaskDto>> CreateNewTask([FromBody] EmployeeTaskAddDto employeeTask)
        {
            var entity = _mapper.Map<EmployeeTask>(employeeTask);
            _employeeTaskRepository.AddTask(entity);
            await _employeeTaskRepository.SaveAsync();

            var employeeTaskDto = _mapper.Map<EmployeeTaskDto>(entity);
            return CreatedAtRoute(nameof(GetTaskByTaskId), routeValues:
                new {
                    //这里必须要给employeeId一个空值，因为路径是api/employees/{employeeId}/employeetask， 
                    //ApiController会去找employeeId,如果没有空值路由的{employeeId}段会砍掉就找不到查看的url了

                    //后来我把ApiController这一层级的route改了，就不用加这个空employeeId了!!!
                    //employeeId = Guid.Empty,
                    taskId = employeeTaskDto.taskId
                }, value: employeeTaskDto);
        }

        //批量添加employeeTask
        [HttpPost("taskcollection")]
        public async Task<ActionResult<IEnumerable<EmployeeTaskDto>>> CreateTaskCollection(IEnumerable<EmployeeTaskAddDto> taskCollection)
        {
            if (taskCollection == null) return BadRequest();
            var taskEntities = _mapper.Map<IEnumerable<EmployeeTask>>(taskCollection);
            foreach (var taskEntity in taskEntities)
            {
                _employeeTaskRepository.AddTask(taskEntity);
            }
            await _employeeTaskRepository.SaveAsync();

            var TaskDtoReturn = _mapper.Map<IEnumerable<EmployeeTaskDto>>(taskEntities);
            //把插进去的数据组根据taskId查出来拼接在一起
            string ids = string.Join(",", TaskDtoReturn.Select(x => x.taskId));
            return CreatedAtRoute(nameof(GetTaskCollection), new { ids = ids }, TaskDtoReturn);
        }

        //批量查询，添加后显示的方法
        /// <summary>
        /// 绑定类型ArrayModelBinder是Helpers里自定义的，不是mvc的。 根据批量ID查task
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("taskcollection/{ids}", Name = nameof(GetTaskCollection))]
        public async Task<ActionResult<IEnumerable<EmployeeTaskDto>>> GetTaskCollection([ModelBinder(BinderType =typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null) return BadRequest();
            var taskEntities = await _employeeTaskRepository.GetTasksAsync(ids);
            if (taskEntities.Count() != ids.Count()) return NotFound(); 

            var taskDto = _mapper.Map<IEnumerable<EmployeeTaskDto>>(taskEntities);

            return Ok(taskDto);
        }

        //写一个带查询参数的查询,要注明来自query,默认来自body
        [HttpGet("search",Name = nameof(GetTasksBySearch))]
        public async Task<ActionResult<IEnumerable<EmployeeTaskDto>>> GetTasksBySearch([FromQuery] TaskDtoParameters parameters)
        {
            if (!_propertyMappingServiceForTask.ExistsMapping<EmployeeTaskDto, EmployeeTask>(parameters.OrderBy)) return BadRequest();

            if (parameters == null) return BadRequest();
            var employeeTasks = await _employeeTaskRepository.GetTasksAsync(parameters);
            //有前一页就创造前一页的uri,没有就为空
            //因为CreateTasksResponseUri用了Url.Link把datetime加密了，所以这里要解密一下，但是一般情况下，uri的datetime是不直接显示的，这里为了显示前后页的正确uri
            var previousLink = employeeTasks.HasPrevious ? HttpUtility.UrlDecode(CreateTasksResponseUri(parameters, ResourceUriType.PreviousPage)) : null;
            var nextLink = employeeTasks.HasNext ? HttpUtility.UrlDecode(CreateTasksResponseUri(parameters, ResourceUriType.NextPage)) : null;
            //创建一个页码匿名类,没有类名，只是为了使用里面的属性
            var paginationMetadata = new
            {
                totalCount = employeeTasks.TotalCount,
                pageSize = employeeTasks.PageSize,
                currentPage = employeeTasks.CurrentPage,
                totalPage = employeeTasks.TotalPages,
                previousLink,
                nextLink
            };
            //把匿名类的属性添加到header里，命名为X-Pagination
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata, new JsonSerializerOptions {
                //不安全的字符会被转义,这里改一下不让转义
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));
            var taskDto = _mapper.Map<IEnumerable<EmployeeTaskDto>>(employeeTasks);

            return Ok(taskDto);
        }

        //根据条件模糊查询，根据字段过滤,查询在指定StartTime之后的数据
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<EmployeeTaskDto>>> GetTasksByFilter([FromQuery(Name = "starttime")] DateTime StartTime, string query)
        {
            var tasks = await _employeeTaskRepository.GetTasksAsync(StartTime, query);
            var taskDto = _mapper.Map<IEnumerable<EmployeeTaskDto>>(tasks);

            return Ok(taskDto);
        }

        //局部更新task
        [HttpPatch("{taskId}")]
        public async Task<IActionResult> UpdateEmployeeTask([FromRoute] Guid taskId, JsonPatchDocument<EmployeeTaskUpdateDto> patchTasks)
        {
            var taskEntity = await _employeeTaskRepository.GetOneTaskAsync(taskId);
            //查出来的是EmployeeTask类型，要转换成EmployeeTaskUpdateDto
            var dtoToPatch = _mapper.Map<EmployeeTaskUpdateDto>(taskEntity);
            //用JsonPatchDocument转化一下Jason Patch操作,相当于把输入的patchTask转换成EmployeeTaskUpdateDto， ModelState返回false如果验证有错误，这里可以空着
            patchTasks.ApplyTo(dtoToPatch, ModelState);

            //处理验证
            if (!TryValidateModel(dtoToPatch))
            {
                return ValidationProblem(ModelState);
            };

            //这里要把输入对象转换成目标对象，因为要存进数据库，所以目标类型是EmployeeTask，目标就是taskEntity,CreateMap<EmployeeTaskUpdateDto, EmployeeTask>();
            _mapper.Map(dtoToPatch, taskEntity);
            //更新task的employeeId的时候要确保这个id确实存在，或者为空
            if (!await _employeeRepository.EmployeeExistAsync(taskEntity.EmployeeId) && taskEntity.EmployeeId != null) return NotFound();

            _employeeTaskRepository.UpdateTask(taskEntity);
            await _employeeTaskRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteEmployeeTask(Guid taskId)
        {
            var taskEntity = await _employeeTaskRepository.GetOneTaskAsync(taskId);
            if (taskEntity == null) return NotFound();

            _employeeTaskRepository.DeleteTask(taskEntity);
            await _employeeTaskRepository.SaveAsync();
            return NoContent();
        }

        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

        //这里用拼接uri，返回的header里能看到翻页前后的uri地址,属于GetTasksBySearch的响应
        private string CreateTasksResponseUri(TaskDtoParameters parameters, ResourceUriType type)
        {
            //PreviousPage and NextPage
            switch (type)
            {
                //前一页
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetTasksBySearch), new
                    {
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.SearchTerm,
                        deadline = parameters.Deadline,
                        orderBy = parameters.OrderBy
                    }); //Url.Link拼接翻页的uri, 这里一定要带上所有的查询过滤条件(TaskDtoParameters里的),否则生成的uri翻页后的数据就不对了

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetTasksBySearch), new
                    {
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.SearchTerm,
                        deadline = parameters.Deadline,
                        orderBy = parameters.OrderBy
                    }); //Url.Link拼接翻页的uri

                default:
                    return Url.Link(nameof(GetTasksBySearch), new
                    {
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        searchTerm = parameters.SearchTerm,
                        deadline = parameters.Deadline,
                        orderBy = parameters.OrderBy
                    }); //Url.Link拼接翻页的uri
            }
        }
    }
}
