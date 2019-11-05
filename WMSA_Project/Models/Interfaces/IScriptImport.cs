using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSA_Project.Models;

namespace WMSA_Project.Models.Interfaces
{
    public interface IScriptImport
    {
        event EventHandler<ScriptReadyEventArgs> ScriptReady;
        Script GetScript();
    }

    public class ScriptReadyEventArgs : EventArgs
    {
        public string Message{ get; set;}
    }

}
