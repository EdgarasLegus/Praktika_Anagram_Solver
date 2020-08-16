using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Interfaces.EF
{
    public interface IEFCachedWordRepo
    {
        List<string> GetCachedWords(string searchInput);
        void InsertCachedWords(string searchInput, List<int> anagramsIdList);
    }
}
