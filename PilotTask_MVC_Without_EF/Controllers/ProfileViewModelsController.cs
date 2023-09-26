using System;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PilotTask_MVC_Without_EF.Models;

namespace PilotTask_MVC_Without_EF.Controllers
{
    public class ProfileViewModelsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProfileViewModelsController> _logger;
        public ProfileViewModelsController(IConfiguration configuration, ILogger<ProfileViewModelsController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        // GET: ProfileViewModels
        public IActionResult Index()
        {
            try
            {
                DataTable dtbl = new DataTable();
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Pilot_Task_ConnectionString")))
                {
                    sqlConnection.Open();
                    SqlDataAdapter sqlDa = new SqlDataAdapter("ProfileViewAll", sqlConnection);
                    sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqlDa.Fill(dtbl);
                }
                return View(dtbl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error");
                throw;
            }
        }

        // GET: ProfileViewModels/AddOrEdit/5
        public IActionResult AddOrEdit(int? id)
        {
            try
            {
                ProfileViewModel profileViewModel = new ProfileViewModel();
                if (id > 0)
                    profileViewModel = FetchBookByID(id);
                return View(profileViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error");
                throw;
            }

        }

        // POST: ProfileViewModels/AddOrEdit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("ProfileId,FirstName,LastName,DateOfBirth,PhoneNumber,EmailId")] ProfileViewModel profileViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
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
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(profileViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error");
                throw;
            }
        }

        // GET: ProfileViewModels/Delete/5
        public IActionResult Delete(int? id)
        {
            return View();
        }

        // POST: ProfileViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  IActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("Pilot_Task_ConnectionString")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("ProfileDeleteByID", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("ProfileId", id);
                    sqlCmd.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error");
                throw;
            }
        }
        [NonAction]
        public ProfileViewModel FetchBookByID(int? id)
        {
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
                return bookViewModel;
            }
        }
    }
}
