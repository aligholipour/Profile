using Microsoft.AspNetCore.Http;
using Profile.Models.Entities;
using Profile.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Profile.Models.ViewModels
{
    public class UserViewModel
    {
        public IList<User> Users { get; set; }
        public OrderType OrderType { get; set; }
        public SortType SortType { get; set; }
        public string SearchFullname { get; set; }
    }
}
