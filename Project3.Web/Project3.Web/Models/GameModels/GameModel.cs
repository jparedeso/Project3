using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project3.Web.Models.GameModels
{
    public class GameModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Platform> Platforms { get; set; }
        public int ReleaseDate { get; set; }
        public string Cover { get; set; }
    }

    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        public int GameId { get; set; }
    }

    public class Platform
    {
        public int PlatformId { get; set; }
        public string PlatformName { get; set; }
        public int GameId { get; set; }
    }
}
