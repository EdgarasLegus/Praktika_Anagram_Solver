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
        public void TestIndex()
        {

        }
    }
}
