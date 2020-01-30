using WMSA.Entities.Interfaces;

namespace WMSA_Project.Models
{
    public class Correlation : ICorrelation
    {
        public int Id { get; set; }
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
