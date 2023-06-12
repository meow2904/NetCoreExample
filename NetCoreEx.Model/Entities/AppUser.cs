
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using NetCoreEx.Model.Interfaces;

namespace NetCoreEx.Model.Entities
{
    [Table("AppUser")]
    public class AppUser : IdentityUser, IDateTracking
    {
        public string? Avatar { get; set; }
        public string? FullName { get; set; }
        public int Status { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }
}