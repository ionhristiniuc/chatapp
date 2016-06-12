using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using P2PCommunicationLibrary.SuperPeer;

namespace SuperPeerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var superPeer = new SuperPeer(IPAddress.Parse("127.0.0.1"), 8899);
                superPeer.Run();                
            }
            catch (Exception e)
            {
                Console.WriteLine("An exception occurred: {0}", e);                
            }            
        }
    }
}
