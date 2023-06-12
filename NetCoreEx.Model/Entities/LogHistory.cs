using NetCoreEx.Model.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreEx.Model.Entities
{
    [Table("LogHistory")]
    public class LogHistory : IDateTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid LogHistoryId { set; get; }
        public Guid ObjectId { set; get; }
        public string? AppUserId { set; get; }
        public string? Content { set; get; }
        public string Url { set; get; } = "/";
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }
}