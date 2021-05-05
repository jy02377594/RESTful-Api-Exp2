using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
    //这里的业务逻辑，task是不应该完全依赖employee的，因为业务需求很可能先创建任务但是没有想好分配给谁，所以会创建一个空employeeId的task
    //因为employeeId可能会为空，所以这里的路由不应该先把employeeId加上，避免后面的逻辑复杂
    //[Route(template:"api/employees/{employeeId}/employeetask")]
    [Route("api/employeetask")]
    public class EmployeesTaskController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeTaskRepository _employeeTaskRepository;

        public EmployeesTaskController(IEmployeeRepository employeeRepository, IMapper mapper, IEmployeeTaskRepository employeeTaskRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _employeeTaskRepository = employeeTaskRepository ?? throw new ArgumentNullException(nameof(employeeTaskRepository));
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
            if (employeeId == Guid.Empty) return NotFound();
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
                    //后来我把ApiController这一层级的route改了，就不用加这个空employeeId了
                    //employeeId = Guid.Empty,
                    taskId = employeeTaskDto.taskId
                }, value: employeeTaskDto);
        }

        //下次写个批量添加employeeTask

        //局部更新task
        [HttpPatch("{taskId}")]
        public async Task<IActionResult> UpdateEmployeeTask([FromRoute]Guid taskId, JsonPatchDocument<EmployeeTaskUpdateDto> patchTasks)
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
            _employeeTaskRepository.UpdateTask(taskEntity);
            await _employeeTaskRepository.SaveAsync();

            return NoContent();
        }

        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
