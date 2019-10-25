namespace WMSA_Project.Models
{
    public class Correlation
    {
        public int Id { get; }
        public string Rule { get; set; }
        public string OriginalValue { get; set; }
        public string ExtractionLogic { get; set; }
        public Correlation() { }
        public Correlation(string rule, string extractionLogic, string originalValue)
        {
            Rule = rule;
            ExtractionLogic = extractionLogic;
            OriginalValue = originalValue;
        }
    }
}
