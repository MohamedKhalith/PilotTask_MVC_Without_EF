using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PilotTask_MVC_Without_EF.Models;
using System;
using System.Data;

namespace PilotTask_MVC_Without_EF.Controllers
{
    public class TaskController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TaskController> _logger;
        public TaskController(IConfiguration configuration, ILogger<TaskController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        // GET: TaskController
        public ActionResult Index(int? id)
        {
            try
            {
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Pilot_Task_ConnectionString")))
                {
                    if (id != null)
                    {
                        sqlConnection.Open();
                        SqlDataAdapter sqlDa = new SqlDataAdapter("TaskViewAllById", sqlConnection);
                        sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                        sqlDa.SelectCommand.Parameters.AddWithValue("ProfileId", id);
                        sqlDa.Fill(dtbl);
                    }
                    else
                    {
                        sqlConnection.Open();
                        SqlDataAdapter sqlDa = new SqlDataAdapter("TaskViewAll", sqlConnection);
                        sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                        sqlDa.Fill(dtbl);
                    }
                }
                return View(dtbl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error");
                throw;
            }
        }

        // GET: TaskController/Edit/5
        public ActionResult AddOrEdit(int id)
        {
            try
            {
                TaskViewModel taskViewModel = new TaskViewModel();
                if (id > 0)
                    taskViewModel = FetchTaskByID(id);
                return View(taskViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error");
                throw;
            }
        }


        // POST: TaskController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddOrEdit(int id, [Bind("Id,ProfileId,TaskName,TaskDescription,StartTime,Status")] TaskViewModel taskViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Pilot_Task_ConnectionString")))
                    {
                        sqlConnection.Open();
                        SqlCommand sqlCmd = new SqlCommand("[TaskAddOrEdit]", sqlConnection);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("Id", taskViewModel.Id);
                        sqlCmd.Parameters.AddWithValue("ProfileId", taskViewModel.ProfileId);
                        sqlCmd.Parameters.AddWithValue("TaskName", taskViewModel.TaskName);
                        sqlCmd.Parameters.AddWithValue("TaskDescription", taskViewModel.TaskDescription);
                        sqlCmd.Parameters.AddWithValue("StartTime", taskViewModel.StartTime);
                        sqlCmd.Parameters.AddWithValue("Status", taskViewModel.Status);
                        sqlCmd.ExecuteNonQuery();
                    }
                    return RedirectToAction(nameof(Index), new { id = taskViewModel.ProfileId });
                }
                return View(taskViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error");
                throw;
            }
        }

        // GET: TaskController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TaskController/Delete/5
        // POST: ProfileViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Pilot_Task_ConnectionString")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("TaskDeleteByID", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("Id", id);
                    sqlCmd.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index), "ProfileViewModels");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error");
                throw;
            }
        }
        [NonAction]
        public TaskViewModel FetchTaskByID(int? id)
        {
            TaskViewModel bookViewModel = new TaskViewModel();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Pilot_Task_ConnectionString")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("TaskViewByID", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("Id", id);
                sqlDa.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {
                    bookViewModel.Id = Convert.ToInt32(dtbl.Rows[0]["Id"].ToString());
                    bookViewModel.ProfileId = Convert.ToInt32(dtbl.Rows[0]["ProfileId"].ToString());
                    bookViewModel.TaskName = dtbl.Rows[0]["TaskName"].ToString();
                    bookViewModel.TaskDescription = dtbl.Rows[0]["TaskDescription"].ToString();
                    bookViewModel.StartTime = Convert.ToDateTime(dtbl.Rows[0]["StartTime"].ToString());
                    bookViewModel.Status= Convert.ToInt32(dtbl.Rows[0]["Status"].ToString());
                }
                return bookViewModel;
            }
        }
    }
}
