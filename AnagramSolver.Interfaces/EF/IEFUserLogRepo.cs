using AnagramSolver.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Interfaces.EF
{
    public interface IEFUserLogRepo
    {
        void InsertUserLog(string word, string ip, UserAction userAction);
        int CheckUserLogActions(string ip, UserAction userAction);
    }
}