using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSA_Project.Models;

namespace WMSA_Project.Controls.Interfaces
{
    public interface IScriptImportControl
    {
        event EventHandler<ScriptReadyEventArgs> ScriptReady;
        Script GetScript();
    }

    public class ScriptReadyEventArgs : EventArgs
    {
        public string Message{ get; set;}
    }

}
