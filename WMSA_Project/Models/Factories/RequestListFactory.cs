using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using WMSA_Project.Models;

namespace WMSA_Project.Models.Factories
{
    internal static class RequestListFactory
    {
        private static readonly string[][] _boundRules = new string[][]
        {
            new[]{ "[TenantKey]", "/learning/DataStore/", "/" },
            new[]{ "[TimeStamp]", "blank___", "."},
            new[]{ "[CommunityId]", "/communities/", "/"},
            new[]{ "[TenantKey]", "contentengine/TCAPI/", "/"},
            new[]{ "[UserId]", "gamification/summaries/", null},
            new[]{ "[CommunityId]", "/communities/", null},
            new[]{ "[DiscussionId]", "api/social/discuss/", null},
        };
        public static List<Request> GetRequestsFromXElement(XElement xElement, bool addNonVisibles = true)
        {
            var requests = new List<Request>();

            var jsChildBlockElements = xElement.Elements("JavaScriptObject");

            foreach (var jsCBE in jsChildBlockElements)
            {
                //jsCBE descendant elements that contain the request strings
                IEnumerable<XElement> httpHeaderElements = jsCBE.Descendants("PropertyPage")
                    .Where(desc => desc.Attribute("Name").Value == "HTTPHeaders")
                    .Elements() ?? new List<XElement>();

                //jsCBE descendant elements that contain the correlation strings if any
                XElement nodeScriptEl = jsCBE.Descendants("PropertyPage")
                    .Where(desc => desc.Attribute("Name").Value == "JavaScript")
                    .First()
                    .Element("NodeScript") ?? new XElement("NodeScript");

                //the first request value in the string is the only visible value in the Webload IDE
                foreach (var httpEl in httpHeaderElements)
                {
                    if (httpEl == httpHeaderElements.First() && nodeScriptEl.Value.Contains("setCorr"))
                    {
                        requests.Add(new Request()
                        {
                            Verb = ParseRequestVerb(httpEl),
                            URL = ParseRequestUrl(httpEl),
                            Visible = true,
                            Correlations = CorrelationListFactory.GetCorrelationsFromXElement(nodeScriptEl)
                        });
                    }
                    else if (httpEl == httpHeaderElements.First())
                    {
                        requests.Add(new Request()
                        {
                            Verb = ParseRequestVerb(httpEl),
                            URL = ParseRequestUrl(httpEl),
                            Visible = true,
                        });
                    }
                    else
                    {
                        if (addNonVisibles)
                        {
                            requests.Add(new Request()
                            {
                                Verb = ParseRequestVerb(httpEl),
                                URL = ParseRequestUrl(httpEl),
                                Visible = false
                            });
                        }
                    }
                }
            }
            return requests;
        }
        private static RequestVerb ParseRequestVerb(XElement xElement)
        {
            string line = xElement.Attribute("Text").Value;
            if (line.StartsWith(" CONNECT")) return RequestVerb.CONNECT;
            if (line.StartsWith(" DELETE")) return RequestVerb.DELETE;
            if (line.StartsWith(" POST")) return RequestVerb.POST;
            if (line.StartsWith(" PUT")) return RequestVerb.PUT;
            if (line.StartsWith(" GET")) return RequestVerb.GET;
            return RequestVerb.GET;
        }
        private static string ParseRequestUrl(XElement xElement)
        {
            string line = xElement.Attribute("Text").Value;

            //initially filter out domain and session data
            string sumTotalSite = "sumtotaldevelopment.net";
            string domain = (line.Contains(sumTotalSite)) ? sumTotalSite : "https://";
            int domainLastIndex = line.IndexOf(domain) + domain.Length;
            int paramIndex = line.IndexOfAny(new[] { '?', '%', ' ' }, domainLastIndex);
            string filteredUrl = line.Substring(domainLastIndex, paramIndex - domainLastIndex);

            //filter dynamic values
            foreach (string[] rule in _boundRules)
            {
                if (!filteredUrl.Contains(rule[1])) continue;
                if (rule[2] != null && !filteredUrl.Substring(filteredUrl.IndexOf(rule[1]) + rule[1].Length).Contains(rule[2])) continue;

                int lBIndex = filteredUrl.IndexOf(rule[1]) + rule[1].Length;

                if (lBIndex >= filteredUrl.Length - 1) break;

                int rBIndex; 
                string targetValue;

                if(rule[2] != null)
                {
                    rBIndex = filteredUrl.IndexOf(rule[2], lBIndex);
                    targetValue = filteredUrl.Substring(lBIndex, rBIndex - lBIndex);
                }
                else
                {
                    targetValue = filteredUrl.Substring(lBIndex);
                    if (targetValue.Contains("/"))
                    {
                        int slashIndex = targetValue.IndexOf('/');
                        targetValue = targetValue.Substring(0, slashIndex);
                    }
                }
                var strBuilder = new StringBuilder(filteredUrl);

                strBuilder.Replace(targetValue, rule[0]);
                filteredUrl = strBuilder.ToString();
            }

            //truncate ending front slash
            if (filteredUrl.LastIndexOf('/') == filteredUrl.Length - 1)
            {
                filteredUrl = filteredUrl.Remove(filteredUrl.Length - 1);
            }
            return filteredUrl;
        }
    }
}