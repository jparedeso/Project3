using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Costos.API.Utilities
{
    public static class DbConnectionFactory
    {
        private static string _connectionString =
            "Data source=project3db.cxnhooasyo17.us-east-1.rds.amazonaws.com,1433;Initial Catalog=Project3;User ID=project3;Password=Project31234;Connection Timeout=0;MultipleActiveResultSets=true;";

        public static SqlConnection CreateSqlConnection()
        {
            return CreateSqlConnectionEx("DefaultConnection");
        }

        public static SqlConnection CreateSqlConnectionEx(string connStringKey)
        {
            //string connectionString = string.Empty;

            //var connectionStringSetting = ConfigurationManager.ConnectionStrings[connStringKey];
            //if (connectionStringSetting != null)
            //{
            //    connectionString = connectionStringSetting.ConnectionString;
            //}

            //if (string.IsNullOrEmpty(connectionString))
            //{
            //    throw new ArgumentException("No connection string was provided in the configuration.");
            //    //throw new ConfigurationErrorsException("No connection string was provided in the configuration.");
            //}

            return CreateSqlConnection(_connectionString);
        }

        public static SqlConnection CreateSqlConnection(string dataSource, string initialCatalog)
        {
            return CreateSqlConnection(dataSource, initialCatalog, string.Empty, string.Empty);
        }

        public static SqlConnection CreateSqlConnection(string dataSource, string initialCatalog, string userID, string password)
        {
            SqlConnectionStringBuilder connStringBuilder = new SqlConnectionStringBuilder();
            connStringBuilder.DataSource = dataSource;
            connStringBuilder.InitialCatalog = initialCatalog;
            connStringBuilder.PersistSecurityInfo = true;
            connStringBuilder.UserID = userID;
            connStringBuilder.Password = password;

            return CreateSqlConnection(connStringBuilder.ConnectionString);
        }

        public static SqlConnection CreateSqlConnection(string connectionString)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();

            return conn;
        }

        public static Task<SqlConnection> CreateSqlConnectionAsync()
        {
            return CreateSqlConnectionExAsync("connectionString");
        }

        public static async Task<SqlConnection> CreateSqlConnectionExAsync(string connStringKey)
        {
            //string connectionString = string.Empty;

            //var connectionStringSetting = ConfigurationManager.ConnectionStrings[connStringKey];
            //if (connectionStringSetting != null)
            //{
            //    connectionString = connectionStringSetting.ConnectionString;
            //}

            //if (string.IsNullOrEmpty(connectionString))
            //{
            //    throw new ArgumentException("No connection string was provided in the configuration.");
            //    //throw new ConfigurationErrorsException("No connection string was provided in the configuration.");
            //}

            return await CreateSqlConnectionAsync(_connectionString);
        }

        public static async Task<SqlConnection> CreateSqlConnectionAsync(string dataSource, string initialCatalog)
        {
            return await CreateSqlConnectionAsync(dataSource, initialCatalog, string.Empty, string.Empty);
        }

        public static async Task<SqlConnection> CreateSqlConnectionAsync(string dataSource, string initialCatalog, string userID, string password)
        {
            SqlConnectionStringBuilder connStringBuilder = new SqlConnectionStringBuilder();
            connStringBuilder.DataSource = dataSource;
            connStringBuilder.InitialCatalog = initialCatalog;
            connStringBuilder.PersistSecurityInfo = true;
            connStringBuilder.UserID = userID;
            connStringBuilder.Password = password;

            return await CreateSqlConnectionAsync(connStringBuilder.ConnectionString);
        }

        public static async Task<SqlConnection> CreateSqlConnectionAsync(string connectionString)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            return conn;
        }
    }
}
