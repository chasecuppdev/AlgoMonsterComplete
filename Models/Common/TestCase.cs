namespace AlgoMonsterComplete.Models.Common
{
    public class TestCase
    {
        public string Input { get; set; } = string.Empty;
        public string Expected { get; set; } = string.Empty;
        public List<string> ExpectedOptions { get; set; } = new();
        public string Notes { get; set; } = string.Empty;
    }
}
