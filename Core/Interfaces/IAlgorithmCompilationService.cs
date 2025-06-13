using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlgoMonsterComplete.Core.Interfaces
{
    public interface IAlgorithmCompilationService
    {
        MethodInfo? CompileAlgorithm(string exerciseName, string code);
    }
}
