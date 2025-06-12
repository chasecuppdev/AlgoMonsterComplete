using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoMonsterComplete.Models.Common
{
    public class Exercise
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Pattern { get; set; }
        public string AlgoMonsterReference { get; set; }
        public Solution MySolution { get; set; }
        public List<TestCase> TestCases { get; set; }
    }
}
