using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Profile.Models;
using Profile.Models.Entities;

namespace Profile.Controllers
{
    public class UserController : Controller
    {
        private readonly ProjectDbContext _context;

        public UserController(ProjectDbContext context)
        {
            _context = context;
        }

        public IActionResult UserList()
        {
            return View(_context.Users.ToList());
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
    }
}