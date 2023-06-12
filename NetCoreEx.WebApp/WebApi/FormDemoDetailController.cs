using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreEx.Library.Helper;
using NetCoreEx.Model.Entities;
using NetCoreEx.Service;
using NetCoreEx.Utilities.ViewModels;
using NetCoreEx.WebApp.Extensions;
using Newtonsoft.Json;
using System.Transactions;

namespace NetCoreEx.WebApp.WebApi
{
    public class FormDemoDetailController : ApiControllerBase
    {
        private readonly ILogger<FormDemoDetailController> _logger;
        private readonly ILogHistoryService _logHistoryService;
        private readonly IFormDemoDetailService _formDemoDetailService;
        private readonly IWebHostEnvironment _environment;

        public FormDemoDetailController(ILogger<FormDemoDetailController> logger, ILogHistoryService logHistoryService, IFormDemoDetailService formDemoDetailService, IWebHostEnvironment environment) : base(logHistoryService)
        {
            _logger = logger;
            _logHistoryService = logHistoryService;
            _formDemoDetailService = formDemoDetailService;
            _environment = environment;
        }

        [HttpGet]
        [Route("FindByFormDemoIdAsync/{formDemoId}")]
        public async Task<IActionResult> FindByFormDemoIdAsync(Guid formDemoId)
        {
            try
            {
                var result = await _formDemoDetailService.FindByFormDemoIdAsync(formDemoId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Param: " + formDemoId);
                return BadRequest(ex.Message);
            }
        }
    }
}