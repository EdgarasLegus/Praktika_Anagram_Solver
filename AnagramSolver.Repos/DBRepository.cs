using System;
using System.Collections.Generic;
using System.Text;
//using System.Data.Common.DBConnection;
using System.Data.SqlClient;
using System.Data;
using AnagramSolver.Interfaces;
using System.Linq;
using MySql.Data.MySqlClient;
using AnagramSolver.Contracts;

namespace AnagramSolver.Repos
{
    public class DBRepository : IWordRepository
    {
        private readonly string connectionString = "Server=LT-LIT-SC-0513;Database=AnagramSolver;" +
            "Integrated Security = true;Uid=auth_windows";
        private readonly IFillDB fillDB;

        public DBRepository()
        {
            fillDB = new FillDBRepository();
        }

        public List<WordModel> GetWords()
        {
            var wordModel = new List<WordModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM AnagramSolver.dbo.Word", connection);

                var check = fillDB.checkIfTableIsEmpty();

                if (check == true)
                {
                    throw new Exception("Error! AnagramSolver.dbo.Word table is empty!");
                }
                else
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
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
                }
            }
            return wordModel;
        }

    }
}

