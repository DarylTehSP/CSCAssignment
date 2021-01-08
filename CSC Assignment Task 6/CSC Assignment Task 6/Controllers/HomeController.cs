using CSC_Assignment_Task_6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CSC_Assignment_Task_6.Controllers
{
    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }

        
    }
}
