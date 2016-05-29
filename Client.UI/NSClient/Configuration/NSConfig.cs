using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.NSClient.Configuration
{
    static class NSConfig
    {
        public static readonly string Url = ConfigurationManager.AppSettings["NSUrl"];
        public static readonly int Port = Convert.ToInt32(ConfigurationManager.AppSettings["NSPort"]);
    }
}
