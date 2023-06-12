using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetCoreEx.Model.Entities;
using NetCoreEx.Model.Infrastructure;
using NetCoreEx.Service.AutoMapper;
using NetCoreEx.Utilities;
using NetCoreEx.Utilities.ViewModels;

namespace NetCoreEx.Service
{
    /// <summary>
    /// Code mẫu FormDemo, Lưu ý mặc định viết theo đúng thứ tự 1.2.3.4 như bên dưới. Nếu bổ sung viết thêm theo đúng thứ tự
    /// </summary>
    public interface IFormDemoService : IDisposable
    {
        /// <summary>
        /// 1. Phân trang mẫu code bất đồng bộ
        /// </summary>
        Task<PagedResult<FormDemoViewModel>> GetAllPagingSortAsync(int page, int pageSize, string search);

        /// <summary>
        /// 2. Thêm sửa xóa
        /// </summary>
        Task<FormDemoViewModel> AddOrUpdateAsync(FormDemoViewModel formDemoViewModel);

        /// <summary>
        /// 3. Bộ chọn Bất đồng bộ
        /// </summary>
        Task<IEnumerable<FormDemoViewModel>> SelectAsync();

        /// <summary>
        /// 4.Tìm kiếm theo Id
        /// </summary>
        Task<FormDemoViewModel> FindByIdAsync(Guid formDemoId);

        /// <summary>
        /// 5.Xóa, trả ra tên bản ghi cần xóa
        /// </summary>
        Task<string> RemoveAsync(Guid formDemoId);

        /// <summary>
        /// 6. SaveAsync
        /// </summary>
        Task SaveAsync();
    }

    public class FormDemoService : IFormDemoService
    {
        private readonly IRepository<LogHistory, Guid> _logHistoryRepository;
        private readonly IRepository<AppUser, string> _appUserRepository;
        private readonly IRepository<FormDemo, Guid> _formDemoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper = new Mapper(AutoMapperConfiguration.Configure());

        public FormDemoService(IRepository<LogHistory, Guid> logHistoryRepository, IRepository<AppUser, string> appUserRepository, IRepository<FormDemo, Guid> formDemoRepository, IUnitOfWork unitOfWork)
        {
            _logHistoryRepository = logHistoryRepository;
            _appUserRepository = appUserRepository;
            _formDemoRepository = formDemoRepository;
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<PagedResult<FormDemoViewModel>> GetAllPagingSortAsync(int page, int pageSize, string search)
        {
            var query = from formDemo in _formDemoRepository.FindAll()
                        join appUser in _appUserRepository.FindAll() on formDemo.AppUserId equals appUser.Id
                        //Lưu ý, ta chỉ được lấy ra những trường mà cần sử dụng thôi, nghiêm cấm lấy thừa trường dữ liệu
                        select new FormDemoViewModel()
                        {
                            FormDemoId = formDemo.FormDemoId,
                            DateTime = formDemo.DateTime,
                              Name = formDemo.Name,
                            AppUserViewModel = new AppUserViewModel()
                            {
                                Id = appUser.Id,
                                UserName = appUser.UserName,
                                Avatar = appUser.Avatar ?? "~/images/avatar.png",
                                FullName = appUser.FullName ?? ""
                            }
                        };
            //Nếu có tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().Trim();
                query = query.Where(x => (x.Name ?? "").ToUpper().Trim().Contains(search)
                                         || (x.AppUserViewModel!.UserName ?? "").ToUpper().Trim().Contains(search));
            }
            //Trả về dữ liệu phân trang
            return new PagedResult<FormDemoViewModel>
            {
                Results = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),
                CurrentPage = page,
                RowCount = await query.CountAsync(),
                PageSize = pageSize
            };
        }

        public async Task<FormDemoViewModel> AddOrUpdateAsync(FormDemoViewModel formDemoViewModel)
        {
            FormDemo formDemo = new FormDemo();
            if (formDemoViewModel.FormDemoId != Guid.Empty)
            {
                formDemo = await _formDemoRepository.FindByIdAsync(formDemoViewModel.FormDemoId);
                formDemo.UpdateViewModel(formDemoViewModel);
                _formDemoRepository.Update(formDemo);
            }
            else
            {
                formDemo.UpdateViewModel(formDemoViewModel);
                formDemo.Count = ((await _formDemoRepository.FindAll().OrderByDescending(x => x.Count).FirstOrDefaultAsync())?.Count ?? 0) + 1;
                formDemo.Number = "CODEMAU-" + formDemo.Count.ToString("0000");
                _formDemoRepository.Add(formDemo);
                formDemoViewModel.FormDemoId = formDemo.FormDemoId;
                formDemoViewModel.Number = formDemo.Number;
            }
            return formDemoViewModel;
        }

        public async Task<IEnumerable<FormDemoViewModel>> SelectAsync()
        {
            var query = from formDemo in _formDemoRepository.FindAll()
                        select new FormDemoViewModel()
                        {
                            FormDemoId = formDemo.FormDemoId,
                            Name = formDemo.Name,
                        };
            return await query.ToListAsync();
        }

        public async Task<FormDemoViewModel> FindByIdAsync(Guid formDemoId)
        {
            //Lưu ý phải check đoạn này
            if (formDemoId == Guid.Empty) return new FormDemoViewModel();
            return _mapper.Map<FormDemo, FormDemoViewModel>(await _formDemoRepository.FindByIdAsync(formDemoId));
        }
        /// <summary>
        /// Luôn return ra đối tượng
        /// </summary>
        public async Task<string> RemoveAsync(Guid formDemoId)
        {
            var formDemo = await _formDemoRepository.FindByIdAsync(formDemoId);
            _formDemoRepository.Remove(formDemo);
            return formDemo.Name ?? formDemoId.ToString();
        }

        public async Task SaveAsync()
        {
            await _unitOfWork.SaveAsync();
        }
    }
}