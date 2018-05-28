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
            public int GameId { get; set; }
            public string Name { get; set; }
            public string Summary { get; set; }
            public string GenresStr { get; set; }
            public List<Genre> Genres { get; set; }
            public string PlatformsStr { get; set; }
            public List<Platform> Platforms { get; set; }
            public int ReleaseDate { get; set; }
            public string Cover { get; set; }
        }

        public class GameAPIModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Summary { get; set; }
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

        public class Logo
        {
            public string url { get; set; }
        }


        public class PlatformAPIModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Logo Logo { get; set; }
        }

        public class Platform
        {
            public int PlatformId { get; set; }
            public string PlatformName { get; set; }
            public int GameId { get; set; }
        }

        public class GenreAPIModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Genre
        {
            public int GenreId { get; set; }
            public string GenreName { get; set; }
            public int GameId { get; set; }
        }

        public class GameInfoModel
        {
            public int GameId { get; set; }
            public string Name { get; set; }
            public string Summary { get; set; }
            public string Cover { get; set; }
            public int GenreId { get; set; }
            public string GenreName { get; set; }
            public int PlatformId { get; set; }
            public string PlatformName { get; set; }
        }

        public static JToken GameInfo(string username)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            using (SqlCommand command = new SqlCommand("GameInfo", conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Username", username);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return JsonUtils.CreateJsonArrayFromSqlReader(reader);
                }
            }
        }

        public static JToken GetGames(string username, int? gameId)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            using (SqlCommand command = new SqlCommand("Game_Select", conn))
            {
                command.CommandType = CommandType.StoredProcedure;
                
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@GameId", gameId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return JsonUtils.CreateJsonArrayFromSqlReader(reader);
                }
            }
        }

        public static JToken InsertGame(string username, GameModel game)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            using (SqlCommand command = new SqlCommand("AddGame", conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@GameID", game.GameId);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@GameName", game.Name);
                command.Parameters.AddWithValue("@Summary", game.Summary);
                command.Parameters.AddWithValue("@Genres", game.GenresStr);
                command.Parameters.AddWithValue("@Platforms", game.PlatformsStr);
                command.Parameters.AddWithValue("@Cover", game.Cover);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return JsonUtils.CreateJsonFromSqlReader(reader);
                }
            }
        }

        public static JToken InsertUserGame(string username, int id)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            using (SqlCommand command = new SqlCommand("AddUserGame", conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@GameID", id);
                command.Parameters.AddWithValue("@Username", username);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return JsonUtils.CreateJsonFromSqlReader(reader);
                }
            }
        }

        public static JToken DeleteGame(string username, int gameId)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            using (SqlCommand command = new SqlCommand("DeleteGame", conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@GameID", gameId);
                command.Parameters.AddWithValue("@Username", username);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return JsonUtils.CreateJsonFromSqlReader(reader);
                }
            }
        }

        public static JToken GetPlatform(int platformId)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            using (SqlCommand command = new SqlCommand("Platforms_Select", conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@PlatformId", platformId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return JsonUtils.CreateJsonFromSqlReader(reader);
                }
            }
        }

        public static JToken InsertPlatform(PlatformAPIModel platform)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            using (SqlCommand command = new SqlCommand("AddPlatform", conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@PlatformId", platform.Id);
                command.Parameters.AddWithValue("@PlatformName", platform.Name);
                command.Parameters.AddWithValue("@Logo", platform.Logo == null ? "https://www.picclickimg.com/00/s/MTYwMFgxNjAw/z/8xgAAOSwr81USRqc/$/Nintendo-WII-DVD-Video-Game-Case-White-Blank-_57.jpg" : platform.Logo.url);
                //command.Parameters.AddWithValue("@GameId", platform.Cover);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return JsonUtils.CreateJsonFromSqlReader(reader);
                }
            }
        }

        public static JToken GetGenre(int genreId)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            using (SqlCommand command = new SqlCommand("Genres_Select", conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@GenreId", genreId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return JsonUtils.CreateJsonFromSqlReader(reader);
                }
            }
        }

        public static JToken InsertGenre(GenreAPIModel genre)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            using (SqlCommand command = new SqlCommand("AddGenre", conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@GenreId", genre.Id);
                command.Parameters.AddWithValue("@GenreName", genre.Name);
                //command.Parameters.AddWithValue("@GameId", platform.Cover);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return JsonUtils.CreateJsonFromSqlReader(reader);
                }
            }
        }
    }

}
