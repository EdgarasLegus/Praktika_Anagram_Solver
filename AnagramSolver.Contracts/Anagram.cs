using System;
using System.Collections.Generic;

namespace AnagramSolver.Contracts
{
    [Serializable]
    public class Anagram
    {
        public string Word { get; set; }
        public string PartOfSpeech { get; set; }

        public override string ToString()
        {
            return "\n" + $"{Word}, {PartOfSpeech}";
        }

    }
}