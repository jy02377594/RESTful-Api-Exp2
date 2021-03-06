using RESTful_Api_Exp2.DtoParameters;
using RESTful_Api_Exp2.Entities;
using RESTful_Api_Exp2.Helpers;
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
        Task<IEnumerable<EmployeeTask>> GetTasksAsync(IEnumerable<Guid> taskIds);
        Task<IEnumerable<EmployeeTask>> GetTasksAsync(string TaskList);
        Task<IEnumerable<EmployeeTask>> GetTasksAsync(Guid employeeId);
        Task<PagedListForTask<EmployeeTask>> GetTasksAsync(TaskDtoParameters parameters);
        Task<IEnumerable<EmployeeTask>> GetTasksAsync(DateTime StartTime, string query);
        // get a task by name
        Task<EmployeeTask> GetOneTaskAsync(string TaskName);
        Task<EmployeeTask> GetOneTaskAsync(Guid taskId);
        void AddTask(EmployeeTask task);
        void AddTask(Guid employeeId, EmployeeTask task);
        void UpdateTask(EmployeeTask task);
        void DeleteTask(EmployeeTask task);

        Task<bool> SaveAsync();
    }
}
