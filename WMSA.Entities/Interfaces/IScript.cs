using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSA.Entities.Interfaces
{
    public interface IScript : IEntity
    {
        string TestName { get; set; }
        string BuildVersion { get; set; }
        string Name { get; set; }
        DateTime RecordedDate { get; set; }        
        ICollection<ITransaction> Transactions { get; set; }
    }
}
