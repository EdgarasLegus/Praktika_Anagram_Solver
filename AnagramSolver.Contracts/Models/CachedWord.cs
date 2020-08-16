using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts
{
    public class CachedWord
    {
        public int Id { get; set; }
        public string SearchWord { get; set; }
        public int AnagramWordId { get; set; }

    }
}