namespace WMSA_Project.Models
{
    public class Correlation
    {
        public int Id { get; }
        public string Rule { get; set; }
        public string OriginalValue { get; set; }
        public Correlation() { }
        public Correlation(string rule, string originalValue)
        {
            Rule = rule;
            OriginalValue = originalValue;
        }

        public string GetInfoString()
        {
            return $"{Rule}: {OriginalValue}";
        }
    }
}
