using AnagramSolver.Contracts.Entities;
using AnagramSolver.EF.CodeFirst;
using System.Linq;
using AnagramSolver.EF.DatabaseFirst;
using AnagramSolver.Interfaces;
using AnagramSolver.Interfaces.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Repos.EF
{
    public class EFCachedWordRepository : IEFCachedWordRepo
    {
        //private readonly AnagramSolverDBFirstContext _context;
        private readonly AnagramSolverCodeFirstContext _context;
        //public EFLogic(AnagramSolverDBFirstContext context)
        //{
        //    _context = context;
        //}

        public EFCachedWordRepository(AnagramSolverCodeFirstContext context)
        {
            _context = context;
        }

        public List<string> GetCachedWords(string searchInput)
        {
            //var cachedwords = _context.CachedWord.Where(x => x.SearchWord == searchInput).ToList();
            //var cachedWordList = cachedwords.Select(x => x.SearchWord).ToList();
            var cachedAnagramList = from w in _context.Word
                                    from cw in _context.CachedWord
                                    where (cw.SearchWord == searchInput) && (cw.AnagramWordId == w.Id)
                                    select w.Word1;
            return cachedAnagramList.ToList();
        }

        public void InsertCachedWords(string searchInput, List<int> anagramsIdList)
        {
            var cachedWordEntity = new CachedWordEntity();
            foreach (var item in anagramsIdList)
            {
                cachedWordEntity = new CachedWordEntity
                {
                    SearchWord = searchInput,
                    AnagramWordId = item
                };
                _context.CachedWord.Add(cachedWordEntity);
            }
            //_context.CachedWord.Add(cachedWordEntity);
            _context.SaveChanges();
        }
    }
}

