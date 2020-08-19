using AnagramSolver.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Interfaces.EF
{
    public interface IEFWordRepo
    {
        List<WordEntity> GetWords();
        List<WordEntity> SearchWords(string searchInput);
        List<int> GetAnagramsId(IEnumerable<string> anagrams);
        //List<WordEntity> GetWordEntityFromFile();
        void InsertWordTableData(List<WordEntity> fileColumns);
        void InsertAdditionalWord(string word, string category);
        bool CheckIfWordExists(string word);
        void RemoveWord(string word);
        void UpdateWord(string existingWord, WordEntity newWord);
    }
}
