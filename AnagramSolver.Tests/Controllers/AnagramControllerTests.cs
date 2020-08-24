using AnagramSolver.Contracts;
using AnagramSolver.Interfaces;
using NSubstitute;
using AnagramSolver.WebApp.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using AnagramSolver.Interfaces.DBFirst;
using AnagramSolver.Interfaces.EF;
using AnagramSolver.Contracts.Entities;
using Microsoft.AspNetCore.Mvc;
using AnagramSolver.WebApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramSolver.Tests
{
    public class AnagramControllerTests
    {
        //IWordRepository repository;
        //IAnagramSolver anagramSolver;
        //List<Anagram> list;
        //Anagram anagram;

        IWordRepository _repositoryMock;
        IAnagramSolver _anagramSolverMock;
        IDatabaseLogic _databaseLogicMock;
        IEFLogic _efLogicMock;
        IEFWordRepo _efWordRepositoryMock;

        AnagramController _anagramController;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = Substitute.For<IWordRepository>();
            _anagramSolverMock = Substitute.For<IAnagramSolver>();
            _databaseLogicMock = Substitute.For<IDatabaseLogic>();
            _efLogicMock = Substitute.For<IEFLogic>();
            _efWordRepositoryMock = Substitute.For<IEFWordRepo>();

            _anagramController = new AnagramController(_repositoryMock, _anagramSolverMock, _databaseLogicMock, _efLogicMock, _efWordRepositoryMock);

        }

        [Test]
        public void TestIndex_IfSearchIsPerformed()
        {
            var testWordEntity = new WordEntity()
            {
                Word1 = "panorama",
                Category = "dkt"
            };
            var testAnagramList = new List<WordEntity>() { testWordEntity };

            _efWordRepositoryMock.SearchWords(Arg.Any<string>()).Returns(testAnagramList);

            var result = _anagramController.Index(1, testWordEntity.Word1) as ViewResult;
            var model = result.ViewData.Model as PaginatedList<WordEntity>;

            _efWordRepositoryMock.Received().SearchWords(Arg.Any<string>());

            Assert.AreEqual(testAnagramList, model);

        }

        [Test]
        public void TestIndex_IfSearchIsNotPerformed()
        {
            var searchWord = ""; 
            var testWordEntity = new WordEntity()
            {
                Word1 = "panorama",
                Category = "dkt"
            };
            var testAnagramList = new List<WordEntity>() { testWordEntity };

            _efWordRepositoryMock.GetWords().Returns(testAnagramList);

            var result = _anagramController.Index(1, searchWord) as ViewResult;
            var model = result.ViewData.Model as PaginatedList<WordEntity>;

            _efWordRepositoryMock.Received().GetWords();

            Assert.AreEqual(testAnagramList, model);

        }

        [Test]
        public async Task TestWordAnagrams_IfReturnedViewIsCorrect()
        {
            var testWordEntity = new WordEntity()
            {
                Word1 = "mokinys",
                Category = "dkt"
            };

            var testAnagramsList = new List<string>() { testWordEntity.Word1};

            _anagramSolverMock.GetAnagrams(testWordEntity.Word1).Returns(testAnagramsList);

            var result = await _anagramController.WordAnagrams(testWordEntity.Word1) as ViewResult;
            var model = result.ViewData.Model as IEnumerable<string>;

            _anagramSolverMock.Received().GetAnagrams(testWordEntity.Word1);

            Assert.AreEqual(testAnagramsList, model);
        }
    }
}
