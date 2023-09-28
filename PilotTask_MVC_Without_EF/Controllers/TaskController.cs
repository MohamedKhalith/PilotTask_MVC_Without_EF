using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PilotTask_MVC_Without_EF.DAL;
using PilotTask_MVC_Without_EF.Models;
using System;
using System.Threading.Tasks;

namespace PilotTask_MVC_Without_EF.Controllers
{
    public class TaskController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TaskController> _logger;
        private ITaskDataAccess _taskDataAccess;
        public TaskController(IConfiguration configuration, ILogger<TaskController> logger, ITaskDataAccess taskDataAccess)
        {
            _configuration = configuration;
            _logger = logger;
            _taskDataAccess = taskDataAccess;
        }
        /// <summary>
        /// Index
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Index(int? id)
        {
            try
            {
                _logger.LogDebug($"Started method for Getting all Task from Database with Id : {id}");
                var response = await _taskDataAccess.GetAllTask(id);
                _logger.LogDebug($"Ended method for Getting all Task from Database with Id : {id}");
                return View(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error getting from Index Method");
                throw;
            }
        }

        /// <summary>
        /// AddOrEdit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> AddOrEdit(int id)
        {
            try
            {
                _logger.LogDebug($"Started method for Getting the specific Task with Id : {id}");
                TaskViewModel taskViewModel = new TaskViewModel();
                if (id > 0)
                    taskViewModel = await _taskDataAccess.FetchTaskByID(id);
                _logger.LogDebug($"Ended method for Getting the specific Task with Id : {id}");
                return View(taskViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error getting from AddOrEdit Method");
                throw;
            }
        }


        /// <summary>
        /// AddOrEdit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(int id, [Bind("Id,ProfileId,TaskName,TaskDescription,StartTime,Status")] TaskViewModel taskViewModel)
        {
            try
            {
                _logger.LogDebug($"Started method for Create or edit the specific Task with Id : {id}");
                if (ModelState.IsValid)
                {
                    _taskDataAccess.CreateOrEditTask(taskViewModel);
                    _logger.LogDebug($"Ended method for Create or edit the specific Task with Id : {id}");
                    return RedirectToAction(nameof(Index), new { id = taskViewModel.ProfileId });
                }
                return View(taskViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error getting from AddOrEdit Method");
                throw;
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            return View();
        }

        /// <summary>
        /// DeleteConfirmed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogDebug($"Started method for Delete specific Task with Id : {id}");
                _taskDataAccess.DeleteTaskById(id);
                _logger.LogDebug($"Ended method for Delete specific Task with Id : {id}");
                return RedirectToAction(nameof(Index), "ProfileViewModels");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error getting from DeleteConfirmed Method");
                throw;
            }
        }
    }
}
