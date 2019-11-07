using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSA_Project.Models;
using System.Xml.Linq;

namespace WMSA_Project.Models.Factories
{
    internal static class CorrelationListFactory
    {
        public static List<Correlation> GetCorrelationsFromXElement(XElement xElement)
        {
            var correlations = new List<Correlation>();
            string[] corArgs;

            using (System.IO.StringReader reader = new System.IO.StringReader(xElement.Value))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null) break;
                    if (line.Contains("setCorrelation"))
                    {
                        corArgs = Parse(line);
                        correlations.Add(new Correlation()
                        {                            
                            Rule = corArgs[0],
                            OriginalValue = corArgs[1]
                        });
                    }
                }
            }
            return correlations;
        }
        private static string[] Parse(string line)
        {
            //Capture outermost parenthesis
            Stack<char> quoteStack = new Stack<char>();
            List<string> argList = new List<string>();
            string[] argArray = new string[] { "--no argument captured--", "--no argument captured--" };
            StringBuilder buildingString = new StringBuilder();


            //get rule
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '\"')
                {
                    if (quoteStack.Count == 0)
                    {
                        quoteStack.Push(line[i]);
                        continue;
                    }
                    else
                    {
                        quoteStack.Pop();
                        argArray[0] = buildingString.ToString();
                        buildingString.Clear();
                        break;
                    }
                }
                else
                {
                    if (quoteStack.Count > 0)
                    {
                        buildingString.Append(line[i]);
                    }
                }
            }

            //get original value
            for (int j = line.Length - 1; j >= 0; j--)
            {
                if (line[j] == '\"')
                {
                    if (quoteStack.Count == 0)
                    {
                        quoteStack.Push(line[j]);
                        continue;
                    }
                    else
                    {
                        quoteStack.Pop();
                        argArray[1] = buildingString.ToString();
                        buildingString.Clear();
                        break;
                    }
                }
                else
                {
                    if (quoteStack.Count > 0)
                    {
                        buildingString.Insert(0, line[j]);
                    }
                }
            }
            return argArray;
        }
    }
}
