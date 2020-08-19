using AnagramSolver.Contracts.Entities;
using AnagramSolver.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AnagramSolver.BusinessLogic.Services
{
    public class WordService : IWordService
    {
        public List<WordEntity> GetWordEntityFromFile()
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile(@"./appsettings.json")
               .Build();

            var path = configuration["Settings:FileName"];

            if (!File.Exists(path))
            {
                throw new Exception($"Data file {path} does not exist!");
            }

            var wordModelList = new Dictionary<string, string>();

            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string word = line.Split('\t').First();
                    string PartOfSpeech = line.Split('\t').ElementAt(1);
                    var wordModel = new WordEntity()
                    {
                        Word1 = word,
                        Category = PartOfSpeech
                    };
                    if (!wordModelList.ContainsKey(word))
                    {
                        wordModelList.Add(wordModel.Word1, wordModel.Category);
                    }
                }
            }
            var returnList = wordModelList.Select(pair => new WordEntity()
            {
                Word1 = pair.Key,
                Category = pair.Value
            }).ToList();
            return returnList;

        }
    }
}
