using AnagramSolver.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Interfaces.EF
{
    public interface IEFWordRepo
    {
        Task <List<WordEntity>> GetWords();
        //Task<List<WordEntity>> GetWords();
        Task<List<WordEntity>> SearchWords(string searchInput);
        List<int> GetAnagramsId(IEnumerable<string> anagrams);
        //List<WordEntity> GetWordEntityFromFile();
        Task InsertWordTableData(List<WordEntity> fileColumns);
        Task InsertAdditionalWord(string word, string category);
        bool CheckIfWordExists(string word);
        void RemoveWord(string word);
        bool UpdateWord(string updatableWord, WordEntity newWord);
    }
}
