using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductsValidation.Models;
using ProductsValidation.Services;

namespace ProductsValidation.Controllers
{
    public class UsersController : Controller
    {
        private readonly List<User> users;

        public UsersController(Data data)
        {
            users = data.Users;
        }

        public IActionResult Index()
        {
            return View("Index", users);
        }

        // GET /Users/Create?Id=5&Name=Ann&Email=ann@gmail.com&Role=user
        // The form only shows the query string values (all fields are readonly).
        [HttpGet]
        public IActionResult Create([FromQuery] User user)
        {
            return View(user);
        }

        // The form posts back to the same URL (with the same query string),
        // and the user is bound ONLY from the query string — form fields are ignored.
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost([FromQuery] User user)
        {
            users.Add(user);
            return View("View", user);
        }
    }
}
