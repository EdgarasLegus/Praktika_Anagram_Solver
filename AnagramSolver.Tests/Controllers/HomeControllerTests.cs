using AnagramSolver.Interfaces;
using AnagramSolver.WebApp.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using AnagramSolver.Interfaces.DBFirst;
using AnagramSolver.Interfaces.EF;
using Shouldly;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Moq;
using System.IO.IsolatedStorage;
using AnagramSolver.Contracts.Enums;
using AnagramSolver.Contracts.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace AnagramSolver.Tests
{
    public class HomeControllerTests
    {
        IAnagramSolver _anagramSolverMock;
        IDatabaseLogic _databaseLogicMock;

        IEFWordRepo _efWordRepositoryMock;
        IEFUserLogRepo _efUserLogRepositoryMock;
        IEFCachedWordRepo _efCachedWordRepositoryMock;

        IUserLogService _userLogServiceMock;
        IEFLogic _efLogicMock;

        HttpContext _httpContextMock;
        IHttpContextAccessor _httpMock;
        HomeController _controller;

        [SetUp]
        public void Setup()
        {
            _userLogServiceMock = Substitute.For<IUserLogService>();
            _efUserLogRepositoryMock = Substitute.For<IEFUserLogRepo>();
            _efCachedWordRepositoryMock = Substitute.For<IEFCachedWordRepo>();
            _anagramSolverMock = Substitute.For<IAnagramSolver>();
            _efWordRepositoryMock = Substitute.For<IEFWordRepo>();
            _databaseLogicMock = Substitute.For<IDatabaseLogic>();
            _efLogicMock = Substitute.For<IEFLogic>();


            _controller = new HomeController(_anagramSolverMock, _databaseLogicMock,
                _efWordRepositoryMock, _efUserLogRepositoryMock, _efCachedWordRepositoryMock, _userLogServiceMock, _efLogicMock);

            _httpContextMock = Substitute.For<HttpContext>();
            _httpMock = Substitute.For<IHttpContextAccessor>();

            
        }

        [Test]
        public async Task Test_IfIndexView_IsEmpty_WhenWordIsNull()
        {
            var viewResult = await _controller.Index(null) as ViewResult;
            var word = viewResult.ViewData.Model as IEnumerable<string>;

            Assert.IsNull(word);
        }

        [Test]
        public async Task Test_Index_IfMessageIsShown_WhenSearchLimitIsExceeded()
        {
            var word = "gimtadienis";
            var ip = _efLogicMock.GetIP();

            //_controller.ControllerContext = new ControllerContext();
            //var ip = _controller.ControllerContext.HttpContext.Connection.RemoteIpAddress.ToString();
            //var ip = _httpContextMock.Connection.RemoteIpAddress.ToString();
            //_controller.ControllerContext = new ControllerContext();
            //_controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //_controller.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            //var ip = _controller.GetContextIP();

            _userLogServiceMock.ValidateUserLog(ip).Returns("failed");

            var result = await _controller.Index(word) as ViewResult;
            var message = "Limit of searches was exceeded! In order to have more searches, add/update word:";

            _userLogServiceMock.Received().ValidateUserLog(ip);

            Assert.AreEqual(message, _controller.ViewBag.Message);

        }


        [Test]
        public async Task Test_Index_IfAnagramsListReturned_WhenSearchLimitIsOkAndWordIsNotCached()
        {
            var testWord = "pantis";
            var anagramList = new List<string>() { "pantis", "spinta" };
            var ip = _efLogicMock.GetIP();
            var wordList = new List<string>();

            _userLogServiceMock.ValidateUserLog(ip).Returns("ok");
            _efCachedWordRepositoryMock.GetCachedWords(testWord).Returns(wordList);
            _anagramSolverMock.GetAnagrams(testWord).Returns(anagramList);

            var viewResult = await _controller.Index(testWord) as ViewResult;
            var model = viewResult.ViewData.Model as IEnumerable<string>;

            _userLogServiceMock.Received().ValidateUserLog(ip);
            _efCachedWordRepositoryMock.Received().GetCachedWords(testWord);
            await _anagramSolverMock.Received().GetAnagrams(testWord);

            Assert.AreEqual(anagramList, model);
        }

        [Test]
        public async Task Test_Index_IfCachedAnagramsListReturned_WhenSearchLimitIsOkAndWordIsCached()
        {
            var testWord = "jaunas";
            var anagramList = new List<string>() { "jaunas", "naujas"};
            var ip = _efLogicMock.GetIP();

            _userLogServiceMock.ValidateUserLog(ip).Returns("ok");
            _efCachedWordRepositoryMock.GetCachedWords(testWord).Returns(anagramList);
            _efCachedWordRepositoryMock.GetCachedWords(testWord).Returns(anagramList);

            var viewResult = await _controller.Index(testWord) as ViewResult;
            var model = viewResult.ViewData.Model as IEnumerable<string>;

            _userLogServiceMock.Received().ValidateUserLog(ip);
            _efCachedWordRepositoryMock.Received().GetCachedWords(testWord);
            _efCachedWordRepositoryMock.Received().GetCachedWords(testWord);

            Assert.AreEqual(anagramList, model);
        }

        [Test]
        public void Test_AdditionFormValidationMessage_IfModelIsNotValid_WhenCategoryDoesNotExist()
        {

            var testWordEntity = new WordEntity() { Word1 = "pramoga", Category = "unkwnown" };
            var error = "The field Category must match the regular expression '" +
                "(^dll$|^rom\\.sk$|^dkt$|^tikr\\.dkt2|^įst$|^įv$|tikr\\" +
                ".dkt$|^sktv$|išt$|^sutr$prl$|^būdn$|^vksm$|^prv$|^bdv$|^jng$|^akronim$)'.";
            _controller.ModelState.AddModelError("Category", error);

            var viewResult = _controller.AdditionForm(testWordEntity) as ViewResult;

            var errorList = new List<string>();
            foreach (var item in viewResult.ViewData.ModelState.Values)
            {
                foreach(var errormessage in item.Errors)
                {
                    errorList.Add(errormessage.ErrorMessage);
                }
            }
            var myMessage = string.Join(",", errorList.ToArray()); 

            Assert.AreEqual(error, myMessage);
            
        }

        [Test]
        public void Test_AdditionForm_ErrorMessageComparison_IfWordAlreadyExists()
        {
            var testWordEntity = new WordEntity() { Word1 = "Barca", Category = "dkt" };
            var ip = _efLogicMock.GetIP();
            var alertMessage = "This word already exists!";

            _efWordRepositoryMock.CheckIfWordExists(testWordEntity.Word1).Returns(true);

            var viewResult = _controller.AdditionForm(testWordEntity) as ViewResult;

            var errorList = new List<string>();
            foreach (var item in viewResult.ViewData.ModelState.Values)
            {
                foreach (var errormessage in item.Errors)
                {
                    errorList.Add(errormessage.ErrorMessage);
                }
            }
            var actualMessage = string.Join(",", errorList.ToArray());

            _efWordRepositoryMock.Received().CheckIfWordExists(testWordEntity.Word1);

            Assert.AreEqual(alertMessage, actualMessage);
        }

        [Test]
        public void Test_AdditionForm_ViewBagContent_IfWordIsNew()
        {
            var testWordEntity = new WordEntity() { Word1 = "ManchesterUnited", Category = "dkt" };
            var ip = _efLogicMock.GetIP();

            _efWordRepositoryMock.CheckIfWordExists(testWordEntity.Word1).Returns(false);

            var viewResult = _controller.AdditionForm(testWordEntity) as ViewResult;
            var message = "New word added successfully! +1 search is added";

            _efWordRepositoryMock.Received().CheckIfWordExists(testWordEntity.Word1);
            Assert.AreEqual(message, _controller.ViewBag.Message);

        }

        [Test]
        public void Test_RemovalForm_ErrorMessageComparison_IfWordDoesNotExists()
        {
            var testWord = "Uagadugu";
            var ip = _efLogicMock.GetIP();
            var alertMessage = "Specified word does not exist!";

            _efWordRepositoryMock.CheckIfWordExists(testWord).Returns(false);

            var viewResult = _controller.RemovalConfirmed(testWord) as ViewResult;

            var errorList = new List<string>();
            foreach (var item in viewResult.ViewData.ModelState.Values)
            {
                foreach (var errormessage in item.Errors)
                {
                    errorList.Add(errormessage.ErrorMessage);
                }
            }
            var actualMessage = string.Join(",", errorList.ToArray());

            _efWordRepositoryMock.Received().CheckIfWordExists(testWord);
            Assert.AreEqual(alertMessage, actualMessage);
        }

        [Test]
        public void Test_DeletionForm_ViewBagContent_IfWordWasDeleted()
        {
            var testWord = "PSG";
            var ip = _efLogicMock.GetIP();

            _efWordRepositoryMock.CheckIfWordExists(testWord).Returns(true);

            var viewResult = _controller.RemovalConfirmed(testWord) as ViewResult;
            var message = "Selected word was deleted! Your searches will be reduced by 1";

            _efWordRepositoryMock.Received().CheckIfWordExists(testWord);
            Assert.AreEqual(message, _controller.ViewBag.Message);

        }

        [Test]
        public void Test_UpdateForm_ErrorMessageComparison_IfUpdatableWordDoesNotExists()
        {
            var testWord = "unknownword";
            var testWordEntity = new WordEntity() { Word1 = "kalnas", Category = "dkt" };
            var ip = _efLogicMock.GetIP();
            var alertMessage = "Specified word does not exist!";

            _efWordRepositoryMock.CheckIfWordExists(testWord).Returns(false);

            var viewResult = _controller.UpdateForm(testWord, testWordEntity) as ViewResult;

            var errorList = new List<string>();
            foreach (var item in viewResult.ViewData.ModelState.Values)
            {
                foreach (var errormessage in item.Errors)
                {
                    errorList.Add(errormessage.ErrorMessage);
                }
            }
            var actualMessage = string.Join(",", errorList.ToArray());

            _efWordRepositoryMock.Received().CheckIfWordExists(testWord);
            Assert.AreEqual(alertMessage, actualMessage);
        }

        [Test]
        public void Test_UpdateForm_ErrorMessageComparison_IfNewWordIsAlreadyExists()
        {
            var testWord = "pleiskana";
            var testWordEntity = new WordEntity() { Word1 = "plazminis", Category = "bdv" };
            var ip = _efLogicMock.GetIP();
            var alertMessage = "This word cannot be updated with your option, same word already exists!";

            _efWordRepositoryMock.CheckIfWordExists(testWord).Returns(true);
            _efWordRepositoryMock.UpdateWord(testWord, testWordEntity).Returns(true);

            var viewResult = _controller.UpdateForm(testWord, testWordEntity) as ViewResult;

            var errorList = new List<string>();
            foreach (var item in viewResult.ViewData.ModelState.Values)
            {
                foreach (var errormessage in item.Errors)
                {
                    errorList.Add(errormessage.ErrorMessage);
                }
            }
            var actualMessage = string.Join(",", errorList.ToArray());

            _efWordRepositoryMock.Received().CheckIfWordExists(testWord);
            _efWordRepositoryMock.Received().UpdateWord(testWord, testWordEntity);
            Assert.AreEqual(alertMessage, actualMessage);
        }

        [Test]
        public void Test_UpdateForm_ViewBagContent_IfWordWasUpdated()
        {
            var testWord = "pleiskana";
            var testWordEntity = new WordEntity() { Word1 = "plazminepleiskana", Category = "dkt" };
            var ip = _efLogicMock.GetIP();

            _efWordRepositoryMock.CheckIfWordExists(testWord).Returns(true);
            _efWordRepositoryMock.UpdateWord(testWord, testWordEntity).Returns(false);

            var viewResult = _controller.UpdateForm(testWord, testWordEntity) as ViewResult;
            var message = "Selected word was updated! +1 added";

            _efWordRepositoryMock.Received().CheckIfWordExists(testWord);
            _efWordRepositoryMock.Received().UpdateWord(testWord, testWordEntity);
            Assert.AreEqual(message, _controller.ViewBag.Message);

        }
    }
}

