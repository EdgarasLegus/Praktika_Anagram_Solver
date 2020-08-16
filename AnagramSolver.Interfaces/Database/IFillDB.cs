using AnagramSolver.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Interfaces
{
    public interface IFillDB
    {
        void FillDatabaseFromFile(WordModel wordModel);
        bool checkIfTableIsEmpty();
    }
}
