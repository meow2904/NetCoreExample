using NetCoreEx.Model.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreEx.Model.Entities
{
    [Table("FormDemo")]
    public class FormDemo : IModelExample
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FormDemoId { get; set; }
        public string? Name { get; set; }
        public string? Textarea { get; set; }
        public int? Radio { get; set; }
        public bool Switch { get; set; }
        public bool CheckBox { get; set; }
        public int SelectSearchId { get; set; }
        public string? SelectSearchValue { get; set; }
        public string? AppUserId { get; set; }
        public DateTime? DateTime { get; set; }
        public int Status { get; set; } = 1;
        public string? Number { get; set; }
        public int Count { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}