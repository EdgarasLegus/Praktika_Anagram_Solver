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

namespace AnagramSolver.Tests
{
    public class HomeControllerTests
    {
        IWordRepository repository;
        IAnagramSolver anagramSolver;
        IDatabaseLogic databaseLogic;
        IEFLogic eflogic;

        IEFWordRepo efWordRepository;
        IEFUserLogRepo efUserLogRepository;
        IEFCachedWordRepo efCachedWordRepository;

        IUserLogService userLogService;

        [SetUp]
        public void Setup()
        {
        }

        [TestCase("alus")]
        public void TestIfHomeControllerIndexIsSame(string id)
        {
            //Arrange
            var controller = new HomeController(anagramSolver, databaseLogic, eflogic,
                efWordRepository, efUserLogRepository, efCachedWordRepository, userLogService);
            //Act
            ViewResult result = controller.Index(id) as ViewResult;
            //Assert
            Assert.AreEqual("alus", result.ViewName);
        }

        [Test]
        public void TestIfIndexReturnsNeededResult()
        {
            //Arrange
            anagramSolver = Substitute.For<IAnagramSolver>();
            var anagramList = new List<string>()
            {
                "kalnas", "klanas", "lankas", "skalna"
            };

            anagramSolver.GetAnagrams(Arg.Any<string>()).Returns(anagramList);
            var controller = new HomeController(anagramSolver, databaseLogic, eflogic,
                efWordRepository, efUserLogRepository, efCachedWordRepository, userLogService);
            var result = controller.Index("kalnas");

            //Act
            anagramSolver.Received().GetAnagrams(Arg.Any<string>());

            //Assert
            CollectionAssert.Contains(anagramList, result);

            ////Arrange
            //var anagramList = anagramSolver.GetAnagrams("kalnas");
            //var controller = new HomeController(anagramSolver);
            //var result = controller.Index("kalnas");

            ////Assert
            //CollectionAssert.Contains(anagramList, result);

        }
    }
}

