using AnagramSolver.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Services
{
    public class AnagramSolverREST : IAnagramSolver
    {
        private readonly HttpClient _client;

        public AnagramSolverREST()
        {
            _client = new HttpClient();
        }

        public async Task<IEnumerable<string>> GetAnagrams(string myWords)
        {
            HttpResponseMessage responseMessage = await _client.GetAsync("http://www.anagramica.com/all/" + myWords);

            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception("Cannot GET anagrams for specified word!");

            var responseBody = await responseMessage.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseBody);
            var stringAnagrams = jsonResponse["all"].ToString();
            var anagrams = new string[] {stringAnagrams };

            return anagrams;
        }

        public int CountChars(string input)
        {
            char[] characters = input.ToCharArray();
            int charCount = input.Count(c => !Char.IsWhiteSpace(c));
            return charCount;
        }
    }
}
