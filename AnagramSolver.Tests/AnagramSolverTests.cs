using AnagramSolver.BusinessLogic;
using AnagramSolver.Interfaces;
using AnagramSolver.Repos;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using NSubstitute;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Shouldly;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AnagramSolver.Contracts;
using System.Reflection;
using AnagramSolver.Contracts.Entities;
using AnagramSolver.EF.DatabaseFirst;
using AnagramSolver.EF.CodeFirst;
using AnagramSolver.Repos.EF;

namespace AnagramSolver.Tests
{
    [TestFixture]
    public class AnagramSolverTests
    {
        private IAnagramSolver _anagramSolver;
        //private readonly AnagramSolverDBFirstContext _context;
        private readonly AnagramSolverCodeFirstContext _context;
        private readonly EFWordRepository _efWordRepository;

        //public AnagramSolverTests(AnagramSolverDBFirstContext context, EFRepository efRepository)
        //{
        //    _context = context;
        //    _efRepository = efRepository;
        //}

        public AnagramSolverTests(AnagramSolverCodeFirstContext context, EFWordRepository efWordRepository)
        {
            _context = context;
            _efWordRepository = efWordRepository;
        }

        [SetUp]
        public void Setup()
        {
            _anagramSolver = new BusinessLogic.AnagramSolver()
            {
                //FRepository = new FRepository()
                //DBRepository = new DBRepository()
                //EFRepository = new EFRepository(new AnagramSolverDBFirstContext())
                //EFRepository = new EFRepository(new AnagramSolverCodeFirstContext())
                EFWordRepo = new EFWordRepository(new AnagramSolverCodeFirstContext())
            };
        }

        [TestCase("absorbavimas", "aaabbimorssv")]
        [TestCase("120-as", "-012as")]
        [TestCase("gargastroduodenofibroskopija", "aaabddefggiijknoooooprrrsstu")]
        [TestCase("Simonas", "Saimnos")]
        // Padebugintas
        public void TestIfInputIsSortedCorrectly(string inputWord, string expectedOutput)
        {
            //Arrange
            //string inputWord = "absorbavimas";
            //string expectedOutput = "aaabbimorssv";

            //Act
            string output = _anagramSolver.SortByAlphabet(inputWord);
            bool ifContains = output.Equals(expectedOutput);

            //Assert
            Assert.IsTrue(ifContains);
        }

        [TestCase("gastroenterologinis garvežys ir gargastroduodenofibroskopija", 57)]
        [TestCase("120-as", 6)]
        [TestCase(".", 1)]
        [TestCase("", 0)]
        [TestCase("\t", 0)]
        [TestCase("      ", 0)]
        //Padebugintas
        public void TestIfCharactersAmountIsCountedCorrectly(string inputWord, int expectedCount)
        {
            //Arrange
            //string inputWord = "gastroenterologinis garvežys ir gargastroduodenofibroskopija";
            //int expectedCount = 57;
            //Act
            int outputCount = _anagramSolver.CountChars(inputWord);

            //Assert
            Assert.AreEqual(expectedCount, outputCount);
        }

        [Test]
        public void TestIfCreatedDictionaryIsCorrect()
        {
            //Arrange
            var testInputFromGivenColumns = new Dictionary<string, string>()
            {
                {"Simonas", "tikr." },
                {"nesinchronizuoti", "vksm." },
                {"Ribentropas", "tikr." },
                {"Atlantida", "tikr." },
                {"14-15-tas", "sktv." }

            }.Select(pair => new WordEntity() { Word1 = pair.Key, Category = pair.Value }).ToList();

            //// DB REPOSITORY
            ////}.Select(pair => new WordModel() { Word = pair.Key, Category = pair.Value}).ToList();

            var expectedOutputFromGivenColumns = new Dictionary<string, string>()
            {
                {"Simonas", "Saimnos" },
                {"nesinchronizuoti", "cehiiinnnoorstuz" },
                {"Ribentropas", "Rabeinoprst" },
                {"Atlantida", "Aaadilntt" },
                {"14-15-tas", "--1145ast" }
            };

            //Act
            Dictionary<string, string> outputDictionary = _anagramSolver.MakeDictionary(testInputFromGivenColumns);
            //var outputDictionaryForComparison = outputDictionary.Select(pair => new WordModel() { Word = pair.Key, Category = pair.Value }).ToList();
            //Assert
            CollectionAssert.AreEqual(expectedOutputFromGivenColumns, outputDictionary);
        }

        [TestCase("veidas", "dievas")]
        [TestCase("ristas", "rastis")]
        [TestCase("lipti", "pilti")]
        public void TestAnagramsReturnFromSingleWord(string inputWord, string expectedAnagram)
        {
            //Act
            IEnumerable<string> output = _anagramSolver.GetAnagrams(inputWord);
            //Assert
            CollectionAssert.Contains(output, expectedAnagram);
        }
    }
}