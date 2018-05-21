using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Project3.API.Utilities;

namespace Project3.API.Models
{
    public class UserModel
    {
        public static JToken GetUsers(int userId)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            using (SqlCommand command = new SqlCommand("Users_Select", conn))
            {
                command.CommandType = CommandType.StoredProcedure;

                //if (!string.IsNullOrEmpty(clientIdentifier))
                command.Parameters.AddWithValue("@UserID", userId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    return JsonUtils.CreateJsonArrayFromSqlReader(reader);
                }
            }
        }
    }
}
