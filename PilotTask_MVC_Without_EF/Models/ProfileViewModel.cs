using System;
using System.ComponentModel.DataAnnotations;

namespace PilotTask_MVC_Without_EF.Models
{
    public class ProfileViewModel
    {
        [Key]
        public int ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }
    }
}
