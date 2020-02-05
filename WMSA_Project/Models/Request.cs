using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSA.Entities.Interfaces;

namespace WMSA_Project.Models
{
    public class Request : IRequest
    {
        public int Id { get; set; }
        public RequestVerb Verb { get; set; }
        public string URL { get; set; }
        public bool Visible { get; set; }
        public bool Matched { get; set; }
        public List<Correlation> Correlations { get; set; }
        IEnumerable<ICorrelation> IRequest.Correlations
        {
            get => Correlations;
            set => Correlations = value as List<Correlation>;
        }
        string IRequest.Verb
        {
            get => Verb.ToString();
            set
            {
                RequestVerb v = RequestVerb.MISSING;
                Enum.TryParse(value, out v);
                Verb = v;
            }
        }

        public Request()
        {
            Matched = false;
        }
        public Request(RequestVerb verb, string parameters, bool visible)
        {
            Verb = verb;
            URL = parameters;
            Visible = visible;
            Matched = false;
        }
        public string GetRequestString()
        {
            return Verb.ToString() + " " + URL;
        }
        public bool Equals(Request request)
        {
            return (Verb == request.Verb && URL.Equals(request.URL, System.StringComparison.OrdinalIgnoreCase));
        }
        public string GetInfoString()
        {
            return $"{Verb.ToString()}: {URL}";
        }
    }
}
