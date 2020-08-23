using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts.Enums;
using AnagramSolver.Interfaces;
using AnagramSolver.Interfaces.DBFirst;
using AnagramSolver.Interfaces.EF;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnagramSolver.Tests.Services
{
    [TestFixture]
    public class UserLogsServiceTests
    {
        private IEFUserLogRepo _efUserLogRepositoryMock;
        private IUserLogService _userLogService;
        private IEFLogic _efLogicMock;

        [SetUp]
        public void Setup()
        {
            _efUserLogRepositoryMock = Substitute.For<IEFUserLogRepo>();
            _userLogService = new UserLogService(_efUserLogRepositoryMock, _efLogicMock);
        }

        [Test]
        public void GetUserLogIPValidationForRemove_ShouldFail()
        {
            // appsettings reiksme - "MaxSearchesForIP": 11
            var ip = "::1";
            UserAction userAction = UserAction.Remove;
            _efUserLogRepositoryMock.CheckUserLogActions(ip, userAction).Returns(12);

            var result = _userLogService.ValidateUserLog(ip);

            result.ShouldNotBeNull();
            result.ShouldBe("failed");
            _efUserLogRepositoryMock.Received().CheckUserLogActions(ip, userAction);

            //Assert.That(_efUserLogRepositoryMock.CheckUserLogActions(ip, userAction), Is.EqualTo(0));
        }

        [TestCase(5,3,10,0,"failed")]
        [TestCase(7,1,6,2,"ok")]
        [TestCase(-5,-6,-7,-3, "ok")]
        public void GetCorrectUserLogIPValidationByCheckingSum(int searchAction, int addAction, int removeAction, int updateAction, string expectedResult)
        {
            var ip = "::1";

            _efUserLogRepositoryMock.CheckUserLogActions(ip, UserAction.Search).Returns(searchAction);
            _efUserLogRepositoryMock.CheckUserLogActions(ip, UserAction.Add).Returns(addAction);
            _efUserLogRepositoryMock.CheckUserLogActions(ip, UserAction.Remove).Returns(removeAction);
            _efUserLogRepositoryMock.CheckUserLogActions(ip, UserAction.Update).Returns(updateAction);

            var result = _userLogService.ValidateUserLog(ip);

            _efUserLogRepositoryMock.Received().CheckUserLogActions(ip, UserAction.Search);
            _efUserLogRepositoryMock.Received().CheckUserLogActions(ip, UserAction.Add);
            _efUserLogRepositoryMock.Received().CheckUserLogActions(ip, UserAction.Remove);
            _efUserLogRepositoryMock.Received().CheckUserLogActions(ip, UserAction.Update);

            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void GetUserLogIPValidationForAnyAction_ShouldBeOk()
        {
            var ip = "::1";
            _efUserLogRepositoryMock.CheckUserLogActions(ip, Arg.Any<UserAction>()).Returns(5, 9, 4, 0);

            var result = _userLogService.ValidateUserLog(ip);

            result.ShouldNotBeNull();
            result.ShouldBe("ok");
            _efUserLogRepositoryMock.Received().CheckUserLogActions(ip, Arg.Any<UserAction>());
        }
    }
}
