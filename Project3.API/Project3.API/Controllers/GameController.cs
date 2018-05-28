using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Claims;
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
        [Route("")]
        public JsonResult SearchUserGames()
        {
            var games = Game.GetGames(User.FindFirst(ClaimTypes.NameIdentifier).Value, null);

            return Json(games);
        }

        [HttpGet]
        [Route("Search/{name}")]
        public async Task<ActionResult> SearchGames(string name)
        {
            const int limit = 10;
            var gamesApi = await Utilities.API.Get<List<Game.GameAPIModel>>($"games/?search={name}&fields=id,name,summary,genres,platforms,cover.url,release_dates&limit={limit}");
            var games = new List<Game.GameModel>();

            var userGamesJObj = Game.GetGames(User.FindFirst(ClaimTypes.NameIdentifier).Value, null);
            var userGames = userGamesJObj.ToObject<List<Game.GameModel>>();

            foreach (var game in gamesApi)
            {
                var newGame = new Game.GameModel
                {
                    GameId = game.Id,
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

            games = games.Where(p => userGames.All(p2 => p2.GameId != p.GameId)).ToList();

            return Json(games);
        }

        [HttpGet]
        [Route("Info")]
        public ActionResult GameInfo()
        {
            var gamesObj = Game.GameInfo(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userGames = gamesObj.ToObject<List<Game.GameInfoModel>>();

            var games = userGames
                .GroupBy(g => new
                {
                    g.GameId
                })
                .Select(g => new Game.GameModel
                {
                    GameId = g.Key.GameId,
                    Name = g.First().Name,
                    Summary = g.First().Summary,
                    Cover = g.First().Cover == null ? "https://www.picclickimg.com/00/s/MTYwMFgxNjAw/z/8xgAAOSwr81USRqc/$/Nintendo-WII-DVD-Video-Game-Case-White-Blank-_57.jpg" : g.First().Cover,
                    Genres = new List<Game.Genre>(),
                    Platforms = new List<Game.Platform>()
                }).ToList();

            foreach (var game in games)
            {
                var genres = userGames
                    .Where(g => g.GameId == game.GameId)
                    .GroupBy(g => new
                    {
                        g.GenreId
                    })
                    .Select(g => new Game.Genre
                    {
                        GenreId = g.First().GenreId,
                        GenreName = g.First().GenreName,
                        GameId = g.First().GameId
                    }).ToList();

                var platforms = userGames
                    .Where(g => g.GameId == game.GameId)
                    .GroupBy(g => new
                    {
                        g.PlatformId
                    })
                    .Select(g => new Game.Platform
                    {
                        PlatformId = g.First().PlatformId,
                        PlatformName = g.First().PlatformName,
                        GameId = g.First().GameId
                    }).ToList();

                game.Genres = genres;
                game.Platforms = platforms;
            }

            return Json(games);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> InsertGame()
        {
            var queryString = HttpContext.Request.QueryString.ToString();
            NameValueCollection nvc = HttpUtility.ParseQueryString(queryString);
            var id = int.Parse(nvc["id"]);

            var dbGame = Game.GetGames(null, id);

            if (!dbGame.HasValues)
            {
                const int limit = 1;
                var apiGame = await Utilities.API.Get<List<Game.GameAPIModel>>($"games/{id}/?fields=id,name,genres,platforms,summary,cover.url,release_dates&limit={limit}");

                var game = new Game.GameModel
                {
                    GameId = apiGame[0].Id,
                    Name = apiGame[0].Name,
                    Summary = apiGame[0].Summary,
                    Cover = apiGame[0].Cover == null ? "https://www.picclickimg.com/00/s/MTYwMFgxNjAw/z/8xgAAOSwr81USRqc/$/Nintendo-WII-DVD-Video-Game-Case-White-Blank-_57.jpg" : apiGame[0].Cover.Url,
                    GenresStr = "",
                    PlatformsStr = ""
                };

                foreach (var genre in apiGame[0].Genres)
                {
                    game.GenresStr += $"{genre},";
                }

                foreach (var platform in apiGame[0].Platforms)
                {
                    game.PlatformsStr += $"{platform},";
                }

                try
                {
                    var newGame = Game.InsertGame(User.FindFirst(ClaimTypes.NameIdentifier).Value, game);

                    return Created("/api/Games", newGame);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = ex.Message });
                }
            }
            else
            {
                try
                {
                    var newGame = Game.InsertUserGame(User.FindFirst(ClaimTypes.NameIdentifier).Value, id);

                    return Created("/api/Games", newGame);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = ex.Message });
                }
            }
        }
    }
}