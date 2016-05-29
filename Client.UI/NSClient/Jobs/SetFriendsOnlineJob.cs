using System.Windows.Forms;

namespace Client.UI.NSClient.Jobs
{
    public class SetFriendsOnlineJob : IJob
    {
        public string[] Friends { get; set; }

        public SetFriendsOnlineJob(params string[] friends)
        {
            Friends = friends;
        }

        public void Execute(ClientForm form)
        {
            form.SetFriendsOnline(Friends);            
        }
    }
}