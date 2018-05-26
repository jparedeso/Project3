using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project3.Web.Controllers
{
    public class ProfileController: Controller
    {
        [AllowAnonymous]
        public ActionResult Profile() => View();
    }
}
