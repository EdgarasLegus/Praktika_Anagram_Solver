using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AnagramSolver.Contracts;
using AnagramSolver.Contracts.Entities;
using AnagramSolver.Interfaces;
using AnagramSolver.Interfaces.DBFirst;
using AnagramSolver.Interfaces.EF;
using AnagramSolver.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.WebApp.Controllers
{
    public class AnagramController : Controller
    {
        private readonly IWordRepository _repository;
        private readonly IAnagramSolver _anagramSolver;
        private readonly IDatabaseLogic _databaseLogic;
        private readonly IEFLogic _eflogic;
        private readonly IEFWordRepo _efWordRepository;

        public AnagramController(IWordRepository repository, IAnagramSolver anagramSolver, IDatabaseLogic databaseLogic, IEFLogic eflogic, IEFWordRepo efWordRepository)
        {
            _repository = repository;
            _anagramSolver = anagramSolver;
            _databaseLogic = databaseLogic;
            _eflogic = eflogic;
            _efWordRepository = efWordRepository;
        }

        public async Task<IActionResult> Index(int? pageIndex, string searchInput, int pageSize = 100)
        {
            try
            {
                //var wordList = _repository.GetWords().AsQueryable();
                //return View(PaginatedList<Anagram>.CreateAsync((IQueryable<Anagram>)wordList, pageIndex ?? 1, pageSize));

                //**//List<WordModel> wordList;
                List<WordEntity> wordList;

                if (!string.IsNullOrEmpty(searchInput))
                {
                    //**//wordList = _databaseLogic.SearchWords(searchInput);
                    //#//wordList = _eflogic.SearchWords(searchInput);
                    wordList = await _efWordRepository.SearchWords(searchInput);
                    pageSize = wordList.Count;
                }
                else
                {
                    //**//wordList = _repository.GetWords();
                    //#//wordList = _efRepository.GetWords();
                    wordList = await _efWordRepository.GetWords();
                }
                //wordList = _repository.GetWords()
                //    .Select(x => new Anagram
                //    {
                //        Word = x.Word,//x.Word,//x.Key,
                //        PartOfSpeech = x.Category//x.Category//x.Value
                //    })
                //    .AsQueryable();

                //var paginatedList = PaginatedList<Anagram>.CreateAsync(wordList, pageIndex ?? 1, pageSize);
                //var paginatedList = PaginatedList<WordModel>.Create((IQueryable<WordModel>)wordList, pageIndex ?? 1, 100);


                //**//var paginatedList = PaginatedList<WordModel>.Create(wordList, pageIndex ?? 1, pageSize);
                var paginatedList = PaginatedList<WordEntity>.Create(wordList, pageIndex ?? 1, pageSize);

                return View(paginatedList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> WordAnagrams(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Error! At least one word must be entered.");

                var anagrams = await _anagramSolver.GetAnagrams(id);


                return View(anagrams);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(id);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}

