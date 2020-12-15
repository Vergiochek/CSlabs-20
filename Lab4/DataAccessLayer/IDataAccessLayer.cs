using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseModels.Models;

namespace DataAccessLayer
{
    public interface IFiller<T>
    {
        public SearchRes<HumanData> GetPersons();
    }
}
