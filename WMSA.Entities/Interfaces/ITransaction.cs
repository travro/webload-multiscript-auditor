using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSA.Entities.Interfaces
{
    public interface ITransaction : IEntity
    {
        string Name { get; set; }
        string Sleep { get; set; }
        IEnumerable<IRequest> Requests { get; set; }
    }
}
