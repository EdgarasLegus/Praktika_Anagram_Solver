using AnagramSolver.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AnagramSolver.Contracts;
using AnagramSolver.Repos;
using AnagramSolver.EF.DatabaseFirst;
using AnagramSolver.Interfaces.DBFirst;

namespace AnagramSolver.UI
{
    public class UI : IUI
    {
        private static readonly IAnagramSolver _anagramSolver = new BusinessLogic.AnagramSolver();
        private static readonly HttpClient client = new HttpClient();
        private static readonly IFillDB _fillDB = new FillDBRepository();



        public string GetUserInput(int minInputWordLength)
        {
            // Ivesties skaitymas
            Console.WriteLine("Input your word for solution: ");
            var input = Console.ReadLine();

            // Ivesties characters skaiciavimas
            int charCount = _anagramSolver.CountChars(input);

            // 1-os konfiguracijos tikrinimas
            while (charCount < minInputWordLength)
            {
                Console.WriteLine("--Chars counted: " + charCount);
                Console.WriteLine($"Length of input word is less than {minInputWordLength}! Try again");
                input = Console.ReadLine();

                var inputCharCount = _anagramSolver.CountChars(input);

                if (inputCharCount >= 1)
                {
                    charCount = inputCharCount;
                    Console.WriteLine("Input is Valid!");
                }
            }

            Console.WriteLine("--Chars counted: " + charCount);
            return input;
        }

        public void ToFillTable()
        {
            // Add information to database
            Console.WriteLine("---Fill AnagramSolver.dbo.Word(Y/press any key))?");

            string answer = Console.ReadLine();
            WordModel wordModel = new WordModel();
            if (answer == "Y")
            {
                var check = _fillDB.checkIfTableIsEmpty();
                if (check == true)
                {
                    _fillDB.FillDatabaseFromFile(wordModel);
                    Console.WriteLine("Data pushed successfully! Check AnagramSolver.dbo.Word table.");
                }
                else
                {
                    Console.WriteLine("Table is already filled with data!");
                }
            }
        }

        public async Task<string> RequestAPI(string inputForRequest)
        {
            HttpResponseMessage responseMessage = await client.GetAsync("https://localhost:44325/api/" + inputForRequest);
            responseMessage.EnsureSuccessStatusCode();

            var responseBody = await responseMessage.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}

