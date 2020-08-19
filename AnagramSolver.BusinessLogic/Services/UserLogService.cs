using AnagramSolver.Contracts.Enums;
using AnagramSolver.Interfaces;
using AnagramSolver.Interfaces.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.BusinessLogic.Services
{
    public class UserLogService : IUserLogService
    {
        private readonly IEFUserLogRepo _efUserLogRepository;

        public UserLogService(IEFUserLogRepo efUserLogRepository)
        {
            _efUserLogRepository = efUserLogRepository;
        }

        public string ValidateUserLog(string ip)
        {
            var ipCountSearch = _efUserLogRepository.CheckUserLogActions(ip, UserAction.Search);
            var ipCountAdd = _efUserLogRepository.CheckUserLogActions(ip, UserAction.Add);
            var ipCountRemove = _efUserLogRepository.CheckUserLogActions(ip, UserAction.Remove);
            var ipCountUpdate = _efUserLogRepository.CheckUserLogActions(ip, UserAction.Update);

            var maxSearchesForIP = Contracts.Settings.GetSettingsMaxSearchesForIP();
            if ((ipCountSearch - ipCountAdd + ipCountRemove - ipCountUpdate) >= maxSearchesForIP)
            {
                var validation = "failed";
                return validation;
            }
            else
            {
                var validation = "ok";
                return validation;
            }
        }
    }
}
