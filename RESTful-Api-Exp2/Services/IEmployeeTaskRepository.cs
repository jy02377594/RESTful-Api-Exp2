using RESTful_Api_Exp2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful_Api_Exp2.Services
{
    public interface IEmployeeTaskRepository
    {
        //get all tasks' info
        Task<IEnumerable<EmployeeTask>> GetTasksAsync();
        Task<IEnumerable<EmployeeTask>> GetTasksAsync(string TaskList);
        Task<IEnumerable<EmployeeTask>> GetTasksAsync(Guid employeeId);
        // get a task by name
        Task<EmployeeTask> GetOneTaskAsync(string TaskName);
        Task<EmployeeTask> GetOneTaskAsync(Guid taskId);
        void AddTask(EmployeeTask task);
        void UpdateTask(EmployeeTask task);
        void DeleteTask(EmployeeTask task);

        Task<bool> SaveAsync();
    }
}
