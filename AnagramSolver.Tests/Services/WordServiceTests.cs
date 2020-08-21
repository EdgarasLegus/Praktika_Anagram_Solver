using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Tests.Services
{
    [TestFixture]
    public class WordServiceTests
    {
        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(@"./appsettings.json")
               .Build();

            var path = configuration["Settings:FileName"];
        }

        [Test]
        public void TestFileLoadingToCodeFirstDatabase_ShouldNotThrowException()
        {

        }
    }
}
