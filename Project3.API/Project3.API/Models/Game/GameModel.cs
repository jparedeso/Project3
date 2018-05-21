using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Project3.API.Utilities;
using Newtonsoft.Json.Linq;

namespace Project3.API.Models.Game
{
    public class Game
    {
        public class GameModel
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


        public static JToken GetGames(NameValueCollection nvc)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            using (SqlCommand command = new SqlCommand("Game_Select", conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                //if (!string.IsNullOrEmpty(clientIdentifier))
                command.Parameters.AddWithValue("@GameID", nvc["GameID"]);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return JsonUtils.CreateJsonArrayFromSqlReader(reader);
                }
            }
        }
    }

}
