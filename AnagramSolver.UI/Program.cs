﻿using AnagramSolver.BusinessLogic;
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
using System.Diagnostics;
using System.Net;
using AnagramSolver.Repos.EF;
using AnagramSolver.Interfaces.EF;
using AnagramSolver.EF.CodeFirst;
using AnagramSolver.BusinessLogic.Services;

namespace AnagramSolver.UI
{
    class Program
    {
        private static readonly IEFWordRepo _eFWordRepository = new EFWordRepository(new AnagramSolverCodeFirstContext());
        private static readonly AnagramSolverCodeFirstContext _context = new AnagramSolverCodeFirstContext();
        private static readonly IWordService _wordService = new WordService();
        private static readonly IAnagramSolver _anagramSolver = new BusinessLogic.AnagramSolver(_eFWordRepository, _context, _wordService);
        private static readonly IUI _userInterface = new UI();
        private static readonly IAnagramSolver _anagramSolverREST = new BusinessLogic.Services.AnagramSolverREST();

        //private static readonly Print _print = new Print(WriteLineConsole);
        //private static readonly Display _display = new Display(new Print(WriteLineConsole));

        private static readonly Action<string> _print = new Action<string>(WriteLineConsole);
        private static readonly Display _display = new Display(new Action<string>(WriteLineConsole));

        // EVENT CONSUMING
        private static readonly DisplayWithEvents _displayWithEvents = new DisplayWithEvents();

        //public event EventHandler<Print> PrintingEvent = new Print();


        // Keiciama is static void Main i static async Task kad kviest async
        static async Task Main(string[] args)
        {
            // 1- Ar uzpildyti lentele
            //**//DB
            //_userInterface.ToFillTable();

            _displayWithEvents.eventPrinting += WriteLineConsole;
            _displayWithEvents.eventPrinting += WriteLineFile;

            var minInputWordLength = Configuration.BuilderConfigurations();

            var input = _userInterface.GetUserInput(minInputWordLength);

            var anagrams = await _anagramSolver.GetAnagrams(input);
            var joinedAnagrams = string.Join('\n', anagrams);

            _print("---Anagramos:");
            _print(string.Join('\n', joinedAnagrams));
            _print("\n");


            _print("---Anagramos su kapitalizuota pirmąja raide");
            _display.FormattedPrint(Capitalize, joinedAnagrams);
            _print("\n");

            // Get anagrams response by making API request
            _print("---API request:");
            var inputForRequest = _userInterface.GetUserInput(minInputWordLength);
            var responseAnagrams = await _userInterface.RequestAPI(inputForRequest);
            _print("---API Response anagramos:");
            _print(responseAnagrams);
            _print("\n");

            //Get anagrams from Academica REST API
            _print("--Anagramica REST API");
            var inputForAnagramica = _userInterface.GetUserInput(minInputWordLength);
            var responseAnagramica = await _anagramSolverREST.GetAnagrams(inputForAnagramica);
            _print("---Anagramica anagramos:");
            _print(string.Join('\n', responseAnagramica));
            _print("\n");
        }

        public static void WriteLineConsole(string msg)
        {
            Console.WriteLine(msg);
        }

        public static void WriteLineDebug(string msg)
        {
            Debug.WriteLine(msg);
        }

        public static void WriteLineFile(string msg)
        {
            File.WriteAllText(@"./output.txt", msg);
        }

        public static string Capitalize(string input)
        {
            if (input.Length == 1)
                return char.ToUpper(input[0]).ToString();
            else
                return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}

