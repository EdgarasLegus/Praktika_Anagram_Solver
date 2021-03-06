﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Entities;
using AnagramSolver.EF.CodeFirst;
using AnagramSolver.EF.DatabaseFirst;
using AnagramSolver.Interfaces;
using AnagramSolver.Interfaces.DBFirst;
using AnagramSolver.Interfaces.EF;
using AnagramSolver.Repos;
using AnagramSolver.Repos.EF;

namespace AnagramSolver.BusinessLogic
{
    public class AnagramSolver : IAnagramSolver
    {
        // TESTAS 
        // ar gerai nuskaitomas failas

        private Dictionary<string, string> _createdDictionary;
        //private readonly AnagramSolverDBFirstContext _context;
        private readonly AnagramSolverCodeFirstContext _context;
        private readonly IEFWordRepo _efWordRepository;
        private readonly IWordService _wordService;

        public IWordRepository FRepository { get; set; }
        public IWordRepository DBRepository { get; set; }
        public IEFWordRepo EFWordRepo { get; set; }

        public AnagramSolver(IEFWordRepo eFWordRepository, AnagramSolverCodeFirstContext context, IWordService wordservice)
        {
            //var cont = new AnagramSolverDBFirstContext();
            //.//var cont = new AnagramSolverCodeFirstContext();

            //var repository = new FRepository();
            //var repository = new DBRepository();
            //var repository = new EFRepository(cont);
            //.//var repository = new EFWordRepository(cont);

            _context = context;
            _wordService = wordservice;
     
            //.//var wordService = new WordService();

            _efWordRepository = eFWordRepository;
            if (!_context.Word.Any())
            {
                //repository.InsertWordTableData(repository.GetWordEntityFromFile());
                _efWordRepository.InsertWordTableData(_wordService.GetWordEntityFromFile());
            }

            // 1 zingsnis --- Gauname failo pirmuosius 2 stulpelius
            //var fileColumns = await repository.GetWords();

            //_createdDictionary = MakeDictionary(fileColumns);
        }

        public async Task<IEnumerable<string>> GetAnagrams(string myWords)
        {
            // 1 - Failo pirmieji 2 stulpeliai
            // Kiekviena karta ne kolint idet 
            //Dictionary<string, string> fileColumns = FRepository.GetWords();
            var fileColumns = await _efWordRepository.GetWords();
            // 2 - Sudarytas žodynas
            //Dictionary<string, string> createdDictionary = MakeDictionary(fileColumns);
            _createdDictionary = MakeDictionary(fileColumns);
            // 3 - Išrušiuotas įvesties žodis
            var mySortedInputWord = SortByAlphabet(myWords);

            // 4 - Anagramų sudarymas
            var anagrams = _createdDictionary
                .Where(kvp => kvp.Value.Equals(mySortedInputWord))
                .Select(kvp => kvp.Key);

            return anagrams;
        }


        //buvo public
        private string SortByAlphabet(string inputWord)
        {
            char[] convertedToChar =  inputWord.ToCharArray();
            Array.Sort(convertedToChar);

            return new string(convertedToChar);
        }

        //**// public Dictionary<string, string> MakeDictionary(List<WordModel> wordModel)
        private Dictionary<string, string> MakeDictionary(List<WordEntity> wordModel)
        {
            var wordModelDictionary = wordModel.ToDictionary(x => x.Word1, x => x.Category);
            var sortedDictionary = wordModelDictionary.ToDictionary(x => x.Key, y => SortByAlphabet(y.Key));
            return sortedDictionary;
        }

        public int CountChars(string input)
        {
            char[] characters = input.ToCharArray();
            int charCount = input.Count(c => !Char.IsWhiteSpace(c));
            return charCount;
        }

        //public static string GetIP()
        //{
        //    var host = Dns.GetHostEntry(Dns.GetHostName());
        //    foreach (var ip in host.AddressList)
        //    {
        //        if (ip.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            return ip.ToString();
        //        }
        //    }
        //    throw new Exception("IP is not recognised!");
        //}

        //222222 ----------------------
        //public Dictionary<string, string> MakeDictionary(Dictionary<string, string> dictionary)
        //{
        //    //Dictionary<string, string> data = new Dictionary<string, string>();

        //    //List<string> firstColumn = dictionary.Keys.ToList();

        //    //string sortedPart;
        //    //List<string> sortedWords = new List<string>();
        //    //for (int i = 0; i < firstColumn.Count; i++)
        //    //{

        //    //    sortedPart = SortByAlphabet(firstColumn[i]);
        //    //    sortedWords.Add(sortedPart);
        //    //}

        //    ////Žodyno sudarymas
        //    //Dictionary<string, string> myDictionary = new Dictionary<string, string>();

        //    //for (int i = 0; i < firstColumn.Count; i++)
        //    //{
        //    //    if (myDictionary.ContainsKey(firstColumn[i]))
        //    //    {
        //    //        myDictionary[firstColumn[i]] = sortedWords[i];
        //    //    }
        //    //    else
        //    //    {
        //    //        myDictionary.Add(firstColumn[i], sortedWords[i]);
        //    //    }
        //    //}
        //    //return myDictionary;

        //    var sortedDictionary = dictionary.ToDictionary(x => x.Key, y => SortByAlphabet(y.Key));
        //    return sortedDictionary;
        //}

    }
}

