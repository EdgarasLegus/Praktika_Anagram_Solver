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

        private readonly IUserLogService _userLogService;

        public HomeController(IAnagramSolver anagramSolver, IDatabaseLogic databaseLogic,
            IEFWordRepo efWordRepository, IEFUserLogRepo efUserLogRepository, IEFCachedWordRepo efCachedWordRepository, IUserLogService userLogService, IEFLogic efLogic)
        {
            _anagramSolver = anagramSolver;
            _databaseLogic = databaseLogic;

            _efWordRepository = efWordRepository;
            _efUserLogRepository = efUserLogRepository;
            _efCachedWordRepository = efCachedWordRepository;

            _userLogService = userLogService;
            _eflogic = efLogic;
        }

        public async Task<IActionResult> Index(string word)
        {
            try
            {
                if (string.IsNullOrEmpty(word))
                    throw new Exception("Error! At least one word must be entered.");

                var ip = _eflogic.GetIP();
                //var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                //var ip = GetContextIP();
                var validationCheck = _userLogService.ValidateUserLog(ip);

                if (validationCheck != "ok")
                {
                    ViewBag.Message = "Limit of searches was exceeded! In order to have more searches, add/update word:";
                    return View();
                }
                else
                {
                    //var ip = _eflogic.GetIP();
                    _efUserLogRepository.InsertUserLog(word, ip, UserAction.Search);
                    ////var check = _databaseLogic.GetCachedWords(id);
                    var check = _efCachedWordRepository.GetCachedWords(word);

                    if (check.Count == 0)
                    {
                        var anagrams = await _anagramSolver.GetAnagrams(word);
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

        // GET - get form of word addition

        public IActionResult AdditionForm()
        {
            return View();
        }

        //POST - add word to dictionary

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdditionForm([Bind("Word1, Category")] WordEntity wordEntity)
        {
            if (ModelState.IsValid)
            {
                var word = wordEntity.Word1;
                var category = wordEntity.Category;
                //var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                var ip = _eflogic.GetIP();

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

        public IActionResult RemovalForm()
        {
            return View();
        }

        [HttpPost, ActionName("RemovalForm")]
        [ValidateAntiForgeryToken]
        public IActionResult RemovalConfirmed(string word)
        {
            if (ModelState.IsValid)
            {
                var ip = _eflogic.GetIP();
                //var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                var exists = _efWordRepository.CheckIfWordExists(word);

                if (!exists)
                {
                    ModelState.AddModelError(string.Empty, "Specified word does not exist!");
                    return View();
                }
                else
                {
                    _efWordRepository.RemoveWord(word);
                    _efUserLogRepository.InsertUserLog(word, ip, UserAction.Remove);
                    ViewBag.Message = "Selected word was deleted! Your searches will be reduced by 1";
                    return View();
                }
            }
            return View();
        }

        public IActionResult UpdateForm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateForm(string updatableWord, [Bind("Word1, Category")] WordEntity wordEntity)
        {
            if (ModelState.IsValid)
            {
                var ip = _eflogic.GetIP();
                //var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                var exists = _efWordRepository.CheckIfWordExists(updatableWord);

                if (!exists)
                {
                    ModelState.AddModelError(string.Empty, "Specified word does not exist!");
                    return View();
                }
                else
                {
                    var updateCheck = _efWordRepository.UpdateWord(updatableWord, wordEntity);
                    if (updateCheck == true)
                    {
                        ModelState.AddModelError(string.Empty, "This word cannot be updated with your option, same word already exists!");
                        return View();
                    }
                    else
                    {
                        _efUserLogRepository.InsertUserLog(updatableWord, ip, UserAction.Update);
                        ViewBag.Message = "Selected word was updated! +1 added";
                        return View();
                    }
                }
            }
            return View();
        }

        public string GetContextIP()
        {
            return HttpContext.Connection.RemoteIpAddress.ToString();
            //return "::1";
        }

    }
}
