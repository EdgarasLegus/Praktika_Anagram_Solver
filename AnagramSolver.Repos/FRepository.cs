using AnagramSolver.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace AnagramSolver.Repos
{
    public class FRepository : Interfaces.IWordRepository
    {
        public List<WordModel> GetWords()
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
                    var wordModel = new WordModel()
                    {
                        Word = word,
                        Category = PartOfSpeech
                    };
                    //Console.WriteLine("OPA"+ wordModel);

                    if (!wordModelList.ContainsKey(word))
                    {
                        wordModelList.Add(wordModel.Word, wordModel.Category);
                        //Console.WriteLine(dictionary);
                    }
                    //Console.WriteLine(dictionary);
                    //Console.WriteLine("FULL: " + wordModelList);

                }
            }
            var returnList = wordModelList.Select(pair => new WordModel()
            {
                Word = pair.Key,
                Category = pair.Value
            }).ToList();
            return returnList;

        }

        // Isnesame configuration is metodo ribu, kad galima butu keisti

        //private readonly IConfigurationRoot _configuration = new ConfigurationBuilder().AddJsonFile(@"./appsettings.json").Build();
        //public IConfigurationRoot _configuration;

        //private IConfigurationRoot _configuration;

        //public FRepository()
        //{
        //    _configuration = new ConfigurationBuilder()
        //       .AddJsonFile(@"./appsettings.json", optional: true)
        //     .Build();
        //}

        //const string path = @"./zodynas.txt";
        // ---- 1111111 Pirmojo ir antrojo stulpelio gavimas is failo
        //        public Dictionary<string, string> GetWords()
        //        {
        //            var configuration = new ConfigurationBuilder()
        //              .AddJsonFile(@"./appsettings.json")
        //              .Build();
        //            // var configuration = Contracts.ConfigurationConstants.ConfBuilder;

        //            //Contracts.ConfigurationConstants.FileName = _configuration["Settings:FileName"];

        //            //var path = ConfigurationConstants.FileName;
        //            var path = configuration["Settings:FileName"];
        //            //var path = configuration[Contracts.ConfigurationConstants.FileName];
        //            // = _configuration["Settings:FileName"];

        //            if (!File.Exists(path))
        //            {
        //                throw new Exception($"Data file {path} does not exist!");
        //            }

        //            // Dictionary inicializacija

        //            var dictionary = new Dictionary<string, string>();
        //            // Failo skaitymas
        //            using (StreamReader reader = new StreamReader(path))
        //            {
        //                string line;
        //                while ((line = reader.ReadLine()) != null)
        //                {
        //                    string word = line.Split('\t').First();
        //                    string PartOfSpeech = line.Split('\t').ElementAt(1);

        //                    if (!dictionary.ContainsKey(word))
        //                    {
        //                        dictionary.Add(word, PartOfSpeech);
        //                        //Console.WriteLine(dictionary);
        //                    }
        //                }
        //;
        //            }
        //            return dictionary;
        //        }

        //        public List<WordModel> GetTest()
        //        {
        //            var configuration = new ConfigurationBuilder()
        //               .AddJsonFile(@"./appsettings.json")
        //               .Build();

        //            var path = configuration["Settings:FileName"];

        //            if (!File.Exists(path))
        //            {
        //                throw new Exception($"Data file {path} does not exist!");
        //            }

        //            var wordModelList = new List<WordModel>();

        //            using (StreamReader reader = new StreamReader(path))
        //            {
        //                string line;
        //                while ((line = reader.ReadLine()) != null)
        //                {
        //                    string word = line.Split('\t').First();
        //                    string PartOfSpeech = line.Split('\t').ElementAt(1);
        //                    var wordModel = new WordModel()
        //                    {
        //                        Word = word,
        //                        Category = PartOfSpeech
        //                    };
        //                    //Console.WriteLine("OPA"+ wordModel);
        //                    if (!wordModelList.Exists(x => x.Word == wordModel.Word))
        //                    {
        //                        wordModelList.Add(wordModel);
        //                        //Console.WriteLine(dictionary);
        //                    }
        //                    //Console.WriteLine("FULL: " + wordModelList);

        //                }
        //;
        //            }
        //            return wordModelList;
        //        }
    }
}

