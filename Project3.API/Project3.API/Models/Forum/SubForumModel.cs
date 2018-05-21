using System;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Project3.API.Utilities;

namespace Project3.API.Models.Forum
{
    public class SubForumModel
    {
        public class Topic
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTimeOffset Created { get; set; }
            public int UserId { get; set; }
            public int PlatformId { get; set; }
            public DateTimeOffset LastActivity { get; set; }
        }

        public static JToken GetTopics()
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            using (SqlCommand command = new SqlCommand("SubForum_Select", conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return JsonUtils.CreateJsonArrayFromSqlReader(reader);
                }
            }
        }
    }
}
