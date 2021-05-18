using Microsoft.EntityFrameworkCore;
using RESTful_Api_Exp2.Data;
using RESTful_Api_Exp2.DtoParameters;
using RESTful_Api_Exp2.Entities;
using RESTful_Api_Exp2.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Services
{
    public class EmployeeTaskRepository: IEmployeeTaskRepository
    {
        private readonly Restful_DbContext _context;

        public EmployeeTaskRepository(Restful_DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        //get all tasks
        public async Task<IEnumerable<EmployeeTask>> GetTasksAsync()
        {
            //var query = _context.Set<EmployeeTask>("select * from EmployeeTask");
            //return await query.ToListAsync();
            return await _context.EmployeeTasks.ToListAsync();
        }

        //get task info by tasklist, task name list
        public async Task<IEnumerable<EmployeeTask>> GetTasksAsync(string TaskList)
        {
            if (TaskList == null || TaskList.Trim() == null)
            {
                throw new ArgumentNullException(nameof(TaskList));
            }

            return await _context.EmployeeTasks
                .Where(x => TaskList.Contains(x.TaskName))
                .OrderBy(x => x.Employees)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeTask>> GetTasksAsync(IEnumerable<Guid> taskIds)
        {
            if (taskIds == null) throw new ArgumentNullException(nameof(taskIds));

            return await _context.EmployeeTasks
                .Where(x => taskIds.Contains(x.taskId))
                .OrderBy(x => x.TaskName)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeTask>> GetTasksAsync(Guid employeeId)
        {
            if (employeeId == Guid.Empty) throw new ArgumentNullException(nameof(employeeId));

            return await _context.EmployeeTasks
                .Where(x => x.EmployeeId == employeeId)
                .OrderBy(x => x.Employees)
                .ToListAsync();
        }
        //fuzzy query by parameters
        public async Task<PagedListForTask<EmployeeTask>> GetTasksAsync(TaskDtoParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            var queryExpression = _context.EmployeeTasks as IQueryable<EmployeeTask>;

            //判断datetime类型为空就看它和最小值相等与否，最小值是0001-01-01
            if (string.IsNullOrWhiteSpace(parameters.SearchTerm) && parameters.Deadline == DateTime.MinValue)
                return await PagedListForTask<EmployeeTask>.CreateAsnyc(queryExpression, parameters.PageNumber, parameters.PageSize);

            //这里不能转成IEnumerable因为后面不能转成异步list,而IQueryable相当于生成不马上执行的sql查询语句，可以转成异步list
            //var queryExpression = _context.EmployeeTasks as IEnumerable<EmployeeTask>;
            if (parameters.Deadline != DateTime.MinValue)
            {
                parameters.Deadline = DateTime.Parse(parameters.Deadline.ToString().Trim());
                queryExpression = queryExpression.Where(x => x.Deadline == parameters.Deadline);
            }

            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                parameters.SearchTerm = parameters.SearchTerm.Trim();
                queryExpression = queryExpression.Where(x => x.TaskName.Contains(parameters.SearchTerm) || x.TaskDescription.Contains(parameters.SearchTerm));
            }

            return await PagedListForTask<EmployeeTask>.CreateAsnyc(queryExpression, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<IEnumerable<EmployeeTask>> GetTasksAsync(DateTime StartTime, string query)
        {
            if (StartTime == DateTime.MinValue && string.IsNullOrWhiteSpace(query)) 
                return await _context.EmployeeTasks.ToListAsync();

            var items = _context.EmployeeTasks as IQueryable<EmployeeTask>;
            //after starttime
            if (StartTime != DateTime.MinValue)
            {
                items = _context.EmployeeTasks.Where(x => x.StartTime >= StartTime);
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                items = items.Where(x => x.TaskName.Contains(query) || x.TaskDescription.Contains(query));
            }

            return await items.OrderBy(x => x.EmployeeId).ToListAsync();
        }

        public async Task<EmployeeTask> GetOneTaskAsync(Guid taskId)
        {
            if (taskId == Guid.Empty) throw new ArgumentNullException(nameof(taskId));

            return await _context.EmployeeTasks
                .FirstOrDefaultAsync(x => x.taskId == taskId);
        }
        public async Task<EmployeeTask> GetOneTaskAsync(string TaskName)
        {
            if (TaskName == null)
            {
                throw new ArgumentNullException(nameof(TaskName));
            }

            return await _context.EmployeeTasks.FirstOrDefaultAsync(x => x.TaskName == TaskName);
        }

        public void AddTask(Guid employeeId, EmployeeTask task)
        {
            if (employeeId == Guid.Empty) throw new ArgumentNullException(nameof(employeeId));
            if (task == null) throw new ArgumentNullException(nameof(task));

            task.EmployeeId = employeeId;
            _context.EmployeeTasks.Add(task);
        }
        public void AddTask(EmployeeTask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }
            task.EmployeeId = null;

            _context.EmployeeTasks.Add(task);
        }
        public void UpdateTask(EmployeeTask task)
        {
            // the reason why I can comment this line of core is EF Core realtimely trace/monitor entity。
            _context.Entry(task).State = EntityState.Modified;
        }
        public void DeleteTask(EmployeeTask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            _context.EmployeeTasks.Remove(task);
        }

        public async Task<bool> SaveAsync()
        {
            return (await _context.SaveChangesAsync()) >= 0;
        }
    }
}
