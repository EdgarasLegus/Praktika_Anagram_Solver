using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Interfaces
{
    public interface IDatabaseLogic
    {
        List<WordModel> SearchWords(string searchInput);
        List<string> GetCachedWords(string searchInput);

        void InsertCachedWords(string searchInput, List<int> anagramsIdList);

        List<int> GetAnagramsId(IEnumerable<string> anagrams);

        //List<string> PseudoCaching(string searchInput, List<int> anagramsIdList);
    }
}
