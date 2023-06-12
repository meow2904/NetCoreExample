namespace NetCoreEx.Model.Interfaces
{
    public interface IDateTracking
    {
        DateTime? DateCreated { set; get; }
        DateTime? DateModified { set; get; }
    }
}