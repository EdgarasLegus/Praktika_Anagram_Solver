using AnagramSolver.Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace AnagramSolver.Interfaces
{
    public interface IWordRepository
    {
        //Dictionary<string, string> GetWords();
        List<WordModel> GetWords();
    }
}
