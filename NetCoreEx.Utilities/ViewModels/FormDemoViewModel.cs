using NetCoreEx.Model.Entities;

namespace NetCoreEx.Utilities.ViewModels
{
    /// <summary>
    /// Lớp này thực hiện ViewModel
    /// Cấu trúc {Tên}ViewModel
    /// </summary>
    public class FormDemoViewModel
    {
        public Guid FormDemoId { get; set; }
        public string? Name { get; set; }
        public string? Textarea { get; set; }
        public int? Radio { get; set; }
        public bool Switch { get; set; }
        public bool CheckBox { get; set; }
        public int SelectSearchId { get; set; }
        public string? SelectSearchValue { get; set; }
        public string? AppUserId { get; set; }
        public AppUserViewModel? AppUserViewModel { get; set; }
        public List<FormDemoDetailViewModel>? FormDemoDetailViewModels { get; set; }
        public DateTime? DateTime { get; set; }
        public int Status { get; set; } = 1;
        public string? Number { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    /// <summary>
    /// Lớp này dùng để Mapper dữ liệu từ ViewModel sang Model
    /// Cấu trúc {Tên}Mapper
    /// </summary>
    public static class FormDemoMapper
    {
        public static void UpdateViewModel(this FormDemo formDemo, FormDemoViewModel formDemoViewModel)
        {
            formDemo.FormDemoId = formDemoViewModel.FormDemoId;
            formDemo.AppUserId = formDemoViewModel.AppUserId;
            formDemo.Name = formDemoViewModel.Name;
            formDemo.Textarea = formDemoViewModel.Textarea;
            formDemo.Radio = formDemoViewModel.Radio;
            formDemo.Switch = formDemoViewModel.Switch;
            formDemo.CheckBox = formDemoViewModel.CheckBox;
            formDemo.DateTime = formDemoViewModel.DateTime;
            formDemo.SelectSearchId = formDemoViewModel.SelectSearchId;
            formDemo.SelectSearchValue = formDemoViewModel.SelectSearchValue;
        }
    }

    /// <summary>
    /// Lớp này dùng để mô tả toàn bộ Const Status
    /// Cấu trúc {Tên}Status
    /// </summary>
    public static class FormDemoStatus
    {
        public const int Created = 1;
        public const int Delete = 2;
        public const int Copy = 3;

        public static string Status(int status)
        {
            if (status == Created) return "Mới tạo";
            if (status == Delete) return "Đã xóa";
            if (status == Copy) return "Copy";
            return "";
        }

        public static string Class(int status)
        {
            if (status == Delete) return "bg-light";
            return "";
        }
    }

    /// <summary>
    /// Lớp này dùng để mô tả toàn bộ Const Type (Nếu có)
    /// Cấu trúc {Tên}Type
    /// </summary>
    public static class FormDemoType
    {
        public const int CheckBox = 1;
        public const int TextInput = 2;
        public const int Datetime = 3;
        public const int ComboBox = 4;

        public static string GetTypeTemp(int? type)
        {
            if (type == 1) return "CheckBox";
            if (type == 2) return "TextInput";
            if (type == 3) return "Datetime";
            if (type == 3) return "ComboBox";
            return "Khác";
        }
    }
}