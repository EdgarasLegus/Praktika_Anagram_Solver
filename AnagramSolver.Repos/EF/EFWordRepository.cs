﻿using AnagramSolver.Contracts.Entities;
using AnagramSolver.EF.CodeFirst;
using AnagramSolver.EF.DatabaseFirst;
using AnagramSolver.Interfaces.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Repos.EF
{
    public class EFWordRepository : IEFWordRepo
    {
        //private readonly AnagramSolverDBFirstContext _context;
        private readonly AnagramSolverCodeFirstContext _context;

        //public EFRepository(AnagramSolverDBFirstContext context)
        //{
        //    _context = context;
        //}

        public EFWordRepository(AnagramSolverCodeFirstContext context)
        {
            _context = context;
        }

        public Task<List<WordEntity>> GetWords()
        {
            return _context.Word.ToListAsync();
        }

        public Task<List<WordEntity>> SearchWords(string searchInput)
        {
            var wordList = _context.Word.Where(x => x.Word1.Contains(searchInput)).ToListAsync();
            return wordList;
        }

        public List<int> GetAnagramsId(IEnumerable<string> anagrams)
        {
            var anagramList = new List<WordEntity>();
            anagrams = anagrams.ToList();
            //anagramList = _context.Word.Where(x => x.Word1.Equals(anagrams)).ToList();
            anagramList = _context.Word.Where(x => anagrams.Contains(x.Word1)).ToList();
            var anagramIdList = anagramList.Select(x => x.Id).ToList();

            return anagramIdList;
        }

        // CODE FIRST
        public async Task InsertWordTableData(List<WordEntity> fileColumns)
        {
            var enity = new WordEntity();
            foreach (var item in fileColumns)
            {

                var wordEntity = new WordEntity
                {
                    Id = item.Id,
                    Word1 = item.Word1,
                    Category = item.Category
                };
                await _context.Word.AddAsync(wordEntity);
                //_context.SaveChanges();
            }
            //_context.Word.Add(wordEntity);
            await _context.SaveChangesAsync();
        }

        public async Task InsertAdditionalWord(string word, string category)
        {
            var additionalWord = new WordEntity
            {
                Word1 = word,
                Category = category
            };
            await _context.Word.AddAsync(additionalWord);
            await _context.SaveChangesAsync();
        }

        public bool CheckIfWordExists(string word)
        {
            var wordcheck = _context.Word.Where(x => x.Word1 == word).Select(x => x.Word1).Count();
            if (wordcheck > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RemoveWord(string word)
        {
            var wordId = _context.Word.Where(x => x.Word1 == word).Select(x => x.Id).First();
            var wordEntity = new WordEntity
            {
                Id = wordId
            };
            _context.Word.Remove(wordEntity);
            _context.SaveChangesAsync();
        }

        public bool UpdateWord(string updatableWord, WordEntity newWord)
        {
            _context.Word.Where(x => x.Word1 == updatableWord).ToList().ForEach(x => x.Word1 = newWord.Word1);
            _context.Word.Where(x => x.Word1 == updatableWord).ToList().ForEach(x => x.Category = newWord.Category);
            //var existingWord = _context.Word.Where(x => x.Word1 == newWord.Word1).ToList().First();
            var checkExistingWord = CheckIfWordExists(newWord.Word1);
            if(checkExistingWord == true)
            {
                return true;
            }
            else
            {
                _context.SaveChanges();
                return false;
            }
            //_context.SaveChanges();
        }

        //public List<WordEntity> GetWordEntityFromFile()
        //{
        //    var configuration = new ConfigurationBuilder()
        //       .AddJsonFile(@"./appsettings.json")
        //       .Build();

        //    var path = configuration["Settings:FileName"];

        //    if (!File.Exists(path))
        //    {
        //        throw new Exception($"Data file {path} does not exist!");
        //    }

        //    var wordModelList = new Dictionary<string, string>();

        //    using (StreamReader reader = new StreamReader(path))
        //    {
        //        string line;
        //        while ((line = reader.ReadLine()) != null)
        //        {
        //            string word = line.Split('\t').First();
        //            string PartOfSpeech = line.Split('\t').ElementAt(1);
        //            var wordModel = new WordEntity()
        //            {
        //                Word1 = word,
        //                Category = PartOfSpeech
        //            };
        //            if (!wordModelList.ContainsKey(word))
        //            {
        //                wordModelList.Add(wordModel.Word1, wordModel.Category);
        //            }
        //        }
        //    }
        //    var returnList = wordModelList.Select(pair => new WordEntity()
        //    {
        //        Word1 = pair.Key,
        //        Category = pair.Value
        //    }).ToList();
        //    return returnList;

        //}
    }
}

