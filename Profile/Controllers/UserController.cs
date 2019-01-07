using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        public IActionResult UserList(int pageIndex = 1, int pageSize = 3,
            string searchFullname = "",
            OrderType orderType = OrderType.None,
            SortType sortType = SortType.None)
        {
            IEnumerable<User> model = _context.Users;

            //var modelCount = model.Count();
            //model = model.Skip((pageIndex - 1) * pageSize).Take(pageSize);

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
                SearchFullname = searchFullname,
            };

            return View(vm);
        }

        public IActionResult CreateUser()
        {
            var province = _context.provinces.Select(p => new SelectListItem()
            {
                Text = p.ProvinceName,
                Value = p.ProvinceId.ToString()
            }).ToList();

            //var city = _context.cities.Select(p => new SelectListItem()
            //{
            //    Text = p.CityName,
            //    Value = p.CityId.ToString()
            //}).ToList();

            ViewData["Province"] = new SelectList(province, "Value", "Text");
            //ViewData["Cities"] = new SelectList(city, "Value", "Text");

            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(User user, IFormFile Img)
        {
            user.TimeCreated = DateTime.Now;
            user.TimeEdit = DateTime.Now;

            // Upload Image
            if (Img != null || Img.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", Img.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Img.CopyTo(stream);
                }
            }

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

            user.Image = Img.FileName;
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
        public IActionResult EditUser(User user, IFormFile Img)
        {
            user.TimeEdit = DateTime.Now;

            // Upload Image
            if (Img == null || Img.Length == 0)
            {
                user.Image = user.Image;
            }
            else
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", Img.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    Img.CopyTo(stream);
                    user.Image = Img.FileName;
                }
            }

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

        public IActionResult CityAjax(int id)
        {
            var cities = _context.cities.Where(a => a.ProvinceId == id).ToList();

            //List<SelectListItem> city = new List<SelectListItem>();
            //foreach (var item in cities)
            //{
            //    city.Add(new SelectListItem()
            //    {
            //        Value = item.CityId.ToString(),
            //        Text = item.CityName
            //    });
            //}

            return Ok(cities);
        }
    }
}