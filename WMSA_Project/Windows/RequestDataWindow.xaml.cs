using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WMSA.Entities.Interfaces;

namespace WMSA_Project.Windows
{
    /// <summary>
    /// Interaction logic for RequestDataWindow.xaml
    /// </summary>
    public partial class RequestDataWindow : Window
    {
        public RequestDataWindow(IRequest request)
        {
            InitializeComponent();
            DataContext = this;
            Request = request;
            Correlations = Request.Correlations;
            if (Correlations != null)
            {
                foreach (var c in Correlations) { StkPnl_Corrs.Children.Add(new TextBox() { Text = $"{c.Rule}: {c.OriginalValue}" }); };
            }

        }
        public string RDWTitle { get { return $"{Request.Verb} {Request.URL}"; } }
        public IRequest Request { get; }
        public IEnumerable<ICorrelation> Correlations { get; }
    }
}
