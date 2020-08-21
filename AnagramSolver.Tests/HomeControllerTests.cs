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
        HttpContext _httpContextMock;

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


            _controller = new HomeController(_anagramSolverMock, _databaseLogicMock,
                _efWordRepositoryMock, _efUserLogRepositoryMock, _efCachedWordRepositoryMock, _userLogServiceMock);

            //_controller.ControllerContext = new ControllerContext( _httpContextMock.object)

            _httpContextMock = Substitute.For<HttpContext>();
        }

        [Test]
        public void TestIfIndexViewReturnedEmptyWhenWordIsNull()
        {
            var viewResult = _controller.Index(null) as ViewResult;
            var word = viewResult.ViewData.Model as IEnumerable<string>;

            Assert.IsNull(word);
        }


        [Test]
        public void TestUnapprovedSearchValidation_ShouldBeEmptyView()
        {
            var ip = _controller.GetIP();
            var word = "ubagas";
            var list = new List<string>();
            list.Add(word);

            _httpContextMock.Connection.RemoteIpAddress.ToString().Returns("::1");
            _userLogServiceMock.ValidateUserLog("::1").Returns("ok");
            _efCachedWordRepositoryMock.GetCachedWords(word).Returns(list);
            _anagramSolverMock.GetAnagrams(word).Returns(list);

            var viewResult = _controller.Index(word) as ViewResult;

            //viewResult.Model.ShouldBe(list);

            _httpContextMock.Received().Connection.RemoteIpAddress.ToString();
            _userLogServiceMock.Received().ValidateUserLog("::1");
            _efCachedWordRepositoryMock.Received().GetCachedWords(word);
            _anagramSolverMock.Received().GetAnagrams(word);

            Assert.AreEqual(list, viewResult);
            //Assert.IsNotNull(viewResult.ViewData[""]);
            //Assert.IsNull(viewResult.Model);
            //Assert.IsAssignableFrom<ViewResult>(viewResult);
        }














        [TestCase("alus")]
        public void TestIfHomeControllerIndexIsSame(string id)
        {
            //Arrange
            var controller = new HomeController(_anagramSolverMock, _databaseLogicMock,
                _efWordRepositoryMock, _efUserLogRepositoryMock, _efCachedWordRepositoryMock, _userLogServiceMock);
            //Act
            ViewResult result = controller.Index(id) as ViewResult;
            //Assert
            Assert.AreEqual("alus", result.ViewName);
        }

        [Test]
        public void TestIfIndexReturnsNeededResult()
        {
            //Arrange
            _anagramSolverMock = Substitute.For<IAnagramSolver>();
            var anagramList = new List<string>()
            {
                "kalnas", "klanas", "lankas", "skalna"
            };

            _anagramSolverMock.GetAnagrams(Arg.Any<string>()).Returns(anagramList);
            var controller = new HomeController(_anagramSolverMock, _databaseLogicMock,
                _efWordRepositoryMock, _efUserLogRepositoryMock, _efCachedWordRepositoryMock, _userLogServiceMock);
            var result = controller.Index("kalnas");

            //Act
            _anagramSolverMock.Received().GetAnagrams(Arg.Any<string>());

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

