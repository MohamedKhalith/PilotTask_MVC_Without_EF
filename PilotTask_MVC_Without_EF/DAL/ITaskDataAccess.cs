using PilotTask_MVC_Without_EF.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PilotTask_MVC_Without_EF.DAL
{
    public interface ITaskDataAccess
    {
        Task<List<TaskViewModel>> GetAllTask(int? id);
        void DeleteTaskById(int? id);
        void CreateOrEditTask(TaskViewModel taskViewModel);
        Task<TaskViewModel> FetchTaskByID(int? id);
    }
}
