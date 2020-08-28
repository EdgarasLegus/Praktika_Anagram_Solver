using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Interfaces.EF
{
    public interface IEFCachedWordRepo
    {
        Task<List<string>> GetCachedWords(string searchInput);
        Task InsertCachedWords(string searchInput, List<int> anagramsIdList);
    }
}
