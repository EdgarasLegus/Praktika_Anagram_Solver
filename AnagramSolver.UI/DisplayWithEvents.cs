using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.UI
{
    public class DisplayWithEvents : IDisplay
    {
        public event Print eventPrinting;
        private readonly Func<string, string> _formattedPrint;

        public DisplayWithEvents()
        {
            _formattedPrint = Program.Capitalize;
        }

        public void FormattedPrint(Func<string, string> someDelegate, string input)
        {
            eventPrinting(someDelegate(input));
        }

        //public void StartProcess(string msg)
        //{
        //    _print("Process Started!");
        //    OnProcessCompleted(msg);
        //}

        //protected virtual void OnProcessCompleted(string msg)
        //{
        //    ProcessCompleted?.Invoke(msg);
        //}
    }
}
