using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Profile.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public GenderType Gender { get; set; }
        public DateTime TimeCreated { get; set; }
        public string NationalCode { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Mobile { get; set; }
        public DateTime TimeEdit { get; set; }
        public string UserState { get; set; }
    }

    public enum GenderType
    {
        None = 0,
        [Display(Name = "مرد")]
        Male = 1,
        [Display(Name = "زن")]
        Female = 2
    }

    public enum UserStateType
    {
        Active = 0,
        Deactive = 1,
        Block = 2
    }
}
