using Microsoft.EntityFrameworkCore;

namespace PilotTask_MVC_Without_EF.Data
{
    public class PilotTask_MVC_Without_EFContext : DbContext
    {
        public PilotTask_MVC_Without_EFContext (DbContextOptions<PilotTask_MVC_Without_EFContext> options)
            : base(options)
        {
        }

        public DbSet<PilotTask_MVC_Without_EF.Models.ProfileViewModel> ProfileViewModel { get; set; }
        public DbSet<PilotTask_MVC_Without_EF.Models.TaskViewModel> TaskViewModel { get; set; }
    }
}
