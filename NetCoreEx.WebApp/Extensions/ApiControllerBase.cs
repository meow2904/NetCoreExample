using NetCoreEx.Model.Entities;
using NetCoreEx.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace NetCoreEx.WebApp.Extensions
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApiControllerBase : ControllerBase
    {
        private readonly ILogHistoryService _logHistoryService;

        public ApiControllerBase(ILogHistoryService logHistoryService)
        {
            _logHistoryService = logHistoryService;
        }

        public Guid AddLogHistory(LogHistory logHistory)
        {
            return _logHistoryService.Add(logHistory);
        }

        public LogHistory AddCommitLogHistory(LogHistory logHistory)
        {
            _logHistoryService.Add(logHistory);
            _logHistoryService.SaveAsync();
            return logHistory;
        }

        public async Task AddLogHistoryAsync(LogHistory logHistory)
        {
            _logHistoryService.Add(logHistory);
            await _logHistoryService.SaveAsync();
        }
    }
}