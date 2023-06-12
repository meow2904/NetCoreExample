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
    /// Code mẫu FormDemoDetail, Lưu ý mặc định viết theo đúng thứ tự 1.2.3.4 như bên dưới. Nếu bổ sung viết thêm theo đúng thứ tự
    /// </summary>
    public interface IFormDemoDetailService : IDisposable
    {
        /// <summary>
        /// 1. Phân trang mẫu code bất đồng bộ
        /// </summary>
        Task<PagedResult<FormDemoDetailViewModel>> GetAllPagingSortAsync(int page, int pageSize, string search);

        /// <summary>
        /// 2. Thêm sửa xóa
        /// </summary>
        Task<FormDemoDetailViewModel> AddOrUpdateAsync(FormDemoDetailViewModel formDemoDetailViewModel);

        /// <summary>
        /// 3. Bộ chọn Bất đồng bộ
        /// </summary>
        Task<IEnumerable<FormDemoDetailViewModel>> SelectAsync();

        /// <summary>
        /// 4.Tìm kiếm theo Id
        /// </summary>
        Task<FormDemoDetailViewModel> FindByIdAsync(Guid formDemoDetailId);

        /// <summary>
        /// 5.Xóa, trả ra tên bản ghi cần xóa
        /// </summary>
        Task<string> RemoveAsync(Guid formDemoDetailId);

        /// <summary>
        /// 6. SaveAsync
        /// </summary>
        Task SaveAsync();

        Task<List<FormDemoDetailViewModel>> FindByFormDemoIdAsync(Guid formDemoId);
    }

    public class FormDemoDetailService : IFormDemoDetailService
    {
        private readonly IRepository<LogHistory, Guid> _logHistoryRepository;
        private readonly IRepository<AppUser, string> _appUserRepository;
        private readonly IRepository<FormDemoDetail, Guid> _formDemoDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper = new Mapper(AutoMapperConfiguration.Configure());

        public FormDemoDetailService(IRepository<LogHistory, Guid> logHistoryRepository, IRepository<AppUser, string> appUserRepository, IRepository<FormDemoDetail, Guid> formDemoDetailRepository, IUnitOfWork unitOfWork)
        {
            _logHistoryRepository = logHistoryRepository;
            _appUserRepository = appUserRepository;
            _formDemoDetailRepository = formDemoDetailRepository;
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<PagedResult<FormDemoDetailViewModel>> GetAllPagingSortAsync(int page, int pageSize, string search)
        {
            var query = from formDemoDetail in _formDemoDetailRepository.FindAll()
                            //Lưu ý, ta chỉ được lấy ra những trường mà cần sử dụng thôi, nghiêm cấm lấy thừa trường dữ liệu
                        select new FormDemoDetailViewModel()
                        {
                            FormDemoDetailId = formDemoDetail.FormDemoDetailId,
                            Radio = formDemoDetail.Radio,
                            Switch = formDemoDetail.Switch,
                            DateTime = formDemoDetail.DateTime,
                            Name = formDemoDetail.Name,
                        };
            //Nếu có tìm kiếm
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper().Trim();
                query = query.Where(x => (x.Name ?? "").ToUpper().Trim().Contains(search));
            }
            //Trả về dữ liệu phân trang
            return new PagedResult<FormDemoDetailViewModel>
            {
                Results = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),
                CurrentPage = page,
                RowCount = await query.CountAsync(),
                PageSize = pageSize
            };
        }
        public async Task<FormDemoDetailViewModel> AddOrUpdateAsync(FormDemoDetailViewModel formDemoDetailViewModel)
        {
            FormDemoDetail formDemoDetail = new FormDemoDetail();
            if (formDemoDetailViewModel.FormDemoDetailId != Guid.Empty)
            {
                formDemoDetail = await _formDemoDetailRepository.FindByIdAsync(formDemoDetailViewModel.FormDemoDetailId);
                formDemoDetail.UpdateViewModel(formDemoDetailViewModel);
                _formDemoDetailRepository.Update(formDemoDetail);
            }
            else
            {
                formDemoDetail.UpdateViewModel(formDemoDetailViewModel);
                _formDemoDetailRepository.Add(formDemoDetail);
            }
            return formDemoDetailViewModel;
        }

        public async Task<IEnumerable<FormDemoDetailViewModel>> SelectAsync()
        {
            var query = from formDemoDetail in _formDemoDetailRepository.FindAll()
                        select new FormDemoDetailViewModel()
                        {
                            FormDemoDetailId = formDemoDetail.FormDemoDetailId,
                            Name = formDemoDetail.Name,
                        };
            return await query.ToListAsync();
        }

        public async Task<FormDemoDetailViewModel> FindByIdAsync(Guid formDemoDetailId)
        {
            //Lưu ý phải check đoạn này
            if (formDemoDetailId == Guid.Empty) return new FormDemoDetailViewModel();
            return _mapper.Map<FormDemoDetail, FormDemoDetailViewModel>(await _formDemoDetailRepository.FindByIdAsync(formDemoDetailId));
        }

        public async Task<string> RemoveAsync(Guid formDemoDetailId)
        {
            var formDemoDetail = await _formDemoDetailRepository.FindByIdAsync(formDemoDetailId);
            _formDemoDetailRepository.Remove(formDemoDetail);
            return formDemoDetail.Name ?? formDemoDetailId.ToString();
        }

        public async Task SaveAsync()
        {
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<FormDemoDetailViewModel>> FindByFormDemoIdAsync(Guid formDemoId)
        {
            var query = from formDemoDetail in _formDemoDetailRepository.FindAll()
                        where formDemoDetail.FormDemoId == formDemoId
                        select new FormDemoDetailViewModel()
                        {
                            FormDemoDetailId = formDemoDetail.FormDemoDetailId,
                            Name = formDemoDetail.Name,
                            SelectApi = formDemoDetail.SelectApi,
                            SelectSearch = formDemoDetail.SelectSearch,
                            SelectSearchValue = formDemoDetail.SelectSearchValue,
                            DateTime = formDemoDetail.DateTime,
                            Switch = formDemoDetail.Switch,
                            PathFile = formDemoDetail.PathFile,
                        };
            return await query.ToListAsync();
        }
    }
}