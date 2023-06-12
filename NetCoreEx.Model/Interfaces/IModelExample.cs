using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreEx.Model.Interfaces
{
    public interface IModelExample
    {
        public int Status { set; get; }
        public string? Number { set; get; }
        public int Count { set; get; }
        DateTime? DateCreated { set; get; }
        DateTime? DateModified { set; get; }
    }
}
