using AnagramSolver.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Interfaces
{
    public interface IWordService
    {
        List<WordEntity> GetWordEntityFromFile();
    }
}
