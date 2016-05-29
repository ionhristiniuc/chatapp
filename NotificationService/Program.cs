using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new NotificationServer(IPAddress.Parse("127.0.0.1"), 12345);
            server.Run();
        }
    }
}
