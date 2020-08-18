using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.UI
{
    public interface IDisplay
    {
        //void FormattedPrint(FormattedPrint someDelegate, string input);
        void FormattedPrint(Func<string, string> someDelegate, string input);
    }
}
