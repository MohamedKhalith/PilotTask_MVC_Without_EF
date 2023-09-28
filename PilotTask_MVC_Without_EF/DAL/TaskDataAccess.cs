using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PilotTask_MVC_Without_EF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace PilotTask_MVC_Without_EF.DAL
{
    public class TaskDataAccess: ITaskDataAccess
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TaskDataAccess> _logger;
        public TaskDataAccess(IConfiguration configuration, ILogger<TaskDataAccess> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        /// <summary>
        /// CreateOrEditTask
        /// </summary>
        /// <param name="taskViewModel"></param>
        public void CreateOrEditTask(TaskViewModel taskViewModel)
        {
            try
            {
                _logger.LogDebug($"Started method CreateOrEditTask for Edit or Create the Task from Database with taskViewModel : {taskViewModel}");
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
                    _logger.LogDebug($"Ended method CreateOrEditTask for Edit or Create the Task from Database with taskViewModel : {taskViewModel}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error getting from CreateOrEditTask Method");
                throw;
            }
        }

        /// <summary>
        /// DeleteTaskById
        /// </summary>
        /// <param name="id"></param>
        public void DeleteTaskById(int? id)
        {
            try
            {
                _logger.LogDebug($"Started method DeleteTaskById for delete the Task from Database with Id : {id}");
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Pilot_Task_ConnectionString")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("TaskDeleteByID", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("Id", id);
                    sqlCmd.ExecuteNonQuery();
                    _logger.LogDebug($"Ended method DeleteTaskById for delete the Task from Database with Id : {id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error getting from DeleteTaskById Method");
                throw;
            }
        }

        /// <summary>
        /// FetchTaskByID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TaskViewModel</returns>
        public async Task<TaskViewModel> FetchTaskByID(int? id)
        {
            try
            {
                _logger.LogDebug($"Started method FetchTaskByID for getting the Task from Database with Id : {id}");
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
                        bookViewModel.Status = Convert.ToInt32(dtbl.Rows[0]["Status"].ToString());
                    }
                    _logger.LogDebug($"Ended method FetchTaskByID for getting the Task from Database with Id : {id}");
                    return await Task.FromResult(bookViewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error getting from FetchTaskByID Method");
                throw;
            }
        }

        /// <summary>
        /// GetAllTask
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TaskViewModel</returns>
        public async Task<List<TaskViewModel>> GetAllTask(int? id)
        {
            _logger.LogDebug($"Started method GetAllTask for getting the Task from Database with Id : {id}");
            List<TaskViewModel> taskViewModels = new List<TaskViewModel>();
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
                    foreach (DataRow dr in dtbl.Rows)
                    {
                        taskViewModels.Add(new TaskViewModel()
                        {
                            Id = Convert.ToInt32(dr["Id"].ToString()),
                            ProfileId = Convert.ToInt32(dr["ProfileId"].ToString()),
                            TaskName = dr["TaskName"].ToString(),
                            TaskDescription = dr["TaskDescription"].ToString(),
                            StartTime = Convert.ToDateTime(dr["StartTime"].ToString()),
                            Status = Convert.ToInt32(dr["Status"].ToString())
                        });
                    }
                }
                _logger.LogDebug($"Ended method GetAllTask for getting the Task from Database with Id : {id}");
                return await Task.FromResult(taskViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error getting from GetAllTask Method");
                throw;
            }
        }
    }
}
