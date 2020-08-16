using AnagramSolver.BusinessLogic;
using System;
using System.Collections.Generic;
using AnagramSolver.Interfaces;
using Microsoft.Extensions.Configuration;
using System.IO;
using AnagramSolver.Contracts;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using AnagramSolver.Repos;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using AnagramSolver.EF.DatabaseFirst;
using AnagramSolver.Interfaces.DBFirst;

namespace AnagramSolver.UI
{
    class Program
    {

        private static readonly IAnagramSolver _anagramSolver = new BusinessLogic.AnagramSolver();
        private static readonly IUI _userInterface = new UI();

        // Keiciama is static void Main i static async Task kad kviest async
        static async Task Main(string[] args)
        {
            // 1- Ar uzpildyti lentele
            //**//DB
            //_userInterface.ToFillTable();

            var minInputWordLength = Configuration.BuilderConfigurations();

            var input = _userInterface.GetUserInput(minInputWordLength);

            var anagrams = _anagramSolver.GetAnagrams(input);

            Console.WriteLine("---Anagramos:");
            Console.WriteLine(string.Join('\n', anagrams));

            // Get anagrams response by making API request
            Console.WriteLine("---API request:");
            var inputForRequest = _userInterface.GetUserInput(minInputWordLength);
            var responseAnagrams = await _userInterface.RequestAPI(inputForRequest);
            Console.WriteLine("---API Response anagramos:");
            Console.WriteLine(responseAnagrams);


            //+++ Configuration klase , o metodas AnagramValidator, sukurt objekta pries tai.
            // Ideti validavima i try catch catch --> Console.. ArgumentException 
        }
    }
}

