using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Project3.API.Utilities
{
    public class ParameterDictionary : IEnumerable
    {
        private Dictionary<string, object> _internalDict = new Dictionary<string, object>();

        public int Count { get { return _internalDict.Count; } }

        private string FormatParam(string param)
        {
            return ((param.StartsWith("@")) ? param.Substring(1) : param).ToLower();
        }

        public bool ContainsParam(string param)
        {
            return _internalDict.ContainsKey(FormatParam(param));
        }

        public object GetParamValue(string param)
        {
            if (ContainsParam(param))
                return _internalDict[FormatParam(param)];

            return null;
        }

        public void SetParamValue(string param, object value)
        {
            _internalDict[FormatParam(param)] = value;
        }

        public IEnumerator GetEnumerator()
        {
            return _internalDict.GetEnumerator();
        }
        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var item in _internalDict)
            {
                result.Add(item.Key, item.Value.ToString());
            }

            return result;
        }
    }

    public class SqlUtils
    {
        public static void BuildCommandParameters(SqlCommand cmd, ParameterDictionary parameters, bool update = false)
        {
            //SqlCommandBuilder.DeriveParameters(cmd);

            foreach (SqlParameter param in cmd.Parameters)
            {
                if (parameters.ContainsParam(param.ParameterName))
                {
                    switch (param.DbType)
                    {
                        case DbType.Guid:
                            string guid = Convert.ToString(parameters.GetParamValue(param.ParameterName));
                            if (!string.IsNullOrWhiteSpace(guid))
                                param.Value = Guid.Parse(guid);
                            break;

                        case DbType.Binary:
                            // Convert.ToString(null) will return string.Empty
                            object obj = parameters.GetParamValue(param.ParameterName);
                            param.Value = obj == null ? null : Convert.FromBase64String(Convert.ToString(obj));
                            break;

                        case DbType.Boolean:
                            try
                            {
                                param.Value = Convert.ToBoolean(parameters.GetParamValue(param.ParameterName));
                            }
                            catch
                            {
                                param.Value = (Convert.ToInt32(parameters.GetParamValue(param.ParameterName)) != 0) ? true : false;
                            }
                            break;

                        case DbType.AnsiString:
                        case DbType.AnsiStringFixedLength:
                        case DbType.String:
                        case DbType.StringFixedLength:
                            param.Value = parameters.GetParamValue(param.ParameterName);
                            break;

                        case DbType.DateTime:
                        case DbType.DateTime2:
                            param.Value = _GetDateTimeValue<DateTime>(parameters.GetParamValue(param.ParameterName));
                            break;

                        case DbType.DateTimeOffset:
                            param.Value = _GetDateTimeValue<DateTimeOffset>(parameters.GetParamValue(param.ParameterName));
                            break;

                        default:
                            object value = parameters.GetParamValue(param.ParameterName);
                            string stringValue = Convert.ToString(value);

                            param.Value = (string.IsNullOrEmpty(stringValue)) ? null : value;
                            break;
                    }

                    if (update)
                    {
                        string updatedParameter = string.Format("{0}Updated", param.ParameterName);
                        if (cmd.Parameters.Contains(updatedParameter))
                            cmd.Parameters[updatedParameter].Value = true;
                    }
                }
            }
        }

        public static void BuildCommandParameters(SqlCommand cmd, NameValueCollection nvc, bool update = false)
        {
            if (cmd.CommandType == CommandType.StoredProcedure)
            {
                //SqlCommandBuilder.DeriveParameters(cmd);

                foreach (string s in nvc ?? new NameValueCollection())
                {
                    string parameter = string.Format("@{0}", s);

                    if (cmd.Parameters.Contains(parameter))
                    {
                        SqlParameter param = cmd.Parameters[parameter];

                        if (nvc[s] == null)
                            param.Value = DBNull.Value;
                        else
                        {
                            switch (param.DbType)
                            {
                                case DbType.Guid:
                                    param.Value = Guid.Parse(nvc[s]);
                                    break;

                                case DbType.Binary:
                                    param.Value = Convert.FromBase64String(nvc[s]);
                                    break;

                                case DbType.Boolean:
                                    try
                                    {
                                        param.Value = Convert.ToBoolean(nvc[s]);
                                    }
                                    catch
                                    {
                                        param.Value = (Convert.ToInt32(nvc[s]) != 0) ? true : false;
                                    }
                                    break;

                                default:
                                    param.Value = nvc[s];
                                    break;
                            }
                        }
                    }

                    if (update)
                    {
                        string updatedParameter = string.Format("@{0}Updated", s);
                        if (cmd.Parameters.Contains(updatedParameter))
                            cmd.Parameters[updatedParameter].Value = true;
                    }
                }
            }
            else
            {
                foreach (string s in nvc ?? new NameValueCollection())
                {
                    string parameter = string.Format("@{0}", s);
                    cmd.Parameters.AddWithValue(parameter, nvc[s]);
                }
            }
        }

        private static T? _GetDateTimeValue<T>(object value) where T : struct
        {
            // This isn't a truly generic method for all structs. Only DateTime and DateTimeOffset
            // are supported, but there is no way to restrict the type to only those values.
            if (typeof(T) != typeof(DateTime) && typeof(T) != typeof(DateTimeOffset))
            {
                throw new InvalidOperationException($"{typeof(T).Name} is not a valid Type parameter for this method");
            }

            if (value == null)
            {
                return null;
            }

            // If value is already T we can use it directly.
            if (value is T || value is T?)
            {
                return (T?)value;
            }

            // If value is a string, attempt to convert it to either DateTime or DateTimeOffset.
            if (value is string)
            {
                string msg = $"Invalid {typeof(T).Name} string";

                if (typeof(T) == typeof(DateTimeOffset))
                {
                    DateTimeOffset parsedDTO;
                    if (DateTimeOffset.TryParse((string)value, null, DateTimeStyles.RoundtripKind, out parsedDTO))
                    {
                        return parsedDTO as T?;
                    }

                    else
                    {
                        throw new FormatException(msg);
                    }
                }

                else if (typeof(T) == typeof(DateTime))
                {
                    DateTime parsedDT;
                    if (DateTime.TryParse((string)value, null, DateTimeStyles.RoundtripKind, out parsedDT))
                    {
                        return parsedDT as T?;
                    }

                    else
                    {
                        throw new FormatException(msg);
                    }
                }
            }

            throw new ArgumentException($"Unable to convert object of type {value.GetType().Name} to a {typeof(T).Name}", nameof(value));
        }
    }
}
