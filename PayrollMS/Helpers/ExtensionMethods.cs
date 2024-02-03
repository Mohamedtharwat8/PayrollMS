using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Dynamic;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml;
using System.Collections;

namespace PayrollMS.Helpers
{
    public static partial class ExtensionMethods
    {
        public static bool IsList(this object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public static bool IsDictionary(this object o)
        {
            if (o == null) return false;
            return o is IDictionary &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }
        public static string GetValue(this ClaimsPrincipal principal, string type)
        {
            string value = principal.FindFirstValue(type);
            if (value.IsNotNullOrEmpty())
            {
                return value;
            }

            return "";
        }
        public static bool IsNumber(this string number)
        {
            long result = 0;
            return long.TryParse(number.ConvertArabicNumbersToEnglish(), out result);
        }
        public static bool IsNumber(ref string number)
        {
            long result;
            if (long.TryParse(number.ConvertArabicNumbersToEnglish(), out result))
            {
                number = result.ToString();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Convert Arabic Numbers to English Numbers
        /// </summary>
        /// <param name="value">numbers as string</param>
        /// <returns>if value is Null Or WhiteSpace then value else converted value</returns>
        public static string ConvertArabicNumbersToEnglish(this string value)
        {
            if (value.IsNullOrEmpty() || value.Trim().Length == 0)
                return value;

            string englishNumber = string.Empty;
            foreach (var character in value)
                englishNumber += char.IsDigit(character) ? char.GetNumericValue(character).ToString() : character.ToString();
            return englishNumber;
        }
        /// <summary>
        /// Validate Commercial Registration number (Long)
        /// </summary>
        /// <param name="crNumber">Commercial Registration number to validate</param>
        /// <returns>Boolean</returns>
        public static bool IsCRValid(this long crNumber)
        {
            try
            {
                return crNumber.ToString().Length == 10 && crNumber > 999999999;
            }
            catch (Exception)
            {

                return false;
            }

        }
        /// <summary>
        /// Validate Commercial Registration number (String)
        /// </summary>
        /// <param name="crNumber">Commercial Registration number to validate</param>
        /// <returns>Boolean</returns>
        public static bool IsCRValid(this string crNumber)
        {
            return crNumber.IsNotNullOrEmpty() && crNumber.ToString().Length == 10 && crNumber.ToLong() > 999999999;
        }
        /// <summary>
        /// Validate identity ( YakeenID - IqamaID ) syntax
        /// </summary>
        /// <param name="identity">YakeenID 0r IqamaID</param>
        /// <returns></returns>
        public static bool IsIdentityValid(this string identity)
        {
            try
            {
                if (identity == "$$NoID$$")
                    return true;
                if (identity.Length < 9 || !IsNumber(ref identity))
                    return false;
                identity = identity.ConvertArabicNumbersToEnglish();

                StringBuilder digits = new StringBuilder(15);
                for (int i = 0; i <= 8; i++)
                {
                    if ((i + 1) % 2 == 0)
                        digits.Append(identity.Substring(i, 1));
                    else
                        digits.Append((identity.Substring(i, 1).ToInt() * 2).ToString());
                }

                int digitsSum = 0;
                for (int i = 0; i < digits.Length; i++)
                    digitsSum += digits[i].ToString().ToInt();

                string sum = digitsSum.ToNullableString();
                short oddSumDigit = (digitsSum.ToString().Length == 1 ? sum.ToShort() : sum.Substring(sum.Length - 1, 1).ToShort());

                string result;
                if (oddSumDigit == 0)
                    result = "0";
                else
                    result = (10 - oddSumDigit).ToString();

                return (result == identity.Substring(identity.Length - 1, 1));
            }
            catch
            {
                return false;
            }
        }
        public static bool IsAlphaNumeric(this string value)
        {
            return Regex.IsMatch(value, @"^[a-zA-Z0-9]+$");
        }
        public static bool IsValidURL(this string strURL, UriKind uriKind)
        {
            return Uri.IsWellFormedUriString(strURL, uriKind);
        }
        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsValidTBCEmail(this string email)
        {
            return email.IsValidEmail() && email.ToLower().EndsWith("@tbc.sa");
        }
        private static sbyte IsValidMobile(string mobile)
        {
            if (!mobile.IsNumber())
                return -2;
            else if (mobile.Length < 7 || mobile.Length > 10)
                return -3;
            else
                return 0;
        }

        public static string ValidateSaudiMobile(this string mobile)
        {
            try
            {
                if (string.IsNullOrEmpty(mobile))
                    return null;
                mobile = mobile.TrimStart('0', '+').Trim().TrimStart('0', '+').Trim();
                if (mobile.StartsWith("966"))
                    mobile = mobile.Substring(3).TrimStart('0');
                if (mobile.StartsWith("665"))
                    mobile = mobile.Substring(2).TrimStart('0');
                if (IsValidMobile(mobile) == 0 && mobile.Length == 9 && mobile.StartsWith("5"))
                    return "966" + mobile;

                return null; // Non Saudi Number
            }
            catch
            {
                return null;
            }
        }

        public static bool IsValidTime(this string thetime)
        {
            //Regex checktime =
            // new Regex(@"^(20|21|22|23|[01]d|d)(([:][0-5]d){1,2})$");
            //return checktime.IsMatch(thetime);
            DateTime time;
            if (DateTime.TryParse(thetime, out time))
            {
                return true;
            }
            return false;
        }


        public static List<dynamic> ToDynamic(this DataTable dt)
        {
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return dynamicDt;
        }
        public static int getTimeInt(this string value)
        {
            try
            {
                return value.Replace(":", "").ToInt();
            }
            catch (Exception)
            {

                return 0;
            }

        }

        public static int ToInt(this object value)
        {
            try
            {
                if (value.IsNull())
                    value = 0;
                return Convert.ToInt32(value);
            }
            catch (Exception)
            {

                return 0;
            }

        }
        public static long ToBigInt(this object value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch (Exception)
            {

                return 0;
            }

        }
        public static UInt32 ToUInt32(this object value)
        {
            return Convert.ToUInt32(value);
        }
        public static int? ToNullableInt(this object value)
        {
            if (Convert.IsDBNull(value) || value == null)
                return null;
            return value.ToInt();
        }
        public static System.DateTime GetDateFromNumber(this object value)
        {
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            var obj = value.ToInt();
            return DateTime.FromOADate(obj);
        }
        public static int GetNumberFromDate(this object value)
        {
            var obj = Convert.ToDateTime(value);
            return Convert.ToInt32(obj.ToOADate());
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
        public static string ToStringIfNull(this object value, string replace = "")
        {
            if (IsNull(value))
                return replace;
            return Convert.ToString(value);
        }
        public static string ToTrim(this object value)
        {
            if (IsNull(value))
                return "";
            return Convert.ToString(value).Trim();
        }
        public static string ToNullableString(this object value)
        {
            if (IsNull(value))
                return null;
            return Convert.ToString(value);
        }
        public static bool ToBoolean(this object value)
        {
            return Convert.ToBoolean(value);
        }

        public static bool? ToNullableBoolean(this object value)
        {
            if (IsNull(value))
                return null;
            return value.ToBoolean();
        }

        public static decimal ToDecimal(this object value, string stringFormat = "N2")
        {
            if (decimal.TryParse(value.ToString(), out decimal parsedInput))
            {
                return Convert.ToDecimal(parsedInput.ToString(stringFormat));
            }
            return 0M;
        }

        public static double ToDouble(this object value)
        {
            return Convert.ToDouble(value);
        }
        public static DateTime ToDateTime(this string value)
        {
            try
            {
                if (DateTime.TryParse(value, out var outdate))
                {
                    return outdate;
                }
                return new DateTime();
            }
            catch (Exception)
            {

                return new DateTime();
            }
        }
        public static DateTime? ToNullbleDateTime(this string value)
        {
            try
            {
                if (IsNull(value))
                    return null;
                if (DateTime.TryParse(value, out var outdate))
                {
                    return outdate;
                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static decimal? ToNullableDecimal(this object value)
        {

            if (IsNull(value) || string.IsNullOrEmpty(value.ToNullableString()))
                return null;
            return value.ToDecimal();
        }

        public static byte ToByte(this object value)
        {
            return Convert.ToByte(value);
        }
        public static byte ToBytes(this object value)
        {
            return Convert.ToByte(value);
        }
        public static byte? ToNullableByte(this object value)
        {
            if (IsNull(value))
                return null;
            return value.ToByte();
        }

        public static sbyte ToSByte(this object value)
        {
            return Convert.ToSByte(value);
        }
        public static sbyte? ToNullableSByte(this object value)
        {
            if (IsNull(value))
                return null;
            return value.ToSByte();
        }

        public static short ToShort(this object value)
        {
            return Convert.ToInt16(value);
        }
        public static short? ToNullableShort(this object value)
        {
            if (IsNull(value))
                return null;
            return value.ToShort();
        }

        public static long ToLong(this object value)
        {
            return Convert.ToInt64(value);
        }
        public static long? ToNullableLong(this object value)
        {

            if (IsNull(value))
                return null;
            return value.ToLong();
        }

        public static char ToChar(this object value)
        {
            return Convert.ToChar(value);
        }
        public static char? ToNullableChar(this object value)
        {
            if (IsNull(value))
                return null;
            return value.ToChar();
        }

        public static bool IsNull(this object obj)
        {
            return (Convert.IsDBNull(obj) || obj == null);
        }
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value) || value.Trim().Length == 0;
        }
        public static bool IsInt(this object obj)
        {
            try
            {
                int temp = 0;
                return int.TryParse(obj.ToString(), out temp);
            }
            catch (Exception)
            {

                return false;
            }

        }
        public static bool IsLong(this object obj)
        {
            try
            {
                long temp = 0;
                return long.TryParse(obj.ToString(), out temp);
            }
            catch (Exception)
            {

                return false;
            }

        }
        public static IEnumerable<List<T>> InSetsOf<T>(this IEnumerable<T> source, int max)
        {
            List<T> toReturn = new List<T>(max);
            foreach (var item in source)
            {
                toReturn.Add(item);
                if (toReturn.Count == max)
                {
                    yield return toReturn;
                    toReturn = new List<T>(max);
                }
            }
            if (toReturn.Any())
            {
                yield return toReturn;
            }
        }
        public static DataSet ToDataSet<T>(this IEnumerable<T> list)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            //add a column to table for each public property on T
            foreach (var propInfo in elementType.GetProperties())
            {
                Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                t.Columns.Add(propInfo.Name, ColType);
            }

            //go through each property on T and add each value to the table
            foreach (T item in list)
            {
                DataRow row = t.NewRow();

                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                }

                t.Rows.Add(row);
            }

            return ds;
        }
        public sealed class Tuple<T1, T2>
        {
            public Tuple() { }
            public Tuple(T1 value1, T2 value2) { Value1 = value1; Value2 = value2; }
            public T1 Value1 { get; set; }
            public T2 Value2 { get; set; }
        }
        public static string GetToDBColumnName<T>(this string columnName) where T : class, new()
        {
            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                ColumnAttribute col = (ColumnAttribute)
                    Attribute.GetCustomAttribute(pi, typeof(ColumnAttribute));
                if (col != null && col.Name.IsNotNullOrEmpty() && pi.Name == columnName)
                    return col.Name;
            }
            return columnName;
        }
        public static List<T> MapTo<T>(this DataTable table)
            where T : class, new()
        {
            var colErr = "";
            List<Tuple<object, PropertyInfo>> map =
                new List<Tuple<object, PropertyInfo>>();

            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                ColumnAttribute col = (ColumnAttribute)
                    Attribute.GetCustomAttribute(pi, typeof(ColumnAttribute));
                if (col == null || col.Name.IsNullOrEmpty())
                {
                    if (table.Columns.Contains(pi.Name))
                    {
                        map.Add(new Tuple<object, PropertyInfo>(
                        table.Columns[pi.Name], pi));
                    }
                }
                else
                {
                    if (!col.Name.Contains(".") && table.Columns.Contains(col.Name))
                    {
                        map.Add(new Tuple<object, PropertyInfo>(
                            table.Columns[col.Name], pi));
                    }
                    else
                    {
                        map.Add(new Tuple<object, PropertyInfo>(
                          col.Name.Split('.').ToList().Last(), pi));
                    }
                }
            }
            try
            {
                List<T> list = new List<T>();
                foreach (DataRow row in table.Rows)
                {
                    if (row == null)
                    {
                        list.Add(null);
                        continue;
                    }
                    T item = new T();
                    foreach (Tuple<object, PropertyInfo> pair in map)
                    {
                        colErr = pair.Value1.ToString();
                        object value;
                        if (pair.Value1.GetType() == typeof(DataColumn))
                        {
                            value = row[(DataColumn)pair.Value1];
                            if (value is DBNull) value = null;
                            pair.Value2.SetValue(item, value, null);
                        }
                        else
                        {
                            pair.Value2.SetValue(item, mapToObject(row, pair.Value2, pair.Value1.ToString()), null); ;

                        }


                    }
                    list.Add(item);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw new Exception($"Error At Col:{colErr}{Environment.NewLine}{ex.Message}", ex);
            }


        }


        private static object mapToObject(DataRow dataRow, PropertyInfo value2, string key)
        {
            if (dataRow == null) return null;

            DataTable table = dataRow.Table.Clone();
            table.ImportRow(dataRow);
            var dtlFieldNames = table.Columns.Cast<DataColumn>().
             Select(item => new
             {
                 Name = item.ColumnName,
                 Type = item.DataType
             }).ToList();

            List<Tuple<DataColumn, PropertyInfo>> map =
                new List<Tuple<DataColumn, PropertyInfo>>();

            foreach (PropertyInfo pi in value2.PropertyType.GetProperties())
            {
                ColumnAttribute col = (ColumnAttribute)
                    Attribute.GetCustomAttribute(pi, typeof(ColumnAttribute));
                if (col == null || col.Name.IsNullOrEmpty())
                {
                    if (pi.Name.ToLower() == "id")
                    {
                        if (dtlFieldNames.Any(s => s.Name.ToLower() == key.ToLower()))
                        {
                            map.Add(new Tuple<DataColumn, PropertyInfo>(
                            table.Columns[key], pi));
                        }
                    }
                    else if (dtlFieldNames.Any(s => s.Name.ToLower() == pi.Name.ToLower()))
                    {
                        map.Add(new Tuple<DataColumn, PropertyInfo>(
                        table.Columns[pi.Name], pi));
                    }
                }
                else
                {
                    if (dtlFieldNames.Any(s => s.Name.ToLower() == col.Name.ToLower()))
                    {
                        map.Add(new Tuple<DataColumn, PropertyInfo>(
                            table.Columns[col.Name], pi));
                    }
                    else
                    {
                        map.Add(new Tuple<DataColumn, PropertyInfo>(
                           null, pi));
                    }
                }
            }
            try
            {
                var row = table.Rows[0];

                if (value2.PropertyType.IsClass)
                {
                    object instance = Activator.CreateInstance(value2.PropertyType);
                    foreach (Tuple<DataColumn, PropertyInfo> pair in map)
                    {
                        object value;
                        if (pair.Value1 != null)
                        {
                            value = row[pair.Value1];
                            if (value is DBNull) value = null;
                            pair.Value2.SetValue(instance, value, null);
                        }




                    }
                    return instance;

                }

                return null;


            }
            catch (Exception ex)
            {

                throw ex; //new Exception(ex.Message,ex.InnerException);
            }
            finally
            {
                table.Dispose();
            }




        }
        public static void FromXmlStringRSA(this RSA rsa, string xmlString)
        {
            try
            {


                var parameters = new RSAParameters();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlString);

                if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
                {
                    foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                    {
                        switch (node.Name)
                        {
                            case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                            case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                            case "P": parameters.P = Convert.FromBase64String(node.InnerText); break;
                            case "Q": parameters.Q = Convert.FromBase64String(node.InnerText); break;
                            case "DP": parameters.DP = Convert.FromBase64String(node.InnerText); break;
                            case "DQ": parameters.DQ = Convert.FromBase64String(node.InnerText); break;
                            case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
                            case "D": parameters.D = Convert.FromBase64String(node.InnerText); break;
                        }
                    }
                }
                else
                {
                    throw new Exception("Invalid XML RSA key.");
                }

                rsa.ImportParameters(parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
