using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.UI.NSClient;
using Client.UI.NSClient.Jobs;
using DTO;
using DTO.Entities;
using Microsoft.Practices.ObjectBuilder2;

namespace Client.UI
{
    public partial class ClientForm : Form
    {
        private UserModel _user;
        private NSConnection _nsConnection;

        private readonly Color _onlineColor = Color.LightGreen;
        private readonly Color _offlineColor = Color.AliceBlue;

        public ClientForm(UserModel user)
        {                                
            InitializeComponent();            
            _user = user;
            PopulateFriends();                                 
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Text = _user.Id;

            _nsConnection = new NSConnection(_user);

            var progress = new Progress<IJob>(s => UpdateUI(s));
            Task.Factory.StartNew(() => _nsConnection.Process(progress),
                TaskCreationOptions.LongRunning);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            _nsConnection.Stop();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void UpdateUI(IJob job)
        {
            job.Execute(this);
        }

        private void PopulateFriends()
        {
            usersListView.BeginUpdate();
            _user.Friends.ForEach(u => usersListView.Items.Add(new ListViewItem() {Text = u.Id, BackColor = _offlineColor, Name = u.Id} ));
            usersListView.EndUpdate();
        }

        // Job helper methods
        public void SetFriendsOnline(string[] friends)
        {
            usersListView.BeginUpdate();
            //MessageBox.Show("Show friends online called");
            foreach (ListViewItem item in usersListView.Items)
            {
                if (friends.Contains(item.Text))
                    item.BackColor = _onlineColor;
            }
            usersListView.EndUpdate();
        }

        public void SetFriendOffline(string friend)
        {
            var item = usersListView.Items.Find(friend, false)
                .First();

            item.BackColor = _offlineColor;
        }
    }
}
