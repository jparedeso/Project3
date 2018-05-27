using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project3.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Game")]
    public class GameController : Controller
    {

    }
}