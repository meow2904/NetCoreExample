using NetCoreEx.Model.Entities;

namespace NetCoreEx.Utilities.ViewModels
{
    public class FormDemoDetailViewModel
    {
        public Guid FormDemoDetailId { get; set; }
        public Guid FormDemoId { get; set; }
        public string? Name { get; set; }
        public int? Radio { get; set; }
        public bool? Switch { get; set; }
        public bool? CheckBox { get; set; }
        public int? SelectSearch { get; set; }
        public int? SelectApi { get; set; }
        public string? SelectSearchValue { get; set; }
        public string? PathFile { get; set; }
        public DateTime? DateTime { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }

    public static class FormDemoDetailMapper
    {
        public static void UpdateViewModel(this FormDemoDetail formDemoDetail, FormDemoDetailViewModel formDemoDetailViewModel)
        {
            formDemoDetail.FormDemoDetailId = formDemoDetailViewModel.FormDemoDetailId;
            formDemoDetail.FormDemoId = formDemoDetailViewModel.FormDemoId;
            formDemoDetail.Name = formDemoDetailViewModel.Name;
            formDemoDetail.Radio = formDemoDetailViewModel.Radio;
            formDemoDetail.PathFile = formDemoDetailViewModel.PathFile;
            formDemoDetail.Switch = formDemoDetailViewModel.Switch;
            formDemoDetail.CheckBox = formDemoDetailViewModel.CheckBox;
            formDemoDetail.DateTime = formDemoDetailViewModel.DateTime;
            formDemoDetail.SelectSearch = formDemoDetailViewModel.SelectSearch;
            formDemoDetail.SelectApi = formDemoDetailViewModel.SelectApi;
            formDemoDetail.SelectSearchValue = formDemoDetailViewModel.SelectSearchValue;
        }
    }
}