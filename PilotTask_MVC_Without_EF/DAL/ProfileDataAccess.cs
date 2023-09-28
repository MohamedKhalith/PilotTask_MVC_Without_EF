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
    public class ProfileDataAccess: IProfileDataAccess
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProfileDataAccess> _logger;
        public ProfileDataAccess(IConfiguration configuration, ILogger<ProfileDataAccess> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        /// <summary>
        /// CreateOrUpdate from the Database
        /// </summary>
        /// <param name="profileViewModel"></param>
        public void CreateOrUpdate(ProfileViewModel profileViewModel)
        {
            try
            {
                _logger.LogDebug($"Started method CreateOrUpdate for Edit or Create the Profile from Database with taskViewModel : {profileViewModel}");
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Pilot_Task_ConnectionString")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("ProfileAddOrEdit", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("ProfileId", profileViewModel.ProfileId);
                    sqlCmd.Parameters.AddWithValue("FirstName", profileViewModel.FirstName);
                    sqlCmd.Parameters.AddWithValue("LastName", profileViewModel.LastName);
                    sqlCmd.Parameters.AddWithValue("PhoneNumber", profileViewModel.PhoneNumber);
                    sqlCmd.Parameters.AddWithValue("EmailId", profileViewModel.EmailId);
                    sqlCmd.Parameters.AddWithValue("DateOfBirth", profileViewModel.DateOfBirth);
                    sqlCmd.ExecuteNonQuery();
                    _logger.LogDebug($"Ended method CreateOrUpdate for Edit or Create the Profile from Database with taskViewModel : {profileViewModel}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error getting from CreateOrUpdate Method");
                throw;
            }
        }
        /// <summary>
        /// DeleteProfileById from the Database
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProfileById(int id)
        {
            try
            {
                _logger.LogDebug($"Started method DeleteProfileById for Delete the Profile from Database with Id : {id}");
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Pilot_Task_ConnectionString")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("ProfileDeleteByID", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("ProfileId", id);
                    sqlCmd.ExecuteNonQuery();
                    _logger.LogDebug($"Ended method DeleteProfileById for Delete the Profile from Database with Id : {id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error getting from DeleteProfileById Method");
                throw;
            }
        }

        /// <summary>
        /// FetchBookByID from the Database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ProfileViewModel</returns>
        public async Task<ProfileViewModel> FetchBookByID(int? id)
        {
            try
            {
                _logger.LogDebug($"Started method FetchBookByID for getting the Profile from Database with Id : {id}");
                ProfileViewModel bookViewModel = new ProfileViewModel();
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Pilot_Task_ConnectionString")))
                {
                    DataTable dtbl = new DataTable();
                    sqlConnection.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("ProfileViewByID", sqlConnection);
                    sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqlDa.SelectCommand.Parameters.AddWithValue("ProfileId", id);
                    sqlDa.Fill(dtbl);
                    if (dtbl.Rows.Count == 1)
                    {
                        bookViewModel.ProfileId = Convert.ToInt32(dtbl.Rows[0]["ProfileId"].ToString());
                        bookViewModel.FirstName = dtbl.Rows[0]["FirstName"].ToString();
                        bookViewModel.LastName = dtbl.Rows[0]["LastName"].ToString();
                        bookViewModel.PhoneNumber = dtbl.Rows[0]["PhoneNumber"].ToString();
                        bookViewModel.EmailId = dtbl.Rows[0]["EmailId"].ToString();
                        bookViewModel.DateOfBirth = Convert.ToDateTime(dtbl.Rows[0]["DateOfBirth"].ToString());
                    }
                    _logger.LogDebug($"Ended method FetchBookByID for getting the Profile from Database with Id : {id}");
                    return await Task.FromResult(bookViewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error getting from FetchBookByID Method");
                throw;
            }
        }

        /// <summary>
        /// GetAllProfile from the Database
        /// </summary>
        /// <returns>ProfileViewModel</returns>
        public async Task<List<ProfileViewModel>> GetAllProfile()
        {
            _logger.LogDebug($"Started method GetAllProfile for getting all the Profile from Database");
            List<ProfileViewModel> profileViewModels = new List<ProfileViewModel>();
            try
            {
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Pilot_Task_ConnectionString")))
                {
                    sqlConnection.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("ProfileViewAll", sqlConnection);
                    sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqlDa.Fill(dtbl);
                    foreach(DataRow dr in dtbl.Rows)
                    {
                        profileViewModels.Add(new ProfileViewModel()
                        {
                            ProfileId = Convert.ToInt32(dr["ProfileId"].ToString()),
                            FirstName = dr["FirstName"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            PhoneNumber = dr["PhoneNumber"].ToString(),
                            EmailId = dr["EmailId"].ToString(),
                            DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"].ToString())
                        });
                    }
                }
                _logger.LogDebug($"Started method GetAllProfile for getting all the Profile from Database");
                return await Task.FromResult(profileViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error getting from GetAllProfile Method");
                throw;
            }
        } 
    }
}