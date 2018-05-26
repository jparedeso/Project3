using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Project3.Web.Models.GameModels;

namespace Project3.Web.Controllers
{
    public class ForumController : Controller
    {
        public ActionResult Forum()
        {
            var subForum = new List<PlatformModel>
            {
                new PlatformModel
                {
                    PlatformId = 1,
                    Name = "Atari 2600"
                },
                new PlatformModel
                {
                    PlatformId = 2,
                    Name = "NES"
                },
                new PlatformModel
                {
                    PlatformId = 3,
                    Name = "Sega Genesis"
                },
            };


            return View(subForum);
        }
    }
}