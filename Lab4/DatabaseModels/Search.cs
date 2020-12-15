using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseModels.Models
{
    public class Search<TEntity> where TEntity : new()
    {
        public   IEnumerable<TEntity> Entities { get; set; }
        public int Total { get; set; }
    }
}
