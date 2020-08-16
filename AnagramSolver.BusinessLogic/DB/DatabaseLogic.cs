using AnagramSolver.Contracts;
using AnagramSolver.Interfaces;
using Renci.SshNet.Messages.Connection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AnagramSolver.BusinessLogic
{
    public class DatabaseLogic : IDatabaseLogic
    {
        private readonly string connectionString = "Server=LT-LIT-SC-0513;Database=AnagramSolver;" +
            "Integrated Security = true;Uid=auth_windows";
        private readonly IAnagramSolver _anagramSolver;

        public DatabaseLogic(IAnagramSolver anagramSolver)
        {
            _anagramSolver = anagramSolver;
        }

        public List<WordModel> SearchWords(string searchInput)
        {
            var wordModel = new List<WordModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM AnagramSolver.dbo.Word WHERE Word LIKE @searchInput", connection);
                cmd.Parameters.Add(new SqlParameter("@searchInput", "%" + searchInput + "%"));
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            wordModel.Add(new WordModel
                            {
                                Word = reader["Word"].ToString(),
                                Category = reader["Category"].ToString()
                            });
                        }
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return wordModel;
        }

        public List<int> GetAnagramsId(IEnumerable<string> anagrams)
        {
            var anagramsIdList = new List<string>();
            anagrams = anagrams.ToList();
            //var anagramsJoined = "'" + string.Join("','", anagrams) + "'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                var query = "SELECT Id FROM AnagramSolver.dbo.Word WHERE Word in ({0})";
                var index = 0;
                foreach (var anagram in anagrams)
                {
                    var paramName = "@anagramsList" + index;
                    cmd.Parameters.AddWithValue(paramName, anagram);
                    anagramsIdList.Add(paramName);
                    index++;
                }
                cmd.CommandText = String.Format(query, string.Join(",", anagramsIdList));
                // cmd.Parameters.Add(new SqlParameter("@anagramsList", anagramsJoined));
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        anagramsIdList.Clear();
                        while (reader.Read())
                        {
                            anagramsIdList.Add(reader["Id"].ToString());
                        }
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            return anagramsIdList.ConvertAll(int.Parse);
        }

        public List<string> GetCachedWords(string searchInput)
        {
            var anagramsList = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT Word FROM AnagramSolver.dbo.Word, AnagramSolver.dbo.CachedWord" +
                    " WHERE AnagramSolver.dbo.CachedWord.SearchWord = @searchInput and AnagramSolver.dbo.CachedWord.AnagramWordId = AnagramSolver.dbo.Word.Id", connection);
                cmd.Parameters.Add(new SqlParameter("@searchInput", searchInput));
                //var check = fillDB.checkIfTableIsEmpty();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        anagramsList.Add(reader["Word"].ToString());
                    }
                    reader.Close();
                }
                connection.Close();
            }
            return anagramsList;
        }

        public void InsertCachedWords(string searchInput, List<int> anagramsIdList)
        {
            var insertQuery = "INSERT INTO CachedWord (SearchWord, AnagramWordId)" +
                "SELECT @SearchWord, Id FROM AnagramSolver.dbo.Word WHERE AnagramSolver.dbo.Word.Id in (@IdsList) ";
            //var connectionString = "Server=LT-LIT-SC-0513;Database=AnagramSolver;Integrated Security = true;Uid=auth_windows";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
            {
                cmd.Parameters.Add(new SqlParameter("@SearchWord", searchInput));
                cmd.Parameters.Add(new SqlParameter("@IdsList", anagramsIdList));

                connection.Open();

                foreach (var item in anagramsIdList)
                {
                    cmd.Parameters[0].Value = searchInput;//item.Word;//item.Key;
                    cmd.Parameters[1].Value = item;//item.Category;//item.Value;

                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        //public List<string> PseudoCaching(string searchInput, List<int> anagramsIdList)
        //{
        //    var result = GetCachedWords(searchInput);
        //    if (result == null)
        //    {
        //        InsertCachedWords(searchInput, anagramsIdList);
        //        return null;
        //    }
        //    else
        //    {
        //        var anagramList = GetCachedWords(searchInput);
        //        return anagramList;
        //    }
        //}
    }
}

