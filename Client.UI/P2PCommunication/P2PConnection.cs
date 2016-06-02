using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.P2PCommunication
{
    public abstract class P2PConnection
    {
        public string UserId { get; set; }

        public abstract bool SendMessage(string message);

        public abstract void Stop();
    }
}
