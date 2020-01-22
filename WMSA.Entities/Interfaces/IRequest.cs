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
        string Parameters { get; set; }
        ICollection<ICorrelation> Correlations { get; set; }
    }
}
