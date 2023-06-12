using NetCoreEx.Model.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreEx.Model.Entities
{
    [Table("FormDemoDetail")]
    public class FormDemoDetail : IDateTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FormDemoDetailId { get; set; }
        public Guid FormDemoId { get; set; }
        public string? Name { get; set; }
        public int? Radio { get; set; }
        public bool? Switch { get; set; }
        public bool? CheckBox { get; set; }
        public int? SelectApi { get; set; }
        public int? SelectSearch { get; set; }
        public string? SelectSearchValue { get; set; }
        public string? PathFile { get; set; }
        public DateTime? DateTime { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}