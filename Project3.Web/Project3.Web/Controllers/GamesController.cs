using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project3.Web.Models;
using Project3.Web.Models.GameModels;

namespace Project3.Web.Controllers
{
    public class GamesController : Controller
    {
        private readonly Utilities.Utilities _utilities;
        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GamesController(AppSettings appSettings, IHttpContextAccessor httpContextAccessor)
        {
            _appSettings = appSettings;
            _httpContextAccessor = httpContextAccessor;
            _utilities = new Utilities.Utilities(appSettings, httpContextAccessor);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> SearchUserGames()
        {
            var games = await Utilities.API.Get<List<GameModel>>(_appSettings, _httpContextAccessor, "Games");

            return Json(games);
        }

        [Route("Search")]
        public async Task<ActionResult> SearchGame()
        {
            var queryString = HttpContext.Request.QueryString.ToString();
            NameValueCollection nvc = HttpUtility.ParseQueryString(queryString);
            var name = nvc["name"];

            var games = await Utilities.API.Get<List<GameModel>>(_appSettings, _httpContextAccessor, $"Games/Search/{name}");

            return Json(games);
        }

        [HttpPost]
        public async Task<ActionResult> InsertGame([FromForm] int id)
        {
            try
            {
                var response = await Utilities.API.Post(_appSettings, _httpContextAccessor, $"Games?id={id}", new {id});

                if (response.StatusCode != HttpStatusCode.Created)
                {
                    return BadRequest(response.ReasonPhrase);
                }

                var content = await response.Content.ReadAsStringAsync();
                var newGame = JsonConvert.DeserializeObject<GameModel>(content);

                return Created("/api/Games", newGame);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}