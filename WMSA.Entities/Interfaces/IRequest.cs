using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSA.Entities.Interfaces
{
    public interface IRequest : IEntity
    {
        string Verb { get; set; }
        string URL { get; set; }
        IEnumerable<ICorrelation> Correlations { get; set; }
    }
}
