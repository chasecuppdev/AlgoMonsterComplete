namespace AlgoMonsterComplete.Models.Common
{
    public class ComplexityAnalysis
    {
        public string Time { get; set; } = string.Empty;
        public string Space { get; set; } = string.Empty;
        public bool Stable { get; set; }
        public bool InPlace { get; set; }
    }
}
