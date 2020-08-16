using System;
using System.Collections.Generic;
using System.Text;

namespace AnagramSolver.Contracts.Entities
{
    public partial class UserLogEntity
    {
        //Id fieldo nera DBFirst
        public int Id { get; set; }
        public string UserIp { get; set; }
        // Keiciam code first - SearchWord bus string, nelieka FK i CachedWord
        //public int SearchWordId { get; set; }
        public string SearchWord { get; set; }
        public DateTime SearchTime { get; set; }

        public string UserAction { get; set; }

        //Nuimam sita irgi
        //public virtual CachedWordEntity SearchWord { get; set; }
    }


    //public class UserLogEntity
    //{
    //    public string UserIp { get; set; }
    //    public int SearchWordId { get; set; }
    //    public TimeSpan SearchTime { get; set; }

    //}
}
