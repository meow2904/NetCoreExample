using Microsoft.AspNetCore.Mvc;
using NetCoreEx.Model.Entities;
using NetCoreEx.Service;
using NetCoreEx.Utilities.ViewModels;
using NetCoreEx.WebApp.Extensions;
using Newtonsoft.Json;
using System.Transactions;

namespace NetCoreEx.WebApp.WebApi
{
    public class FormDemoController : ApiControllerBase
    {
        private readonly ILogger<FormDemoController> _logger;
        private readonly ILogHistoryService _logHistoryService;
        private readonly IFormDemoService _formDemoService;
        private readonly IFormDemoDetailService _formDemoDetailService;
        private readonly IWebHostEnvironment _environment;

        public FormDemoController(ILogger<FormDemoController> logger, ILogHistoryService logHistoryService, IFormDemoService formDemoService, IWebHostEnvironment environment, IFormDemoDetailService formDemoDetailService) : base(logHistoryService)
        {
            _logger = logger;
            _logHistoryService = logHistoryService;
            _formDemoService = formDemoService;
            _environment = environment;
            _formDemoDetailService = formDemoDetailService;
        }

        [HttpPost]
        [Route("AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdate(FormDemoViewModel formDemoViewModel)
        {
            try
            {
                formDemoViewModel.AppUserId = User.Identity.GetUserId();
                formDemoViewModel = await _formDemoService.AddOrUpdateAsync(formDemoViewModel);
                if (formDemoViewModel.FormDemoDetailViewModels!.Count > 0)
                {
                    foreach (var formDemoDetailViewModel in formDemoViewModel.FormDemoDetailViewModels)
                    {
                        formDemoDetailViewModel.FormDemoId = formDemoViewModel.FormDemoId;
                        await _formDemoDetailService.AddOrUpdateAsync(formDemoDetailViewModel);
                    }
                }
                //Ghi lịch sử
                await AddLogHistoryAsync(new LogHistory
                {
                    ObjectId = formDemoViewModel.FormDemoId,
                    Url = "/",
                    Content = formDemoViewModel.FormDemoId != Guid.Empty ? "Cập nhật: " + formDemoViewModel.Name : "Thêm mới: " + formDemoViewModel.Name //Tuyệt đối ghi lịch sử không được lấy Id mà phải là {tên, mã}
                });
                await _formDemoService.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Param: " + JsonConvert.SerializeObject(formDemoViewModel));
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("Select")]
        public async Task<IActionResult> Select()
        {
            try
            {
                return Ok(await _formDemoService.SelectAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("Remove/{formDemoId}")]
        public async Task<IActionResult> Remove(Guid formDemoId)
        {
            try
            {
                var formDemoName = await _formDemoService.RemoveAsync(formDemoId);
                await AddLogHistoryAsync(new LogHistory
                {
                    ObjectId = formDemoId,
                    Url = "/",
                    Content = "Xóa: " + formDemoName //Tuyệt đối ghi lịch sử không được lấy Id mà phải là {tên, mã}
                });
                await _formDemoService.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Param: " + formDemoId);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("FindById/{formDemoId}")]
        public async Task<IActionResult> FindById(Guid formDemoId)
        {
            try
            {
                var result = await _formDemoService.FindByIdAsync(formDemoId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Param: " + formDemoId);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("SelectData")]
        public IActionResult SelectData(string? q = "")
        {
            try
            {
                List<FormDemoViewModel> formDemoViewModels = new List<FormDemoViewModel>
                {
                    new FormDemoViewModel() { FormDemoId = new Guid(), Name = "Select 1" },
                    new FormDemoViewModel() { FormDemoId = new Guid(), Name = "Select 2" },
                    new FormDemoViewModel() { FormDemoId = new Guid(), Name = "Select 3" },
                    new FormDemoViewModel() { FormDemoId = new Guid(), Name = "Select 4" },
                    new FormDemoViewModel() { FormDemoId = new Guid(), Name = "Select 5" }
                };
                if (!string.IsNullOrEmpty(q))
                {
                    formDemoViewModels = formDemoViewModels.FindAll(x => x.Name!.ToUpper().Contains(q.ToUpper())).ToList();
                }
                return Ok(formDemoViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return BadRequest(ex.Message);
            }
        }
    }
}