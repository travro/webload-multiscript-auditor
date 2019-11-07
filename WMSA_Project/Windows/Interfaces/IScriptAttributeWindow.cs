using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSA_Project.Windows.Interfaces
{
    public interface IScriptAttributeWindow
    {
        event EventHandler<ClosedWithAttributeEventArgs> ClosedWithAttribute;
        void Show();
    }

    public class ClosedWithAttributeEventArgs: EventArgs
    {
        public string SelectedValue { get; set; }
    }
}
