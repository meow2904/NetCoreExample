using HtmlAgilityPack;
using System.Text;
using System.Text.RegularExpressions;

namespace NetCoreEx.Library.Helper
{
    public class Company
    {
        public string? MaSoThue { set; get; }
        public string? Title { set; get; }
        public string? TitleEn { set; get; }
        public string? DiaChiCongTy { set; get; }
        public string? DaiDienPhapLuat { set; get; }
        public string? NoiDangKyQuanLy_DienThoai { set; get; }
    }

    public static class CompanyHelper
    {
        public static Company GetCompany(this string taxNumber)
        {
            Company company = new Company();
            HtmlWeb htmlWeb = new HtmlWeb
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8
            };
            try
            {
                HtmlDocument document = htmlWeb.Load("http://www.tratencongty.com/search/" + taxNumber);
                var url = document.DocumentNode.SelectSingleNode("//div[@class='search-results']/a").Attributes["href"].Value;
                document = htmlWeb.Load(url);
                HtmlNode node = document.DocumentNode.SelectSingleNode("//meta[@name='description']");
                string desc = node.GetAttributeValue("content", "").Replace(", Mã số thuế:", "@").Replace(", Địa chỉ:", "@");
                var arrDesc = desc.Split('@');
                company.Title = arrDesc[0].ToString();
                company.MaSoThue = taxNumber;
                company.DiaChiCongTy = arrDesc[1].ToString();
                string form = document.DocumentNode.SelectSingleNode("//div[@class='jumbotron']").InnerHtml;
                Regex regex = new Regex("Đại diện pháp luật:\\D+<br>");
                string value = regex.Match(form).Value;
                company.DaiDienPhapLuat = value.Replace("Đại diện pháp luật:", "").Replace("<br>", "").Trim();
            }
            catch
            {
                return company;
            }
            return company;
        }

        //public static Company GetCompany(this string taxNumber)
        //{
        //    Company company;
        //    try
        //    {
        //        string result = ApiHelper.GetApi("", "https://thongtindoanhnghiep.co", $"/api/company/" + taxNumber).Content.ReadAsStringAsync().Result;
        //        company = JsonConvert.DeserializeObject<Company>(result);
        //    }
        //    catch
        //    {
        //        return new Company();
        //    }

        //    return company;
        //}
    }
}