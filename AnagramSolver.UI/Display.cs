using AnagramSolver.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.UI
{
    public delegate void Print(string msg);

    public class Display : IDisplay
    {
        private readonly Print _print;

        public Display(Print print)
        {
            _print = print;
        }
    }
}
