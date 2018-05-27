using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project3.API.Models.Game;

namespace Project3.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/Games")]
    public class GameController : Controller
    {
        [HttpGet]
        [Route("Feed")]
        public async Task<ActionResult> Feed()
        {
            var queryString = HttpContext.Request.QueryString.ToString();
            NameValueCollection nvc = HttpUtility.ParseQueryString(queryString);

            var from = int.Parse(nvc["from"]); // 1
            var to = int.Parse(nvc["to"]); // 50
            var search = "";

            while (from < to + 1)
            {
                search += $"{from},";

                from++;
            }

            search = search.TrimEnd(',');

            var genres = await Utilities.API.Get<List<Game.GenreAPIModel>>($"genres/{search}/?fields=id,name&limit=50");

            foreach (var genre in genres)
            {
                var newGenre = Game.InsertGenre(genre);
            }

            return Json(genres);
        }


        [HttpGet]
        [Route("Search/{name}")]
        public async Task<ActionResult> SearchGame(string name)
        {
            const int limit = 10;
            var gamesApi = await Utilities.API.Get<List<Game.GameAPIModel>>($"games/?search={name}&fields=id,name,summary,genres,platforms,cover.url,release_dates&limit={limit}");
            var games = new List<Game.GameModel>();

            foreach (var game in gamesApi)
            {
                var newGame = new Game.GameModel
                {
                    Id = game.Id,
                    Name = game.Name,
                    Summary = game.Summary,
                    Cover = game.Cover == null ? "https://www.picclickimg.com/00/s/MTYwMFgxNjAw/z/8xgAAOSwr81USRqc/$/Nintendo-WII-DVD-Video-Game-Case-White-Blank-_57.jpg" : game.Cover.Url,
                    Genres = new List<Game.Genre>(),
                    Platforms = new List<Game.Platform>()
                };

                if (game.Genres != null)
                {
                    foreach (var genreId in game.Genres)
                    {
                        var genre = Game.GetGenre(genreId);

                        newGame.Genres.Add(JsonConvert.DeserializeObject<Game.Genre>(genre.ToString()));
                    }
                }

                if (game.Platforms != null)
                {
                    foreach (var platformId in game.Platforms)
                    {
                        var platform = Game.GetPlatform(platformId);

                        newGame.Platforms.Add(JsonConvert.DeserializeObject<Game.Platform>(platform.ToString()));
                    }
                }

                games.Add(newGame);
            }

            return Json(games);
        }

        [HttpPost]
        [Route("")]
        public ActionResult InsertGame([FromBody] Game.GameModel game)
        {
            try
            {
                var newGame = Game.InsertGame(game);

                return Created("/api/Games", newGame);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {message = ex.Message});
            }
        }
    }
}