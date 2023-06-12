using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreEx.Model.Entities
{
    [Table("AppRoles")]
    public class AppRole : IdentityRole
    {
        public AppRole(string name, string description) : base(name)
        {
            this.Description = description;
        }

        [StringLength(250)]
        public string Description { get; set; }
    }
}