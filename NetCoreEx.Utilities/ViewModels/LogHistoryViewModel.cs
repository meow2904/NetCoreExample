using NetCoreEx.Utilities.ViewModels;

namespace NetCoreEx.Utilities.ViewModels
{
    public class LogHistoryViewModel
    {
        public Guid LogHistoryId { set; get; }
        public string? UserId { set; get; }
        public AppUserViewModel? AppUserViewModel { set; get; }
        public string? Content { set; get; }
        public string Url { set; get; } = "/";
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}