using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSA_Project.Models
{
    public class UnmatchedRequest : Request
    {
        public Script UnmatchedSource { get; set; }

        public UnmatchedRequest(Script s):base()
        {
            UnmatchedSource = s;
        }
        public UnmatchedRequest(Script s, RequestVerb verb, string parameters, bool visible): base(verb, parameters, visible)
        {
            UnmatchedSource = s;
        }
    }
}
