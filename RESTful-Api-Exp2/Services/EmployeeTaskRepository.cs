using Microsoft.EntityFrameworkCore;
using RESTful_Api_Exp2.Data;
using RESTful_Api_Exp2.Entities;
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
            return await _context.EmployeeTasks.ToListAsync();
        }

        //get task info by tasklist
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

        public async Task<IEnumerable<EmployeeTask>> GetTasksAsync(Guid employeeId)
        {
            if (employeeId == Guid.Empty) throw new ArgumentNullException(nameof(employeeId));

            return await _context.EmployeeTasks
                .Where(x => x.EmployeeId == employeeId)
                .OrderBy(x => x.Employees)
                .ToListAsync();
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
