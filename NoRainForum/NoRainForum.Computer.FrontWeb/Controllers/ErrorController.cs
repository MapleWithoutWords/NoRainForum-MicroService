﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NoRainForum.Computer.FrontWeb.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error404()
        {
            return View();
        }
        public IActionResult Error500()
        {
            return View();
        }
    }
}