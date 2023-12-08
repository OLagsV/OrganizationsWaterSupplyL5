﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrganizationsWaterSupplyL4.Data;

namespace OrganizationsWaterSupplyL4.Controllers
{
    public class AdminController : Controller
    {
        UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> manager)
        {
            _userManager = manager;
        }
        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }
    }
}
