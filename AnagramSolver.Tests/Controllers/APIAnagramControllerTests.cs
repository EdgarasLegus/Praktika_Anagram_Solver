using AnagramSolver.Contracts.Entities;
using AnagramSolver.Interfaces;
using AnagramSolver.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Tests.Controllers
{
    public class APIAnagramControllerTests
    {
        IAnagramSolver _anagramSolverMock;
        APIAnagramController _apiAnagramController;

        [SetUp]
        public void Setup()
        {
            _anagramSolverMock = Substitute.For<IAnagramSolver>();

            _apiAnagramController = new APIAnagramController(_anagramSolverMock);
        }

        [Test]
        public async Task TestIActionResult_IfSuccess()
        {
            var testWord = "kalnas";

            var testAnagramsList = new List<string>() { "skalna", "lankas", "klanas", "kalnas" };

            _anagramSolverMock.GetAnagrams(testWord).Returns(testAnagramsList);

            var result = await _apiAnagramController.Get(testWord) as ObjectResult;
            var model = result.Value as IEnumerable<string>;

            await _anagramSolverMock.Received().GetAnagrams(testWord);

            Assert.AreEqual(testAnagramsList, model);
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task TestIActionResult_IfRequestIsUnsuccessful()
        {
            var testWord = "";
            var result = await _apiAnagramController.Get(testWord) as ObjectResult;
            Assert.AreEqual(400, result.StatusCode);
        }
    }
}
