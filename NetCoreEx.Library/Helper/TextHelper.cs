using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace NetCoreEx.Library.Helper
{
	public static class TextHelper
	{
		/// <summary>
		/// Lấy ra đường dẫn SourceUrl 
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static string convertSourceUrl(this string url)
		{
			try
			{
				string resutl = "";
				var ar = url.Split("//");
				if (ar.Length > 0)
				{
					var arurl = ar[1].Split("/");
					if (arurl.Length > 0)
					{
						for (int i = 1; i < arurl.Length; i++)
						{
							resutl += "/" + arurl[i];
						}
					}
				}
				return resutl.DecodeURL();
			}
			catch (Exception)
			{
				return "/";
			}

		}
		/// <summary>
		/// Kiểm tra thẻ Tag
		/// 1, Thẻ tag là chữ thường
		/// 2, Độ dài tối đa là 45 ký tự
		/// 3, Thẻ Tag phải có từ 2 đến 6 từ
		/// 4, Không có ký tự đặc biệt(trừ -)
		/// </summary>
		/// <param name="input"></param>
		/// <param name="result"></param>
		/// <returns>true: Hợp lệ, false: Không hợp lệ</returns>
		public static bool CheckTag(this string input, out string result)
		{
			string check = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÀáÂãÈÉÊÌÍÒóÔÕÙÚÝàáâãèéêìíòóôõùúýĂăĐđĨĩŨũƠơƯưẠạẢảẤấẦầẨẩẪẫẬậẴắẰằẲẳẴẵẶặẸẹẺẻẼẽẾếỀềỂểỄễỆệỈỉỊịỌọỎỏỐốỒồỔổỖỗỘộỚớỜờỞởỠỡỢợỤụỦủỨứỪừỬửỮữỰựỲỳỴỵỶỷỸỹ- ";
			input = input.Trim().ToLower();
			int numberWord = input.Split(' ').Length;
			if (input.Length > 50)
			{
				result = "Thẻ tag có độ dài tối đa là 50 ký tự.";
				return false;
			}
			if (input.Length < 3)
			{
				result = "Thẻ tag có độ dài tối thiểu là 4 ký tự.";
				return false;
			}

			if (CheckAllChar(input, check))
			{
				result = "Thẻ tag có ký tự đặc biệt (trừ '-').";
				return false;
			}
			result = input;
			return true;
		}

		public static bool IsPasswordStrong(string password)

		{
			return Regex.IsMatch(password, @"^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?!.*\s).*$");
		}

		/// <summary>
		/// Lấy ra chuỗi ngẫu nhiên
		/// </summary>
		public static string RandomString(int length)
		{
			Random random = new Random();
			const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}

		/// <summary>
		/// Lấy ra chuỗi ngẫu nhiên
		/// </summary>
		public static string RandomStringNotNumber(int length)
		{
			Random random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}

		/// <summary>
		/// Lấy ra chuỗi ngẫu nhiên
		/// </summary>
		public static string RandomStringNumber(int length)
		{
			Random random = new Random();
			const string chars = "123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}
		/// <summary>
		/// Lấy ra chuỗi ngẫu nhiên có cả số và chữ cái hoa và thường, kí tự đặc biệt
		/// </summary>
		public static string RandomStringCharAndNumber(int length)
		{
			if (length <= 4) return "";
			Random random = new Random();
			const string charNumbers = "123456789";
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
			const string charUppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			const string charSpecials = @"!@#$%^&*()<>?/\|";
			return new string(Enumerable.Repeat(chars, length - 4)
				.Select(s => s[random.Next(s.Length)]).ToArray())
				+ new String(Enumerable.Repeat(charNumbers, 2)
				.Select(s => s[random.Next(s.Length)]).ToArray())
				+ new String(Enumerable.Repeat(charSpecials, 1)
				.Select(s => s[random.Next(s.Length)]).ToArray())
				+ new String(Enumerable.Repeat(charUppers, 1)
				.Select(s => s[random.Next(s.Length)]).ToArray()); 
		}

		/// <summary>
		/// Kiểm tra ký tự tồn tại trong chuỗi cho trước
		/// </summary>
		/// <param name="input">Chuỗi cần check</param>
		/// <param name="check">Chuỗi luật</param>
		/// <returns>true: có tồn tại, false: không tồn tại</returns>
		public static bool CheckAllChar(string input, string check)
		{
			return input.Any(t => check.IndexOf(t) < 0);
		}

		/// <summary>
		/// Xử lý Slug của Url theo Name
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string ToSlugUrl(this string input)
		{
			try
			{
				if (string.IsNullOrEmpty(input)) return "";
				input = input.ToLower();
				if (input.Length > 250) input = input.Substring(0, 250);
				MatchCollection matches = Regex.Matches(input, "\\w+");
				input = "";
				foreach (var match in matches)
				{
					input += match + "-";
				}
				input = Regex.Replace(input, "(\\d{2})/(\\d{2})/(\\d{4})|(\\d{2})-(\\d{2})-(\\d{4})", "");
				input = Regex.Replace(input, "á|à|ả|ạ|ã|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ", "a");
				input = Regex.Replace(input, "é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ", "e");
				input = Regex.Replace(input, "i|í|ì|ỉ|ĩ|ị", "i");
				input = Regex.Replace(input, "ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ", "o");
				input = Regex.Replace(input, "ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự", "u");
				input = Regex.Replace(input, "ý|ỳ|ỷ|ỹ|ỵ", "y");
				input = Regex.Replace(input, "đ", "d");
				input = Regex.Replace(input, "–|”|’|“|ω|♀|λ|→|♂|Ω|´|`", "");
				Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
				input = regex.Replace(input.Normalize(NormalizationForm.FormD), string.Empty);
				//Xóa các ký tự đặt biệt
				for (int i = 0x20; i < 0x30; i++)
				{
					input = input.Replace(((char)i).ToString(), " ");
				}
				for (int i = 0x3A; i < 0x40; i++)
				{
					input = input.Replace(((char)i).ToString(), " ");
				}
				for (int i = 0x5B; i < 0x60; i++)
				{
					input = input.Replace(((char)i).ToString(), " ");
				}
				for (int i = 0x7B; i < 0x7E; i++)
				{
					input = input.Replace(((char)i).ToString(), " ");
				}
				input = input.Replace("\n", "-");
				//Đổi khoảng trắng thành ký tự gạch ngang
				input = Regex.Replace(input, " ", "-");
				//Đổi nhiều ký tự gạch ngang liên tiếp thành 1 ký tự gạch ngang
				//Phòng trường hợp người nhập vào quá nhiều ký tự trắng
				while (input.Contains("--"))
				{
					input = input.Replace("--", "-");
				}
				while (input.Contains(" "))
				{
					input = input.Replace(" ", "");
				}
				if (input.Length > 96) input = input.Substring(0, 96);

				//Xóa các ký tự gạch ngang ở đầu và cuối
				input = '@' + input + '@';
				input = Regex.Replace(input, "@-|-@|@", "");
				input = input.Replace("?", "");
				return input;
			}
			catch
			{
				return "not-slug-url";
			}
		}

		public static string GetStringStar(double starAvg)
		{
			var str1 = "<i class='fa fa-star'></i>";
			var str2 = "<i class='fa fa-star-half-o'></i>";
			var str3 = "<i class='fa fa-star-o'></i>";
			if (starAvg <= 0.5) { return str3 + str3 + str3 + str3 + str3; }
			if (0.5 < starAvg && starAvg <= 1) { return str1 + str3 + str3 + str3 + str3; }
			if (1 < starAvg && starAvg <= 1.7) { return str1 + str2 + str3 + str3 + str3; }
			if (1.7 < starAvg && starAvg <= 2.2) { return str1 + str1 + str3 + str3 + str3; }
			if (2.2 < starAvg && starAvg <= 2.7) { return str1 + str1 + str2 + str3 + str3; }
			if (2.7 < starAvg && starAvg <= 3.2) { return str1 + str1 + str1 + str3 + str3; }
			if (3.2 < starAvg && starAvg <= 3.7) { return str1 + str1 + str1 + str2 + str3; }
			if (3.7 < starAvg && starAvg <= 4.2) { return str1 + str1 + str1 + str1 + str3; }
			if (4.2 < starAvg && starAvg <= 4.7) { return str1 + str1 + str1 + str1 + str2; }
			if (4.7 < starAvg) { return str1 + str1 + str1 + str1 + str1; }
			return str3 + str3 + str3 + str3 + str3;
		}

		/// <summary>
		/// Xử lý biển số xe
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string ExecuteLicensePlate(this string input)
		{
			try
			{
				input = input.ToLower();
				if (string.IsNullOrEmpty(input)) return "";
				input = Regex.Replace(input, "\\W+", "");
				input = Regex.Replace(input, " ", "");
				return input;
			}
			catch
			{
				return input;
			}
		}

		public static string Utf8Convert(this string input)
		{
			try
			{
				if (string.IsNullOrEmpty(input)) return "";
				Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
				string temp = input.Normalize(NormalizationForm.FormD);
				input = regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
				return input;
			}
			catch
			{
				return "";
			}
		}

		/// <summary>
		/// Xóa định dạng Font
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string StripFont(this string input)
		{
			if (string.IsNullOrEmpty(input)) return "";
			input = Regex.Replace(input, $@"font-size:\s*(\d+(\.\d+)*)(pt|px)\s*;*", "");
			input = Regex.Replace(input, "(\\w+\\-)*font\\-family:(\\s*)((\'|(\\&quot\\;))?(\\w+(\\s*)|\\-)+(\'|(\\&quot\\;))?\\s*\\,*\\s*)+(\\;*)", "");
			input = Regex.Replace(input, "(\\w+\\-)*font\\-family:(\\s*)(\"(\\w+(\\s*)|\\-)+\"\\s*\\,*\\s*)+(\\;*)", "");
			input = Regex.Replace(input, "((\\w+\\-)*font\\-family:)", "");
			return input;
		}

		/// <summary>
		/// Lấy ra HTML
		/// </summary>
		public static string ToStringPageSize(this int pageSize)
		{
			StringBuilder htmlText = new StringBuilder();
			if (pageSize == 10)
			{
				htmlText.AppendLine("<option value=\"10\" selected=\"selected\">10</option>");
				htmlText.AppendLine("<option value=\"20\">20</option>");
				htmlText.AppendLine("<option value=\"50\">50</option>");
				htmlText.AppendLine("<option value=\"100\">100</option>");
				htmlText.AppendLine("<option value=\"150\">150</option>");
			}
			else if (pageSize == 20)
			{
				htmlText.AppendLine("<option value=\"10\">10</option>");
				htmlText.AppendLine("<option value=\"20\" selected=\"selected\">20</option>");
				htmlText.AppendLine("<option value=\"50\">50</option>");
				htmlText.AppendLine("<option value=\"100\">100</option>");
				htmlText.AppendLine("<option value=\"150\">150</option>");
			}
			else if (pageSize == 50)
			{
				htmlText.AppendLine("<option value=\"10\">10</option>");
				htmlText.AppendLine("<option value=\"20\">20</option>");
				htmlText.AppendLine("<option value=\"50\" selected=\"selected\">50</option>");
				htmlText.AppendLine("<option value=\"100\">100</option>");
				htmlText.AppendLine("<option value=\"150\">150</option>");
			}
			else if (pageSize == 100)
			{
				htmlText.AppendLine("<option value=\"10\">10</option>");
				htmlText.AppendLine("<option value=\"20\">20</option>");
				htmlText.AppendLine("<option value=\"50\">50</option>");
				htmlText.AppendLine("<option value=\"100\" selected=\"selected\">100</option>");
				htmlText.AppendLine("<option value=\"150\">150</option>");
			}
			else if (pageSize == 150)
			{
				htmlText.AppendLine("<option value=\"10\">10</option>");
				htmlText.AppendLine("<option value=\"20\">20</option>");
				htmlText.AppendLine("<option value=\"50\">50</option>");
				htmlText.AppendLine("<option value=\"100\">100</option>");
				htmlText.AppendLine("<option value=\"150\" selected=\"selected\">150</option>");
			}
			else
			{
				htmlText.AppendLine("<option value=\"10\">10</option>");
				htmlText.AppendLine("<option value=\"20\">20</option>");
				htmlText.AppendLine("<option value=\"50\">50</option>");
				htmlText.AppendLine("<option value=\"100\">100</option>");
				htmlText.AppendLine("<option value=\"" + pageSize + "\" selected=\"selected\">" + pageSize + "</option>");
			}
			return htmlText.ToString();
		}

		/// <summary>
		/// Làm sạch text, xóa nhiều khoảng trắng
		/// </summary>
		public static string ClearText(this string input)
		{
			while (input.Contains("...."))
			{
				input = input.Replace("....", "-");
			}
			while (input.Contains("---"))
			{
				input = input.Replace("---", "=");
			}
			input = Regex.Replace(input, @"(&nbsp;)+", " ");
			input = input.Replace(@"\n", "");
			input = Regex.Replace(input, @"\s+", " ");
			return input.DecodeHTML().Trim();
		}

		/// <summary>
		/// Decode HTML
		/// </summary>
		public static string DecodeHTML(this string input)
		{
			if (string.IsNullOrEmpty(input)) return "";
			return HttpUtility.HtmlDecode(input);
		}

		/// <summary>
		/// Thực hiện EncodeBase64
		/// </summary>
		public static string EncodeBase64(this string plainText)
		{
			try
			{
				var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
				return Convert.ToBase64String(plainTextBytes);
			}
			catch
			{
				return "";
			}
		}

		/// <authors>
		/// Thực hiện DecodeBase64
		/// </authors>
		public static string DecodeBase64(this string base64EncodedData)
		{
			try
			{
				var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
				return Encoding.UTF8.GetString(base64EncodedBytes);
			}
			catch
			{
				return "";
			}
		}

		/// <summary>
		/// Xóa ký tự đặc biệt
		/// </summary>
		public static string RemoveSpecialChar(this string input)
		{
			try
			{
				input = Regex.Replace(input, @"\W+", " ");
				return input;
			}
			catch
			{
				return "";
			}
		}

		/// <summary>
		/// Xóa ký tự đặc biệt
		/// </summary>
		public static string CreateNickNameRandom()
		{
			try
			{
				string[] myName = new string[] { "Cà rốt", "Táo Tàu", "Su hào", "Bắp cải", "Cà chua", "Bí ngô", "Cu Tí", "Cu Tèo", "Nhỏ Xuka", "Thằng Tũn", "Thằng Tít", "Thằng Bờm", "Shin", "Tẹt", "Mén", "Jerry", "Cái Bống" };
				Random rnd = new Random();
				return myName[rnd.Next(1, myName.Length)];
			}
			catch
			{
				return "";
			}
		}

		/// <summary>
		/// Decode chuỗi URL
		/// </summary>
		public static string DecodeURL(this string input)
		{
			if (string.IsNullOrEmpty(input)) return "";
			return HttpUtility.UrlDecode(input);
		}

		/// <summary>
		/// Encode chuỗi URL
		/// </summary>
		public static string EncodeURL(this string input)
		{
			if (string.IsNullOrEmpty(input)) return "";
			return HttpUtility.UrlEncode(input);
		}

		/// <summary>
		/// Thực hiện Md5
		/// </summary>
		public static string GetMd5Hash(MD5 md5Hash, string input)
		{
			// Convert the input string to a byte array and compute the hash.
			byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

			// Create a new Stringbuilder to collect the bytes
			// and create a string.
			StringBuilder sBuilder = new StringBuilder();

			// Loop through each byte of the hashed data
			// and format each one as a hexadecimal string.
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}
			// Return the hexadecimal string.
			return sBuilder.ToString();
		}

		public static string Md5Hash(this string input)
		{
			using (MD5 md5Hash = MD5.Create())
			{
				// Convert the input string to a byte array and compute the hash.
				byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

				// Create a new Stringbuilder to collect the bytes
				// and create a string.
				StringBuilder sBuilder = new StringBuilder();

				// Loop through each byte of the hashed data
				// and format each one as a hexadecimal string.
				for (int i = 0; i < data.Length; i++)
				{
					sBuilder.Append(data[i].ToString("x2"));
				}
				// Return the hexadecimal string.
				return sBuilder.ToString();
			}
		}

		public static string ToMd5Hash(this string input)
		{
			using (MD5 md5Hash = MD5.Create())
			{
				byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
				StringBuilder sBuilder = new StringBuilder();
				for (int i = 0; i < data.Length; i++)
				{
					sBuilder.Append(data[i].ToString("x2"));
				}
				return sBuilder.ToString();
			}
		}

		/***********Encrypt - Decrypt*****************/

		public static string EncryptString(this string? plainText)
		{
			if (string.IsNullOrEmpty(plainText)) return "";
			try
			{
				var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
				var passwordBytes = Encoding.UTF8.GetBytes("phudx1996");
				passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
				var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);
				return Convert.ToBase64String(bytesEncrypted);
			}
			catch
			{
				return "";
			}
		}

		public static string DecryptString(this string encryptedText)
		{
			try
			{
				if (string.IsNullOrEmpty(encryptedText)) return "";
				var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
				var passwordBytes = Encoding.UTF8.GetBytes("phudx1996");
				passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
				var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);
				return Encoding.UTF8.GetString(bytesDecrypted);
			}
			catch
			{
				return "";
			}
		}

		private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
		{
			byte[] encryptedBytes;
			var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

			using (MemoryStream ms = new MemoryStream())
			{
				using (RijndaelManaged AES = new RijndaelManaged())
				{
					var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

					AES.KeySize = 256;
					AES.BlockSize = 128;
					AES.Key = key.GetBytes(AES.KeySize / 8);
					AES.IV = key.GetBytes(AES.BlockSize / 8);
					AES.Mode = CipherMode.CBC;
					using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
						cs.Close();
					}

					encryptedBytes = ms.ToArray();
				}
			}

			return encryptedBytes;
		}

		private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
		{
			try
			{
				byte[] decryptedBytes;
				var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
				using (MemoryStream ms = new MemoryStream())
				{
					using (RijndaelManaged AES = new RijndaelManaged())
					{
						var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

						AES.KeySize = 256;
						AES.BlockSize = 128;
						AES.Key = key.GetBytes(AES.KeySize / 8);
						AES.IV = key.GetBytes(AES.BlockSize / 8);
						AES.Mode = CipherMode.CBC;

						using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
						{
							cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
							cs.Close();
						}

						decryptedBytes = ms.ToArray();
					}
				}

				return decryptedBytes;
			}
			catch
			{
				throw;
			}
		}

		//GUID
		public static string StringToGuid(string guid, out Guid value)
		{
			return ObjectToGuid(guid, out value);
		}

		public static string ReadMoneyToText(string str)
		{
			try
			{
				str = str.Replace("-", "");
				string[] word = { "", " Một", " Hai", " Ba", " Bốn", " Năm", " Sáu", " Bẩy", " Tám", " Chín" };
				string[] million = { "", " Mươi", " Trăm", "" };
				string[] billion = { "", "", "", " Nghìn", "", "", " Triệu", "", "" };
				string result = "{0}";
				int count = 0;
				for (int i = str.Length - 1; i >= 0; i--)
				{
					if (count > 0 && count % 9 == 0)
						result = string.Format(result, "{0} Tỷ");
					if (!(count < str.Length - 3 && count > 2 && str[i].Equals('0') && str[i - 1].Equals('0') && str[i - 2].Equals('0')))
						result = string.Format(result, "{0}" + billion[count % 9]);
					if (!str[i].Equals('0'))
						result = string.Format(result, "{0}" + million[count % 3]);
					else if (count % 3 == 1 && count > 1 && !str[i - 1].Equals('0') && !str[i + 1].Equals('0'))
						result = string.Format(result, "{0} Lẻ");
					var num = Convert.ToInt16(str[i].ToString());
					result = string.Format(result, "{0}" + word[num]);
					count++;
				}
				result = result.Replace("{0}", "");
				result = result.Replace("Một Mươi", "Mười");
				return result.Trim() + " Đồng";
			}
			catch
			{
				return "";
			}
		}

		/// <summary>
		/// Thống kê đầu - đuôi kết quả xổ số
		/// </summary>
		public static string LotteryByFirstChacracter(this string input, string chacracter, string jackpots)
		{
			var result = "";
			bool flagJackpots = false;
			jackpots = jackpots.Substring(jackpots.Length - 2, 2);
			var inputArr = input.Split(',');
			foreach (var item in inputArr)
			{
				if (item.Substring(0, 1) == chacracter)
					if (item == jackpots && !flagJackpots)
					{
						flagJackpots = true;
						result += " <span style=\"color: #f5402d\">" + item + "</span> ,";
					}
					else
						result += " " + item + ",";
			}

			result = (result + ",").Replace(",,", "").Trim();
			if (result == ",") return "";
			return result;
		}

		/// <summary>
		/// Thống kê đầu - đuôi kết quả xổ số. Nhưng chỉ lấy 1 số
		/// </summary>
		public static string LotteryByFirst1Chacracter(this string input, string chacracter, string jackpots)
		{
			var result = "";
			bool flagJackpots = false;
			jackpots = jackpots.Substring(jackpots.Length - 2, 2);
			var inputArr = input.Split(',');
			foreach (var item in inputArr)
			{
				if (item.Substring(0, 1) == chacracter)
					if (item == jackpots && !flagJackpots)
					{
						flagJackpots = true;
						result += " <span style=\"color: #f5402d\">" + item.Substring(item.Length - 1, 1) + "</span> ,";
					}
					else
						result += " " + item.Substring(item.Length - 1, 1) + ",";
			}

			result = (result + ",").Replace(",,", "").Trim();
			if (result == ",") return "";
			return result;
		}

		/// <summary>
		/// Thống kê đuôi - đầu kết quả xổ số
		/// </summary>
		public static string LotteryByEndChacracter(this string input, string chacracter, string jackpots)
		{
			jackpots = jackpots.Substring(jackpots.Length - 2, 2);
			var result = "";
			bool flagJackpots = false;
			var inputArr = input.Split(',');
			foreach (var item in inputArr)
			{
				if (item.Substring(item.Length - 1, 1) == chacracter)
				{
					if (item == jackpots && !flagJackpots)
					{
						flagJackpots = true;
						result += " <span style=\"color: #f5402d\">" + item + "</span> ,";
					}
					else
						result += " " + item + ",";
				}
			}

			result = (result + ",").Replace(",,", "").Trim();
			if (result == ",") return "";
			return result;
		}

		public static string ObjectToGuid(object guid, out Guid value)
		{
			value = Guid.Empty;
			try
			{
				value = new Guid(guid.ToString());
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}

			return "";
		}

		/// <summary>
		///
		/// </summary>

		public static string HashSha1(string input)
		{
			var hash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input));
			return string.Concat(hash.Select(b => b.ToString("x2")));
		}

		public static string FirstCharToUpper(string input)
		{
			try
			{
				return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
			}
			catch
			{
				return input;
			}
		}

		public static string HexDecode(this string hex)
		{
			hex = hex.Replace("\\x22", "\"");
			hex = hex.Replace("\\x7b", "{");
			hex = hex.Replace("\\x5b", "[");
			hex = hex.Replace("\\x7d", "}");
			hex = hex.Replace("\\x5d", "]");
			hex = hex.Replace("\\x3d", "=");
			return hex;
		}

		public static byte[] FromHex(string hex)
		{
			hex = hex.Replace("-", "");
			byte[] raw = new byte[hex.Length / 2];
			for (int i = 0; i < raw.Length; i++)
			{
				raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
			}
			return raw;
		}

		public static string HextoString(this string inputText)
		{
			byte[] bb = Enumerable.Range(0, inputText.Length)
				.Where(x => x % 2 == 0)
				.Select(x => Convert.ToByte(inputText.Substring(x, 2), 16))
				.ToArray();
			//return Convert.ToBase64String(bb);
			char[] chars = new char[bb.Length / sizeof(char)];
			System.Buffer.BlockCopy(bb, 0, chars, 0, bb.Length);
			return new string(chars);
		}

		public static bool NullOrEmpty(this string text)
		{
			return string.IsNullOrEmpty(text);
		}
	}

	public static class ContainerNumber
	{
		//Alphabed and the parent Number from the DIN EN ISO 6346 Standard
		private static Dictionary<char, int> _Alphabet;

		/// <summary>
		/// ISO Validation Alphabed in a dictionary
		/// </summary>
		private static Dictionary<char, int> Alphabet
		{
			get
			{
				if (_Alphabet == null)
				{
					//If _Alphabed is null Initialise new dictionary and fill it
					_Alphabet = new Dictionary<char, int>();

					//Add Letters
					_Alphabet.Add('A', 10);
					_Alphabet.Add('B', 12);
					_Alphabet.Add('C', 13);
					_Alphabet.Add('D', 14);
					_Alphabet.Add('E', 15);
					_Alphabet.Add('F', 16);
					_Alphabet.Add('G', 17);
					_Alphabet.Add('H', 18);
					_Alphabet.Add('I', 19);
					_Alphabet.Add('J', 20);
					_Alphabet.Add('K', 21);
					_Alphabet.Add('L', 23);
					_Alphabet.Add('M', 24);
					_Alphabet.Add('N', 25);
					_Alphabet.Add('O', 26);
					_Alphabet.Add('P', 27);
					_Alphabet.Add('Q', 28);
					_Alphabet.Add('R', 29);
					_Alphabet.Add('S', 30);
					_Alphabet.Add('T', 31);
					_Alphabet.Add('U', 32);
					_Alphabet.Add('V', 34);
					_Alphabet.Add('W', 35);
					_Alphabet.Add('X', 36);
					_Alphabet.Add('Y', 37);
					_Alphabet.Add('Z', 38);

					//Add Numbers
					_Alphabet.Add('0', 0);
					_Alphabet.Add('1', 1);
					_Alphabet.Add('2', 2);
					_Alphabet.Add('3', 3);
					_Alphabet.Add('4', 4);
					_Alphabet.Add('5', 5);
					_Alphabet.Add('6', 6);
					_Alphabet.Add('7', 7);
					_Alphabet.Add('8', 8);
					_Alphabet.Add('9', 9);
				}

				return _Alphabet;
			}
		}

		/// <summary>
		/// Check if a Container number string is valid or not
		/// </summary>
		/// <param name="containerNumberToCheck">Container number string that has to be checked for validation</param>
		/// <returns>Boolean that shows if the Container number string is valid or not</returns>
		public static bool Check(string containerNumberToCheck)
		{
			//Clean the input string from Chars that are not in the Alphabed
			string containerNumber = CleanConNumberString(containerNumberToCheck);

			//Return true if the input string is empty
			//Used mostly for DataGridView to set the False validation only on false Container Numbers
			//and not empty ones
			if (containerNumber == string.Empty) return true;

			//Return False if the input string has not enough Characters
			if (containerNumber.Length != 11) return false;

			//Get the Sum of the ISO Formula
			double summ = GetSumm(containerNumber);

			//Calculate the Check number with the ISO Formula
			double tempCheckNumber = summ - (Math.Floor(summ / 11) * 11);

			//Set temCheckNumber 0 if it is 10 - In somme cases this is needed
			if (tempCheckNumber == 10) tempCheckNumber = 0;

			//Return true if the calculated check number matches with the input check number
			if (tempCheckNumber == GetCheckNumber(containerNumber))
				return true;

			//If no match return false
			return false;
		}

		/// <summary>
		/// Clean a Container number string from Chars that are not in the ISO Alphabed dictionary
		/// </summary>
		/// <param name="inputString">String that has to be cleaned</param>
		/// <returns>String that is cleaned from incorrect Chars</returns>
		private static string CleanConNumberString(string inputString)
		{
			//Set all Chars to Upper
			string resultString = inputString.ToUpper();

			//Loop Trough all chars
			foreach (char c in inputString)
			{
				//Remove Char if its not in the ISO Alphabet
				if (!Alphabet.Keys.Contains(c))
					resultString = resultString.Replace(c.ToString(), string.Empty); //Remove chars with the String.Replace Method
			}

			//Return the cleaned String
			return resultString;
		}

		/// <summary>
		/// Provides the Check number from a Container number string
		/// </summary>
		/// <param name="inputString">String of the Container number</param>
		/// <returns>Integer of the Check number</returns>
		private static int GetCheckNumber(string inputString)
		{
			//Loop if string is longer than 1
			if (inputString.Length > 1)
			{
				//Get the last char of the string
				char checkChar = inputString[inputString.Length - 1];

				//Initialise a integer
				int CheckNumber = 0;

				//Parse the last char to a integer
				if (Int32.TryParse(checkChar.ToString(), out CheckNumber))
					return CheckNumber; //Return the integer if the parsing can be done
			}

			//If parsing can´t be done and the string has just 1 char or is empty
			//Return 11 (A number that can´t be a check number!!!)
			return 11;
		}

		/// <summary>
		/// Calculate the sum by the ISO Formula
		/// </summary>
		/// <param name="inputString">String of the Container number</param>
		/// <returns></returns>
		private static double GetSumm(string inputString)
		{
			//Set summ to 0
			double summ = 0;

			//Calculate only if the container string is not empty
			if (inputString.Length > 1)
			{
				//Loop through all chars in the container string
				//EXCEPT the last char!!!
				for (int i = 0; i < inputString.Length - 1; i++)
				{
					//Get the current char
					char temChar = inputString[i];

					//Initialise a integer to represent the char number in the ISO Alphabet
					//Set it to 0
					int charNumber = 0;

					//If Char exists in the Table get it´s number
					if (Alphabet.Keys.Contains(temChar))
						charNumber = Alphabet[temChar];

					//Add the char number to the sum using the ISO Formula
					summ += charNumber * (Math.Pow(2, i));
				}
			}

			//Return the calculated summ
			return summ;
		}
	}
}