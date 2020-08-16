using AnagramSolver.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Interfaces.DBFirst
{
    public interface IEFLogic
    {
        string GetIP();
        //List<WordEntity> SearchWords(string searchInput);
        //List<int> GetAnagramsId(IEnumerable<string> anagrams);
        //List<string> GetCachedWords(string searchInput);
        //void InsertCachedWords(string searchInput, List<int> anagramsIdList);
        //void InsertUserLog(string searchInput);
    }
}

