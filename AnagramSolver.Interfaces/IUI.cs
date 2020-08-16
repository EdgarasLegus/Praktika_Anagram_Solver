using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Interfaces
{
    public interface IUI
    {
        string GetUserInput(int minInputWordLength);
        void ToFillTable();
        Task<string> RequestAPI(string inputForRequest);
    }
}