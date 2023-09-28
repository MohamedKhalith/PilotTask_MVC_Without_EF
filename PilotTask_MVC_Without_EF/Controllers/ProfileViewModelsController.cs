using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PilotTask_MVC_Without_EF.DAL;
using PilotTask_MVC_Without_EF.Models;

namespace PilotTask_MVC_Without_EF.Controllers
{
    public class ProfileViewModelsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProfileViewModelsController> _logger;
        private IProfileDataAccess _profileDataAccess;
        public ProfileViewModelsController(IConfiguration configuration, ILogger<ProfileViewModelsController> logger, IProfileDataAccess profileDataAccess)
        {
            _configuration = configuration;
            _logger = logger;
            _profileDataAccess = profileDataAccess;
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            try
            {
                _logger.LogDebug($"Started method for Getting all Profile from Database");
                var response = await _profileDataAccess.GetAllProfile();
                _logger.LogDebug($"Started method for Getting all Profile from Database");
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
        public async Task<IActionResult> AddOrEdit(int? id)
        {
            try
            {
                _logger.LogDebug($"Started method for Getting specific Profile by Id : {id}");
                ProfileViewModel profileViewModel = new ProfileViewModel();
                if (id > 0)
                    profileViewModel =await _profileDataAccess.FetchBookByID(id);
                _logger.LogDebug($"Ended method for Getting specific Profile by Id : {id}");
                return View(profileViewModel);
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
        /// <param name="profileViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("ProfileId,FirstName,LastName,DateOfBirth,PhoneNumber,EmailId")] ProfileViewModel profileViewModel)
        {
            try
            {
                _logger.LogDebug($"Started method for Create or edit Profile by Request body : {profileViewModel}");
                if (ModelState.IsValid)
                {
                    _profileDataAccess.CreateOrUpdate(profileViewModel);
                    _logger.LogDebug($"Ended method for Create or edit Profile by Request body : {profileViewModel}");
                    return RedirectToAction(nameof(Index));
                }
                return View(profileViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error getting from AddOrEdit Method");
                throw;
            }
        }

        // GET: ProfileViewModels/Delete/5
        public IActionResult Delete(int? id)
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
                _logger.LogDebug($"Started method for Delete Profile by Request body : {id}");
                _profileDataAccess.DeleteProfileById(id);
                _logger.LogDebug($"Ended method for Delete Profile by Request body : {id}");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception error getting from DeleteConfirmed Method");
                throw;
            }
        }
    }
}
