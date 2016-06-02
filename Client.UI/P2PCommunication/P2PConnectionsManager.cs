using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ObjectBuilder2;

namespace Client.UI.P2PCommunication
{
    public class P2PConnectionsManager
    {
        public event MessageReceivedEventHandler MessageReceivedEvent;
        private IList<P2PConnection> _connections = new List<P2PConnection>(); 

        public bool IsConnected(string userId)
        {
            return _connections.Any(c => c.UserId == userId);
        }

        public bool SendMessage(string userId, string message)
        {
            var conn = _connections.FirstOrDefault(c => c.UserId == userId);

            if (conn == null)
                return false;

            return conn.SendMessage(message);
        }

        public bool ConnectTo(string userId)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            _connections.ForEach(c => c.Stop());
        }
    }

    public delegate void MessageReceivedEventHandler(string userId, string message);
}
