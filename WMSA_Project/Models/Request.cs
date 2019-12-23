﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSA_Project.Models
{
    public class Request
    {
        public int Id { get;  }
        public RequestVerb Verb { get; set; }
        public string Parameters { get; set; }
        public bool Visible { get; set; }
        public List<Correlation> Correlations { get; set; }

        public Request()
        { 
        }

        public Request(RequestVerb verb, string parameters, bool visible)
        {
            Verb = verb;
            Parameters = parameters;
            Visible = visible;
        }

        public string GetRequestString()
        {
            return Verb.ToString() + " " + Parameters;
        }
        public bool Equals(Request request)
        {
            return (Verb == request.Verb && Parameters.Equals(request.Parameters, System.StringComparison.OrdinalIgnoreCase));
        }

        public string GetInfoString()
        {
            return $"{Verb.ToString()}: {Parameters}";
        }
    }
}
