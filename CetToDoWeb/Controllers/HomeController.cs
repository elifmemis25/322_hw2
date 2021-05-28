﻿using CetToDoWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CetToDoWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext, UserManager<CetUser> userManager)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            
            List<ToDoItem> result;
            if (User.Identity.IsAuthenticated)
            {
                var cetUser = await userManager.GetUserAsync(HttpContext.User);
                var query = dbContext.TodoItems // from todoItems
                    .Include(t => t.Category) //inner join categories c on t.categoryId = c.Id
                    .Where(t => t.CetUserId == cetUser.Id && !t.IsCompleted) // where isCompleted=0
                    .OrderBy(t => t.DueDate) // order by dueDate
                    .Take(3); // top 3
                              //select top 3 * from todoItems t inner join categories c on t.categoryId = c.Id
                              //where isCompleted=0 order by dueDate
                result = await query.ToListAsync();
            } else
            {
                result = new List<ToDoItem>();
            }
            
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
