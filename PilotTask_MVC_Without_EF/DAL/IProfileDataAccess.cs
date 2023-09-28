using PilotTask_MVC_Without_EF.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PilotTask_MVC_Without_EF.DAL
{
    public interface IProfileDataAccess
    {
        Task<List<ProfileViewModel>> GetAllProfile();
        void DeleteProfileById(int id);
        void CreateOrUpdate(ProfileViewModel profileViewModel);
        Task<ProfileViewModel> FetchBookByID(int? id);
    }
}
