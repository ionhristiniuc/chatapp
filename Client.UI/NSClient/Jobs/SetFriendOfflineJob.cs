using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.UI.NSClient.Jobs
{
    class SetFriendOfflineJob : IJob
    {
        public string Friend { get; set; }

        public SetFriendOfflineJob(string friend)
        {
            Friend = friend;
        }

        public void Execute(ClientForm form)
        {
            form.SetFriendOffline(Friend);
        }
    }
}
