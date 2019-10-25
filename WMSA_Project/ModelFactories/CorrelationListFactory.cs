using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMSA_Project.Models;
using System.Xml.Linq;

namespace WMSA_Project.ModelFactories
{
    public static class CorrelationListFactory
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
                            ExtractionLogic = corArgs[1],
                            OriginalValue = corArgs[2]
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
            int secondParamStartingIndex = 0;
            int secondParamEndingIndex = 0;
            List<string> argList = new List<string>();
            string[] argArray = new string[] { "--no argument captured--", "--no argument captured--", "--no argument captured--" };
            StringBuilder buildingString = new StringBuilder();


            //get the first correlation argument
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
                        secondParamStartingIndex = i;
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

            //get the third
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
                        secondParamEndingIndex = j;
                        argArray[2] = buildingString.ToString();
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


            argArray[1] = line.Substring(secondParamStartingIndex, secondParamEndingIndex - secondParamStartingIndex);

            return argArray;
        }
    }
}
