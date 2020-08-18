using AnagramSolver.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AnagramSolver.UI
{
    public delegate void Print(string msg);
    public delegate string FormattedPrint(string msg);


    public class Display : IDisplay
    {

        //private readonly Print _print;
        //private readonly FormattedPrint _formattedPrint;

        //public Display(Print print)
        //{
        //    _print = print;
        //    _formattedPrint = new FormattedPrint(Program.Capitalize);
        //}

        //public void FormattedPrint(FormattedPrint someDelegate, string input)
        //{
        //    _print(someDelegate(input));
        //}

        private readonly Action<string> _print;
        private readonly Func<string, string> _formattedPrint;

        public Display(Action<string> print)
        {
            _print = print;
            _formattedPrint = Program.Capitalize;
        }

        public void FormattedPrint(Func<string, string> someDelegate, string input)
        {
            _print(someDelegate(input));
        }


    }
}
