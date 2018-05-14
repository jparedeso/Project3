using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project3.API.Models.Game
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> Genres { get; set; }
        public List<int> Platforms { get; set; }
        public List<ReleaseDate> Release_dates { get; set; }
        public Cover Cover { get; set; }
    }

    public class ReleaseDate
    {
        public int Category { get; set; }
        public int Platform { get; set; }
        public long Date { get; set; }
        public string Human { get; set; }
        public int Y { get; set; }
        public int M { get; set; }
    }

    public class Cover
    {
        public string Url { get; set; }
    }

}
