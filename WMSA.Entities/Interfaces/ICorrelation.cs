using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSA.Entities.Interfaces
{
    public interface ICorrelation: IEntity
    {
        string Rule { get; set; }
        string OriginalValue { get; set; }
    }
}
