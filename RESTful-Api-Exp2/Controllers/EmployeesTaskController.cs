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
    [Route(template:"api/employees/{employeeId}/employeetask")]
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

        
        [HttpGet(template:"alltasks")]
        public async Task<ActionResult<IEnumerable<EmployeeTaskDto>>> GetAllEmployeesTasks()
        {
            var employeesTasks = await _employeeTaskRepository.GetTasksAsync();
            var employeeTaskDtos = _mapper.Map<IEnumerable<EmployeeTaskDto>>(employeesTasks);
            return Ok(employeeTaskDtos);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeTaskDto>>> GetTasksByEmployeeId(Guid employeeId)
        {
            if (!await _employeeRepository.EmployeeExistAsync(employeeId)) return NotFound();

            var employeeTasks = await _employeeTaskRepository.GetTasksAsync(employeeId);
            var employeeTasksDtos = _mapper.Map<IEnumerable<EmployeeTaskDto>>(employeeTasks);

            return Ok(employeeTasksDtos);
        }

        //这里路由加TaskName因为会获取employeeId进入api/employees/{employeeId}/employeetask，会导致multiple endpoints和上面的GetTasksByEmployeeId方法冲突。
        [HttpGet(template:"TaskName")]
        public async Task<ActionResult<EmployeeTaskDto>> GetTaskByTaskName([FromQuery(Name = "TaskName")]string TaskName)
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

        [HttpGet(template: "TaskList")]
        public async Task<ActionResult<IEnumerable<EmployeeTaskDto>>> GetTaskByTaskList([FromQuery(Name = "TaskList")] string TaskList)
        {
            if (String.IsNullOrWhiteSpace(TaskList)) return NotFound();

            var employeeTasks = await _employeeTaskRepository.GetTasksAsync(TaskList);
            if (employeeTasks == null) return NotFound();
            var employeeTaskDtos = _mapper.Map<IEnumerable<EmployeeTaskDto>>(employeeTasks);

            return Ok(employeeTaskDtos);
        }

        [HttpPost(template:"CreateTaskWithEmployeeId")]
        public async Task<ActionResult<EmployeeTaskDto>> CreateNewTaskForEmployee(Guid employeeId, [FromBody]EmployeeTaskAddDto employeeTask)
        {
            if (employeeId == Guid.Empty) return NotFound();
            var entity = _mapper.Map<EmployeeTask>(employeeTask);

            _employeeTaskRepository.AddTask(employeeId,entity);
            await _employeeTaskRepository.SaveAsync();

            var employeeTaskDto = _mapper.Map<EmployeeTaskDto>(entity);

            return CreatedAtRoute(nameof(GetTaskByTaskId), routeValues: new
            {
                employeeId = employeeId,
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
                    employeeId = Guid.Empty,
                    taskId = employeeTaskDto.taskId 
                },value: employeeTaskDto);
        }
    }
}
