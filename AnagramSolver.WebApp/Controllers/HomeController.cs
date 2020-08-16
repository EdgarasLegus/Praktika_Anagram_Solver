using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AnagramSolver.WebApp.Models;
using System.Text.Encodings.Web;
using AnagramSolver.Interfaces;
using Microsoft.Extensions.Configuration;
using AnagramSolver.UI;
using AnagramSolver.Interfaces.DBFirst;
using AnagramSolver.EF.CodeFirst;
using AnagramSolver.Interfaces.EF;
using System.Net;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using AnagramSolver.Contracts.Enums;
using AnagramSolver.Contracts.Entities;

namespace AnagramSolver.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnagramSolver _anagramSolver;
        private readonly IDatabaseLogic _databaseLogic;
        private readonly IEFLogic _eflogic;

        private readonly IEFWordRepo _efWordRepository;
        private readonly IEFUserLogRepo _efUserLogRepository;
        private readonly IEFCachedWordRepo _efCachedWordRepository;

        public HomeController(IAnagramSolver anagramSolver, IDatabaseLogic databaseLogic, IEFLogic efLogic,
            IEFWordRepo efWordRepository, IEFUserLogRepo efUserLogRepository, IEFCachedWordRepo efCachedWordRepository)
        {
            _anagramSolver = anagramSolver;
            _databaseLogic = databaseLogic;
            _eflogic = efLogic;

            _efWordRepository = efWordRepository;
            _efUserLogRepository = efUserLogRepository;
            _efCachedWordRepository = efCachedWordRepository;
        }

        public IActionResult Index(string word)
        {
            try
            {
                if (string.IsNullOrEmpty(word))
                    throw new Exception("Error! At least one word must be entered.");

                var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                var validationCheck = CheckSearchValidation(ip);

                if (validationCheck != "ok")
                {
                    ViewBag.Message = "Limit of searches was exceeded! In order to have more searches, add new word:";
                    return View();
                }
                else
                {
                    _efUserLogRepository.InsertUserLog(word, ip, UserAction.Search);
                    ////var check = _databaseLogic.GetCachedWords(id);
                    var check = _efCachedWordRepository.GetCachedWords(word);

                    if (check.Count == 0)
                    {
                        var anagrams = _anagramSolver.GetAnagrams(word);
                        //var anagramsId = _databaseLogic.GetAnagramsId(anagrams);
                        var anagramsId = _efWordRepository.GetAnagramsId(anagrams);
                        //_databaseLogic.InsertCachedWords(id, anagramsId);
                        _efCachedWordRepository.InsertCachedWords(word, anagramsId);
                        return View(anagrams);
                    }
                    else
                    {
                        //var anagramsFromCache = _databaseLogic.GetCachedWords(id);
                        var anagramsFromCache = _efCachedWordRepository.GetCachedWords(word);
                        return View(anagramsFromCache);
                    }
                }
                //return View(anagrams);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(word);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DictionaryAdditionForm([Bind("Word1, Category")] WordEntity wordEntity)
        {
            if (ModelState.IsValid)
            {
                var word = wordEntity.Word1;
                var category = wordEntity.Category;
                var ip = HttpContext.Connection.RemoteIpAddress.ToString();

                var exists = _efWordRepository.CheckIfWordExists(word);

                if (exists)
                {
                    ModelState.AddModelError(string.Empty, "This word already exists!");
                    return View();
                }
                else
                {
                    _efWordRepository.InsertAdditionalWord(word, category);
                    _efUserLogRepository.InsertUserLog(word, ip, UserAction.Add);
                    ViewBag.Message = "New word added successfully! +1 search is added";
                    return View();
                }
            }
            return View();
        }

        private string CheckSearchValidation(string ip)
        {
            var ipCountSearch = _efUserLogRepository.CheckUserLogActions(ip, UserAction.Search);
            var ipCountAdd = _efUserLogRepository.CheckUserLogActions(ip, UserAction.Add);

            var maxSearchesForIP = Contracts.Settings.GetSettingsMaxSearchesForIP();
            if ((ipCountSearch - ipCountAdd) >= maxSearchesForIP)
            {
                var validation = "failed";
                return validation;
            }
            else
            {
                var validation = "ok";
                return validation;
            }
        }

    }
}
