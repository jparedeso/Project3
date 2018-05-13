using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project3.API.Models.Games
{
    public class GameType
    {
        public int id { get; set; }
        public string name { get; set; }
        public Logo logo { get; set; }
        public string url { get; set; }
    }

    public class Logo
    {
        public string url { get; set; }
    }
}
