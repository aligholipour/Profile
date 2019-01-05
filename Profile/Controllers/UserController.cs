using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Profile.Models;
using Profile.Models.Entities;
using Profile.Models.Enums;
using Profile.Models.ViewModels;

namespace Profile.Controllers
{
    public class UserController : Controller
    {
        private readonly ProjectDbContext _context;

        public UserController(ProjectDbContext context)
        {
            _context = context;
        }

        public IActionResult UserList(string searchFullname = "", OrderType orderType = OrderType.None, SortType sortType = SortType.None)
        {
            IEnumerable<User> model = _context.Users;


            if (string.IsNullOrEmpty(searchFullname))
            {
                model = _context.Users;
            }
            else
            {
                model = model
                    .Where(u => u.FirstName.Contains(searchFullname) || u.LastName.Contains(searchFullname));
            }


            switch (sortType)
            {
                case SortType.None:
                    break;

                case SortType.Age:
                    model = model.OrderBy(u => u.Age);
                    break;

                case SortType.Name:
                    model = model.OrderBy(u => u.FirstName);
                    break;

                default:
                    break;
            }

            switch (orderType)
            {
                case OrderType.None:
                    break;

                case OrderType.Up:
                    break;

                case OrderType.Down:
                    model = model.Reverse();
                    break;

                default:
                    break;
            }

            ViewData["Search"] = searchFullname;

            var vm = new UserViewModel()
            {
                Users = model.ToList(),
                OrderType = orderType,
                SortType = sortType,
                SearchFullname = searchFullname
            };

            return View(vm);
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            user.TimeCreated = DateTime.Now;
            user.TimeEdit = DateTime.Now;
            switch (user.UserState)
            {
                case "1":
                    user.UserState = "Active";
                    break;
                case "2":
                    user.UserState = "Deactive";
                    break;
                case "3":
                    user.UserState = "Blocked";
                    break;
            }
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("UserList");
        }

        public IActionResult EditUser(int id)
        {
            var findUser = _context.Users.Find(id);
            return View(findUser);
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            user.TimeEdit = DateTime.Now;
            switch (user.UserState)
            {
                case "1":
                    user.UserState = "Active";
                    break;
                case "2":
                    user.UserState = "Deactive";
                    break;
                case "3":
                    user.UserState = "Blocked";
                    break;
            }
            _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("UserList");
        }

        public IActionResult DeleteUser(int id)
        {
            var findUser = _context.Users.Find(id);
            _context.Entry(findUser).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("UserList");
        }

        public IActionResult Profile(int id)
        {
            var findUser = _context.Users.Find(id);
            return View(findUser);
        }

        public IActionResult CityAjax(string province)
        {
            string model = "";

            switch (province)
            {
                case "Golestan":
                    model = "Gorgan";
                    break;

                case "Mazandaran":
                    model = "Sari";
                    break;

                case "Gilan":
                    model = "Rasht";
                    break;

                default:
                    break;
            }
            return Json(model);
        }
    }
}