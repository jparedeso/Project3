using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Costos.API.Utilities
{
    public enum DuplicateColumnHandling
    {
        None,
        Ignore,
        Overwrite
    }

    public static class JsonUtils
    {
        public static JObject CreateJsonFromStoredProc(string storedProc, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            {
                return CreateJsonFromStoredProc(conn, storedProc, null, duplicateColumnHandling);
            }
        }
        public static JObject CreateJsonFromStoredProc(SqlConnection conn, string storedProc, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None)
        {
            return CreateJsonFromStoredProc(conn, storedProc, null, duplicateColumnHandling);
        }

        public static JObject CreateJsonFromStoredProc(string storedProc, NameValueCollection nvc, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            {
                return CreateJsonFromStoredProc(conn, storedProc, nvc, duplicateColumnHandling);
            }
        }

        public static JObject CreateJsonFromStoredProc(SqlConnection conn, string storedProc, NameValueCollection nvc, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None)
        {
            using (SqlCommand cmd = new SqlCommand() { Connection = conn, CommandType = CommandType.StoredProcedure, CommandText = storedProc })
            {
                SqlUtils.BuildCommandParameters(cmd, nvc);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return CreateJsonFromSqlReader(reader, duplicateColumnHandling);
                }
            }
        }

        public static JArray CreateJsonArrayFromStoredProc(string storedProc, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            {
                return CreateJsonArrayFromStoredProc(conn, storedProc, null, duplicateColumnHandling);
            }
        }

        public static JArray CreateJsonArrayFromStoredProc(SqlConnection conn, string storedProc, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None, SqlTransaction xact = null)
        {
            return CreateJsonArrayFromStoredProc(conn, storedProc, null, duplicateColumnHandling, xact);
        }

        public static JArray CreateJsonArrayFromStoredProc(string storedProc, NameValueCollection nvc, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None, SqlTransaction xact = null)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            {
                return CreateJsonArrayFromStoredProc(conn, storedProc, nvc, duplicateColumnHandling, xact);
            }
        }

        public static JArray CreateJsonArrayFromStoredProc(SqlConnection conn, string storedProc, NameValueCollection nvc, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None, SqlTransaction xact = null)
        {
            using (SqlCommand cmd = new SqlCommand() { Connection = conn, Transaction = xact, CommandType = CommandType.StoredProcedure, CommandText = storedProc })
            {
                SqlUtils.BuildCommandParameters(cmd, nvc);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return CreateJsonArrayFromSqlReader(reader, duplicateColumnHandling);
                }
            }
        }

        public static string CreateJsonStringArrayFromStoredProc(string storedProc, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            {
                return CreateJsonStringArrayFromStoredProc(conn, storedProc, null, duplicateColumnHandling);
            }
        }

        public static string CreateJsonStringArrayFromStoredProc(SqlConnection conn, string storedProc, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None)
        {
            return CreateJsonStringArrayFromStoredProc(conn, storedProc, null, duplicateColumnHandling);
        }

        public static string CreateJsonStringArrayFromStoredProc(string storedProc, NameValueCollection nvc, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None)
        {
            using (SqlConnection conn = DbConnectionFactory.CreateSqlConnection())
            {
                return CreateJsonStringArrayFromStoredProc(conn, storedProc, nvc, duplicateColumnHandling);
            }
        }

        public static string CreateJsonStringArrayFromStoredProc(SqlConnection conn, string storedProc, NameValueCollection nvc, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None)
        {
            using (SqlCommand cmd = new SqlCommand() { Connection = conn, CommandType = CommandType.StoredProcedure, CommandText = storedProc })
            {
                SqlUtils.BuildCommandParameters(cmd, nvc);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return CreateJsonStringArrayFromSqlReader(reader, duplicateColumnHandling);
                }
            }
        }

        public static JObject CreateJsonFromSqlReader(SqlDataReader reader, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None)
        {
            JObject json = new JObject();

            DataTable columnData = reader.GetSchemaTable();

            if (reader.Read())
            {
                for (int col = 0; col < reader.FieldCount; ++col)
                {
                    DataRow row = columnData.Rows[col];

                    string key = Convert.ToString(row.ItemArray[0]);

                    switch (duplicateColumnHandling)
                    {
                        case DuplicateColumnHandling.Ignore:
                            {
                                JToken value;

                                if (!json.TryGetValue(key, out value))
                                    json.Add(Convert.ToString(row.ItemArray[0]), JToken.FromObject(reader[col]));
                            }
                            break;
                        case DuplicateColumnHandling.Overwrite:
                            json[key] = JToken.FromObject(reader[col]);
                            break;
                        default:
                            json.Add(Convert.ToString(row.ItemArray[0]), JToken.FromObject(reader[col]));
                            break;
                    }
                }
            }
            else
                return null;

            return json;
        }

        public static JArray CreateJsonArrayFromSqlReader(SqlDataReader reader, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None)
        {
            JArray jsonArray = new JArray();

            DataTable columnData = reader.GetSchemaTable();

            while (reader.Read())
            {
                BaseCreateJsonArrayFromSqlReader(reader, jsonArray, columnData, duplicateColumnHandling);
            }

            return jsonArray;
        }

        public static async Task<JArray> CreateJsonArrayFromSqlReaderAsync(SqlDataReader reader, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None)
        {
            JArray jsonArray = new JArray();

            DataTable columnData = reader.GetSchemaTable();

            while (await reader.ReadAsync())
            {
                BaseCreateJsonArrayFromSqlReader(reader, jsonArray, columnData, duplicateColumnHandling);
            }

            return jsonArray;
        }

        private static void BaseCreateJsonArrayFromSqlReader(SqlDataReader reader, JArray jsonArray, DataTable columnData, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None)
        {
            JObject json = new JObject();

            for (int col = 0; col < reader.FieldCount; ++col)
            {
                DataRow row = columnData.Rows[col];

                string key = Convert.ToString(row.ItemArray[0]);

                var jToken = JToken.FromObject(reader[col]);
                var val = jToken.ToString().Contains("\"Data\": null") && jToken.ToString().Contains("\"AssemblyName\": \"\"") ? null : jToken;

                switch (duplicateColumnHandling)
                {
                    case DuplicateColumnHandling.Ignore:
                        {
                            JToken value;

                            if (!json.TryGetValue(key, out value))
                            {
                                json.Add(Convert.ToString(row.ItemArray[0]), val);
                            }
                        }
                        break;
                    case DuplicateColumnHandling.Overwrite:
                        json[key] = val;
                        break;
                    default:
                        json.Add(Convert.ToString(row.ItemArray[0]), val);
                        break;
                }
            }

            // only add it to the array if there was some data
            if (json.Count > 0)
                jsonArray.Add(json);
        }


        public static string CreateJsonStringArrayFromSqlReader(SqlDataReader reader, DuplicateColumnHandling duplicateColumnHandling = DuplicateColumnHandling.None)
        {
            StringBuilder sb = new StringBuilder("[");

            DataTable columnData = reader.GetSchemaTable();
            HashSet<string> keys = new HashSet<string>();

            bool firstObject = true;

            while (reader.Read())
            {
                keys.Clear();

                if (firstObject)
                    sb.Append("{");
                else
                {
                    sb.Append(",");
                    sb.AppendLine();
                    sb.Append("{");
                }

                firstObject = false;

                List<KeyValuePair<string, object>> values = new List<KeyValuePair<string, object>>();

                for (int col = 0; col < reader.FieldCount; ++col)
                {
                    DataRow row = columnData.Rows[col];

                    string key = Convert.ToString(row.ItemArray[0]);

                    switch (duplicateColumnHandling)
                    {
                        case DuplicateColumnHandling.Ignore:
                            if (keys.Contains(key))
                                continue;

                            values.Add(new KeyValuePair<string, object>(key, reader[col]));
                            keys.Add(key);
                            break;
                        case DuplicateColumnHandling.Overwrite:
                            if (keys.Contains(key))
                            {
                                values.Remove(values.FirstOrDefault(v => v.Key == key));
                                values.Add(new KeyValuePair<string, object>(key, reader[col]));
                                continue;
                            }

                            values.Add(new KeyValuePair<string, object>(key, reader[col]));
                            keys.Add(key);
                            break;
                        default:
                            if (keys.Contains(key))
                                throw new DuplicateNameException(string.Format("Cannot add duplicate key [{0}] to JSON; Value = {1}", key, reader[col]));

                            values.Add(new KeyValuePair<string, object>(key, reader[col]));
                            keys.Add(key);
                            break;
                    }
                }

                if (values.Any())
                {
                    bool firstValue = true;

                    foreach (var kvp in values)
                    {
                        if (!firstValue)
                            sb.Append(",");

                        firstValue = false;

                        sb.AppendFormat("\"{0}\":{1}", EscapeString(kvp.Key), FormatJsonValueFromDB(kvp.Value));
                    }
                }

                sb.Append("}");
            }

            sb.Append("]");

            return sb.ToString();
        }

        public static NameValueCollection CreateCollectionFromJson(string json)
        {
            return CreateCollectionFromJson(JObject.Parse(json));
        }

        public static NameValueCollection CreateCollectionFromJson(JObject jobj)
        {
            NameValueCollection nvc = new NameValueCollection();

            if (jobj != null)
            {
                foreach (KeyValuePair<string, JToken> token in jobj)
                {
                    nvc.AddJToken(token);
                }
            }

            return nvc;
        }

        public static void AddJToken(this NameValueCollection nvc, JObject jobj, string key)
        {
            if (nvc == null)
                throw new ArgumentNullException(nameof(nvc));
            if (jobj == null)
                throw new ArgumentNullException(nameof(jobj));

            JToken token = jobj[key];
            AddJToken(nvc, new KeyValuePair<string, JToken>(key, token));
        }

        public static void AddJToken(this NameValueCollection nvc, KeyValuePair<string, JToken> token)
        {
            if (nvc == null)
                throw new ArgumentNullException(nameof(nvc));

            string value;

            switch (token.Value.Type)
            {
                case JTokenType.Date:
                    // The default ToString() only has millisecond precision. Use RoundTrip format to preserve precision and offset.
                    value = token.Value.Value<DateTime>().ToString("o");
                    break;
                case JTokenType.Null:
                    value = null;
                    break;
                default:
                    value = token.Value.ToString();
                    break;
            }

            nvc.Add(token.Key, value);
        }

        public static JObject CreateJsonDiff(JArray array1, JArray array2, float thresholdPercent)
        {
            HashSet<string> array1HashSet = new HashSet<string>();
            List<JToken> addTokens = new List<JToken>();

            int thresholdCount = (int)Math.Round(array2.Count * thresholdPercent, MidpointRounding.AwayFromZero);

            foreach (JToken token in array1)
            {
                array1HashSet.Add(token.ToString());
            }

            foreach (JToken token in array2)
            {
                if (!array1HashSet.Remove(token.ToString()))
                    addTokens.Add(token);

                // if we've already crossed the threshold adding items, abort
                if (thresholdCount > 0 && addTokens.Count > thresholdCount)
                    break;
            }

            JObject result = new JObject();

            // if the total amount of changes is greater than the threshold, just send the new array
            if (thresholdCount > 0 && addTokens.Count + array1HashSet.Count > thresholdCount)
            {
                result.Add("Add", array2);
            }
            else
            {
                result.Add("Remove", JArray.FromObject(array1HashSet.Select(t => JToken.Parse(t))));
                result.Add("Add", JArray.FromObject(addTokens));
            }

            return result;
        }

        public static string FormatJsonValueFromDB(object value)
        {
            if (value == null)
                return "null";

            Type valueType = value.GetType();

            // null
            if (valueType == typeof(DBNull))
                return "null";

            // integer types
            if (valueType == typeof(byte) || valueType == typeof(short) || valueType == typeof(int) || valueType == typeof(long))
                return Convert.ToString(value);

            // floating point types
            if (valueType == typeof(float) || valueType == typeof(double) || valueType == typeof(decimal))
            {
                string strVal = Convert.ToString(value);
                if (strVal.Contains("."))
                {
                    strVal = strVal.TrimEnd('0');
                    if (strVal.EndsWith("."))
                        strVal += "0";
                }

                return strVal;
            }

            // boolean
            if (valueType == typeof(bool))
            {
                var boolean = false;
                return bool.TryParse(value.ToString(), out boolean) ? "true" : "false";
            }

            // DateTime
            if (valueType == typeof(DateTime))
                return string.Format("\"{0}\"", Convert.ToDateTime(value).ToString("yyyy-MM-ddTHH:mm:ss"));

            // DateTimeOffset
            if (valueType == typeof(DateTimeOffset))
                return string.Format("\"{0}\"", ((DateTimeOffset)value).ToString("yyyy-MM-ddTHH:mm:sszzz"));

            // GUID
            if (valueType == typeof(Guid))
                return string.Format("\"{0}\"", Convert.ToString(value));

            // string
            if (valueType == typeof(string))
                return string.Format("\"{0}\"", EscapeString(Convert.ToString(value)));

            // TimeSpan
            if (valueType == typeof(TimeSpan))
                return string.Format("\"{0}\"", ((TimeSpan)value).ToString(@"hh\:mm\:ss\.fffffff"));

            // unexpected type
            //Log.Error(string.Format("Unexpected DB value type {0}", valueType.Name));
            throw new NotImplementedException();
        }

        public static string EscapeString(string value)
        {
            StringBuilder sb = new StringBuilder();

            // per http://json.org/
            foreach (char c in value)
            {
                switch (c)
                {
                    case '"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '/':
                        sb.Append("\\/");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }

            return sb.ToString();
        }
    }
}