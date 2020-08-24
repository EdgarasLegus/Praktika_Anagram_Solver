using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnagramSolver.Interfaces
{
    public interface IAnagramSolver
    {
        Task<IEnumerable<string>> GetAnagrams(string myWords);
        //IEnumerable<string> GetAnagrams(string myWords);
        int CountChars(string input);
        
        //string SortByAlphabet(string inputWord);
        
        //Dictionary<string, string> MakeDictionary(Dictionary<string, string> dictionary);
        //// DB REPO
        //Dictionary<string, string> MakeDictionary(List<WordModel> wordModel);
        
        //// DB FIRST, CODE FIRST THIS
        //Dictionary<string, string> MakeDictionary(List<WordEntity> wordModel);
    }
}
