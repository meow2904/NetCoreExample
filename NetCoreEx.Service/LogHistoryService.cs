using Microsoft.EntityFrameworkCore;
using NetCoreEx.Model.Entities;
using NetCoreEx.Model.Infrastructure;
using NetCoreEx.Utilities;
using NetCoreEx.Utilities.ViewModels;

namespace NetCoreEx.Service
{
    public interface ILogHistoryService
    {
        Task<PagedResult<LogHistoryViewModel>> GetAllPagingSortAsync(int page, int pageSize, string search, string userId);

        Guid Add(LogHistory logHistory);

        Task SaveAsync();
    }

    public class LogHistoryService : ILogHistoryService
    {
        private readonly IRepository<LogHistory, Guid> _logHistoryRepository;
        private readonly IRepository<AppUser, string> _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LogHistoryService(IRepository<LogHistory, Guid> logHistoryRepository, IUnitOfWork unitOfWork, IRepository<AppUser, string> employeeRepository)
        {
            _logHistoryRepository = logHistoryRepository;
            _unitOfWork = unitOfWork;
            _employeeRepository = employeeRepository;
        }

        public async Task<PagedResult<LogHistoryViewModel>> GetAllPagingSortAsync(int page, int pageSize, string search, string userId)
        {
            var query = from logHistory in _logHistoryRepository.FindAll()
                        join employee in _employeeRepository.FindAll() on logHistory.AppUserId equals employee.Id
                        select new LogHistoryViewModel
                        {
                            Content = logHistory.Content ?? "",
                            UserId = logHistory.AppUserId,
                            LogHistoryId = logHistory.LogHistoryId,
                            DateCreated = logHistory.DateCreated,
                            DateModified = logHistory.DateModified,
                            AppUserViewModel = new AppUserViewModel
                            {
                                UserName = employee.UserName,
                            }
                        };
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToUpper();
                query = query.Where(x => (x.Content ?? "").ToUpper().Contains(search));
            }
            if (string.IsNullOrEmpty(userId))
            {
                query = query.Where(x => x.UserId == userId);
            }
            query = query.OrderByDescending(x => x.DateModified);
            return new PagedResult<LogHistoryViewModel>
            {
                Results = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),
                CurrentPage = page,
                RowCount = await query.CountAsync(),
                PageSize = pageSize,
            };
        }

        public async Task SaveAsync()
        {
            await _unitOfWork.SaveAsync();
        }

        public Guid Add(LogHistory logHistory)
        {
            return _logHistoryRepository.Add(logHistory).LogHistoryId;
        }
    }
}