namespace NetCoreEx.Utilities.ViewModels
{
    public class AppUserViewModel
    {
        public string Id { get; set; }
        public string Avatar { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int Status { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; } = DateTime.Now;

    }

    public class LoginViewModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
    public class ChangePasswordViewModel
    {
        public string? PasswordOld { set; get; }
        public string? PasswordNew { set; get; }
    }
    public class EmployeeRoleViewModel
	{
        public string? RoleName { set; get; }
		public int EmployeeId { get; set; }
	}
    public class DataViewModel
    {
        public string? Id { set; get; }

        public string? Value { set; get; }
    }
}