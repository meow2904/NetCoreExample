using System.Text.RegularExpressions;

namespace NetCoreEx.Library.Helper
{
    public static class NumberHelper
    {
        public static long ToNumber(this string obj, long defaultvalue = 0)
        {
            try
            {
                return Convert.ToInt64(obj);
            }
            catch
            {
                return defaultvalue;
            }
        }

        public static int ToNumber(this string obj, int defaultvalue = 0)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return defaultvalue;
            }
        }

        public static double ToNumber(this string obj, double defaultvalue = 0)
        {
            try
            {
                return Convert.ToDouble(obj);
            }
            catch
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// Convert tiền
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ConvertMoney(this object obj)
        {
            if (obj == null) return "";
            double number = Convert.ToDouble(obj);
            bool negative = number < 0;

            try
            {
                number = Math.Abs(number);
                if (Math.Abs(number) < 0.01) return "0";
                if (number < 10) return negative ? "-" : "" + number.ToString("##.000");
                if (number < 100 && number >= 10) return negative ? "-" : "" + number.ToString("##.00");
                if (number < 1000 && number >= 100) return negative ? "-" : "" + number.ToString("###.0");
                return (negative ? "-" : "") + $"{number:0,0}";
            }
            catch
            {
                return "0";
            }
        }

        public static string ToEmptyZero(this string obj)
        {
            if (obj.ToString().Trim() == "0") return "";
            return obj;
        }

        /// <summary>
        /// Lấy số ngẫu nhiên
        /// </summary>
        public static int RandomNumber(int start, int end)
        {
            try
            {
                Random rnd = new Random();
                return rnd.Next(start, end);
            }
            catch
            {
                return 0;
            }
        }

        public static string RandomNumber(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Kiểu tra có phải là sô
        /// </summary>
        public static bool CheckNumber(this string value, out int n)
        {
            return int.TryParse(value, out n);
        }

        public static bool CheckNumber(this string value, out double n)
        {
            return double.TryParse(value, out n);
        }

        public static bool CheckPhoneNumber(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            if (value.Length < 9) return false;
            Regex rgx = new Regex(@"[^0-9.()#; +]");
            return !rgx.IsMatch(value);
        }
        public static string ConvertStringToMoney(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            try
            {
                string result = "";
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i] == ',') continue;
                    else result += value[i].ToString();
                }
                return result;
            }
            catch (Exception)
            {
                return "";
            }

        }
        /// <summary>
        /// kiểm tra xem có phải là tất cả mọi nhân viên không 
        /// </summary>
        /// <param name="IDEmployee"></param>
        /// <returns></returns>
        public static bool CheckEmployeeID(string EmployeeID)
        {
            try
            {
                Array arEmployee = EmployeeID.Split(",");
                for (int i = 0; i < arEmployee.Length; i++)
                {
                    if (((string[])arEmployee)[i] == "0")
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }

        }
    }
}