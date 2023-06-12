using Microsoft.AspNetCore.Mvc;
using NetCoreEx.Library.Helper;
using NetCoreEx.Service;
using NetCoreEx.Utilities;
using NetCoreEx.Utilities.ViewModels;

namespace NetCoreEx.WebApp.Controllers
{
    public class FormDemoController : Controller
    {
        private readonly IFormDemoService _formDemoService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public FormDemoController(IConfiguration configuration, IFormDemoService formDemoService, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _formDemoService = formDemoService;
            _env = env;
        }

        public async Task<IActionResult> Index(int page = 1, string search = "")
        {
            PagedResult<FormDemoViewModel> tempViewModels = await _formDemoService.GetAllPagingSortAsync(page, _configuration.GetValue<int>("PageSize"), search);
            return View(tempViewModels);
        }

        public async Task<IActionResult> AddOrUpdate(Guid formDemoId = new Guid())
        {
            FormDemoViewModel tempViewModel = await _formDemoService.FindByIdAsync(formDemoId);
            return View(tempViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            List<string> listFile = new List<string>();
            if (files.Count == 0) return BadRequest("Không có File nào được tải lên");
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string[] arrAllowExtensions = "jpg,jpeg,bmp,png".Split(',');
                    if (!arrAllowExtensions.Contains(Path.GetExtension(formFile.FileName).Remove(0, 1).ToLower()))
                        return BadRequest("File không hợp lệ. Danh sách các loại file có thể đính kèm là: " + string.Join(", ", arrAllowExtensions));

                    var fileName = "[" + DateTime.Now.ToString("yyyyMMddHHmmss") + "] " + Path.GetFileNameWithoutExtension(formFile.FileName).ToSlugUrl() + Path.GetExtension(formFile.FileName);
                    var pathDirectory = Path.Combine(_env.ContentRootPath, "wwwroot/upload/sale/com");
                    if (!Directory.Exists(pathDirectory))
                        Directory.CreateDirectory(pathDirectory);
                    var filePath = Path.Combine(pathDirectory, fileName);
                    await using var stream = System.IO.File.Create(filePath);
                    await formFile.CopyToAsync(stream);
                    await stream.FlushAsync();

                    var pathFile = "/upload/sale/com/" + fileName;
                    listFile.Add(pathFile);
                }
            }
            return Ok(listFile);
        }

        [Route("CheckBox")]
        public IActionResult CheckBox()
        {
            return View();
        }

        [Route("Input")]
        public IActionResult Input()
        {
            return View();
        }

        [Route("Datetime")]
        public IActionResult Datetime()
        {
            return View();
        }

        [Route("Test")]
        public IActionResult Test()
        {
            return View();
        }

        [Route("File")]
        public IActionResult File()
        {
            return View();
        }
    }
}