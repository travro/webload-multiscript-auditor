using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WMSA_Project.Models
{
    public class UnmatchedRequest : Request
    {
        public Script UnmatchedSource { get; }
        public SolidColorBrush SourceColor { get; }

        public UnmatchedRequest(Script s, SolidColorBrush color):base()
        {
            UnmatchedSource = s;
            SourceColor = color;
        }
        public UnmatchedRequest(Script s, SolidColorBrush color, RequestVerb verb, string parameters, bool visible): base(verb, parameters, visible)
        {
            UnmatchedSource = s;
            SourceColor = color;
        }
    }
}
