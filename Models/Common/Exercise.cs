using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoMonsterComplete.Models.Common
{
    public class Exercise
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Pattern { get; set; } = string.Empty;
        public string AlgoMonsterReference { get; set; } = string.Empty;
        public string InputFormat { get; set; } = string.Empty;
        public Solution MySolution { get; set; } = new();
        public List<TestCase> TestCases { get; set; } = new();
    }
}
