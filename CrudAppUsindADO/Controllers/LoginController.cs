﻿using CrudAppUsindADO.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrudAppUsindADO.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Valid()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Valid(Login model)
        {
            LoginDBContext context = new LoginDBContext();
            var user=context.GetUserByEmail(model.Email);
            if (user != null && user.Password == model.Password && user.Username==model.Username) 
            {
                ViewBag.Message = "Login successful!";
                return RedirectToAction("Index", "Home");


            }
            else
            {
                ViewBag.Message = "Login unsuccessful!";
            }
            
            return View();
        }
    }
}
