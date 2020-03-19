﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WMSA_Project.Models;

namespace WMSA_Project.Models.Factories
{
    internal static class RequestListFactory
    {
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

            filteredUrl = filteredUrl.ReplaceInBounds("[TenantKey]", "/learning/DataStore/", "/Learning/");
            filteredUrl = filteredUrl.ReplaceInBounds("[TenantKey]", "/learning/DataStore/", "/Common/");
            filteredUrl = filteredUrl.ReplaceInBounds("[TimeStamp]", "blank___", ".");
            filteredUrl = filteredUrl.ReplaceInBounds("[UserId]", "gamification/summaries/");
            filteredUrl = filteredUrl.ReplaceInBounds("[CommunityId]", "api/sumtSocial/communities/");
            filteredUrl = filteredUrl.ReplaceInBounds("[CommunityId]", "api/sumtSocial/communities/", "/");
            filteredUrl = filteredUrl.ReplaceInBounds("[CommunityId]", "/communities/", "/blogs");
            filteredUrl = filteredUrl.ReplaceInBounds("[DiscussionId]", "api/social/discuss");
            filteredUrl = filteredUrl.ReplaceInBounds("[TenantKey]", "contentengine/TCAPI/", "/");

            //truncate ending front slash
            if (filteredUrl.LastIndexOf('/') == filteredUrl.Length - 1)
            {
                filteredUrl = filteredUrl.Remove(filteredUrl.Length - 1);
            }

            return filteredUrl;
        }
    }
    //Custom extension methods for checking for different boundaries and replacing internal values if checks are truthy 
    internal static class StringExtension
    {
        public static string ReplaceInBounds(this string url, string replacementVal, string leftBound, string rightBound = null)
        {
            if (!url.Contains(leftBound)) return url;
            if (rightBound != null && !url.Substring(url.IndexOf(leftBound) + leftBound.Length).Contains(rightBound)) return url;


            int leftBoundIndex = url.IndexOf(leftBound) + leftBound.Length;

            if (leftBoundIndex >= url.Length - 1) return url;

            int rightBoundIndex;
            string originalValue;

            if (rightBound != null)
            {
                rightBoundIndex = url.IndexOf(rightBound, leftBoundIndex);
                originalValue = url.Substring(leftBoundIndex, rightBoundIndex - leftBoundIndex);
            }
            else
            {
                originalValue = url.Substring(leftBoundIndex);
            }

            var strBuilder = new StringBuilder(url);
            strBuilder.Replace(originalValue, replacementVal);

            

            url = strBuilder.ToString();
            return url;
        }
    }
}

