using System;
using System.Collections.Generic;
using System.Text;
using AnagramSolver.EF.DatabaseFirst;
using AnagramSolver.Contracts.Entities;
using System.Linq;
using AnagramSolver.Contracts;
using AnagramSolver.Interfaces.DBFirst;
using System.Runtime.CompilerServices;
using AnagramSolver.EF.CodeFirst;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Web;

namespace AnagramSolver.BusinessLogic
{
    public class EFLogic : IEFLogic
    {
        //public string GetIP()
        //{
        //    var host = Dns.GetHostEntry(Dns.GetHostName());
        //    foreach (var ip in host.AddressList)
        //    {
        //        if (ip.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            return ip.ToString();
        //        }
        //    }
        //    throw new Exception("IP is not recognised!");
        //    //string strHostName = "";
        //    //IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
        //    //var addr = ipEntry.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        //    //var myIP = addr.First().ToString();

        //    //var myIP = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;
        //    //return myIP;
        //}


        public string GetIP()
        {
            //return HttpContext.Connection.RemoteIpAddress.ToString();
            return "::1";
        }

        // Gets the IP loopback address and converts it to a string.
        //String IpAddressString = IPAddress.Loopback.ToString();
        //    Console.WriteLine("Loopback IP address : " + IpAddressString);

        //string host = Dns.GetHostName();

        //IPAddress[] hostIPs = Dns.GetHostAddresses(host);
        //// get local IP addresses
        //IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

        //    // test if any host IP equals to any local IP or to localhost
        //    foreach (IPAddress hostIP in hostIPs)
        //    {
        //        // is localhost
        //        if (IPAddress.IsLoopback(hostIP)) Console.WriteLine(hostIP);
        //        // is local address
        //        foreach (IPAddress localIP in localIPs)
        //        {
        //            if (hostIP.Equals(localIP)) Console.WriteLine(localIP);
        //        }
    }
}
