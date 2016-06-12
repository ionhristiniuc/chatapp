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
using Client.UI.P2PCommunication;
using DTO;
using DTO.Entities;
using Microsoft.Practices.ObjectBuilder2;

namespace Client.UI
{
    public partial class ClientForm : Form
    {
        private UserModel _user;
        private NSConnection _nsConnection;
        private P2PConnectionsManager _p2PConnectionsManager;
        private Dictionary<string, Conversation> _conversations; 

        private readonly Color _onlineColor = Color.LightGreen;
        private readonly Color _offlineColor = Color.White;

        private Dictionary<string, string> _connectionMessages = new Dictionary<string, string>();
        private string SelectedUser { get; set; }

        public ClientForm(UserModel user)
        {                                
            InitializeComponent();            
            _user = user;
            _conversations = _user.Friends.ToDictionary(f => f.Id, f => new Conversation()
            {
                UserId = f.Id, Input = string.Empty, TextArea = string.Empty
            });
            PopulateFriends();                                 
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Text = $"{_user.Id}: {_user.FirstName} {_user.LastName}";

            _nsConnection = new NSConnection(_user);

            _p2PConnectionsManager = new P2PConnectionsManager(_user, _nsConnection);
            _p2PConnectionsManager.MessageReceivedEvent += MessageReceivedHandler;
            _p2PConnectionsManager.ConnectedToFriendEvent += P2PConnectionsManagerOnConnectedToFriendEvent;

            var progress = new Progress<IJob>(s => UpdateUI(s));
            Task.Factory.StartNew(() => _nsConnection.Process(progress),
                TaskCreationOptions.LongRunning);
        }

        private void P2PConnectionsManagerOnConnectedToFriendEvent(string userId)
        {
            var message = _connectionMessages[userId];

            if (_p2PConnectionsManager.SendMessage(userId, message))
            {
                PrintMessage(_user.Id, message);
                this.Invoke((MethodInvoker) delegate
                {
                    inputTextBox.Text = string.Empty;
                });
            }
        }

        private void MessageReceivedHandler(string userId, string message)
        {              
            PrintMessage(userId, message);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            _p2PConnectionsManager.Stop();
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
            _user.Friends.ForEach(u => usersListView.Items.Add(
                new ListViewItem() {Text = u.FirstName + " " + u.LastName, BackColor = _offlineColor, Name = u.Id} ));
            usersListView.EndUpdate();
        }

        public void SetFriendsOnline(string[] friends)
        {
            usersListView.BeginUpdate();
            foreach (ListViewItem item in usersListView.Items)
            {
                if (friends.Contains(item.Name))
                    item.BackColor = _onlineColor;
            }
            usersListView.EndUpdate();
        }

        public void SetFriendOffline(string friend)
        {
            var item = usersListView.Items.Find(friend, false)
                .First();

            item.BackColor = _offlineColor;

            _p2PConnectionsManager.RemoveConnectionIfAny(friend);
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            var selectedUserId = GetSelectedUserId();

            if (selectedUserId == null)
                return;

            var message = inputTextBox.Text;

            if (string.IsNullOrWhiteSpace(message))
                return;

            if (!_p2PConnectionsManager.IsConnected(selectedUserId))
            {
                _connectionMessages[selectedUserId] = message;

                if (!_p2PConnectionsManager.StartConnectTo(selectedUserId))
                {
                    MessageBox.Show("Failed to connect to " + selectedUserId);
                    return;
                }
            }
            else
            {
                if (_p2PConnectionsManager.SendMessage(selectedUserId, message))
                {
                    PrintMessage(_user.Id, inputTextBox.Text);                    
                    inputTextBox.Text = string.Empty;                    
                }
                else
                    MessageBox.Show("Failed to send message to " + selectedUserId);
            }
        }

        private void PrintMessage(string userId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            var user = _user.Id == userId ? _user : _user.Friends.First(u => u.Id == userId);

            this.Invoke((MethodInvoker)delegate 
            {
                if (userId == _user.Id)
                {
                    messagesTextArea.Text = messagesTextArea.Text + '\n'
                                            + $"Me> {message}";
                } 
                else if (SelectedUser != null && SelectedUser == userId)
                {
                    messagesTextArea.Text = messagesTextArea.Text + '\n'
                                            + $"{user.FirstName} {user.LastName}> {message}";
                }
                else
                {
                    var conv = _conversations[userId];
                    conv.TextArea +=  '\n' + $"{user.FirstName} {user.LastName}> {message}";
                    // mark as new message received
                }
            });            
        }

        private void usersListView_SelectedIndexChanged(object sender, EventArgs e)
        {            
            var selected = GetSelectedUserId();
                             
            if (selected != null && selected != SelectedUser)
            {
                var lastSelUser = SelectedUser;

                if (lastSelUser != null)
                {
                    SaveConversation(lastSelUser);
                }

                ShowConversation(selected);
            }
            else if (selected == null && SelectedUser != null)
            {
                SaveConversation(SelectedUser);
                ClearInputs();
            }

            selectedUserLabel.Text = string.Empty;
            SelectedUser = selected;
            if (selected != null)
            {
                var user = GetFriend(selected);
                selectedUserLabel.Text = user.FirstName + " " + user.LastName;
            }                                        
        }

        private UserModel GetFriend(string userId)
        {
            return _user.Friends.FirstOrDefault(f => f.Id == userId);
        }

        private void ClearInputs()
        {
            inputTextBox.Text = string.Empty;
            messagesTextArea.Text = string.Empty;
        }

        private void ShowConversation(string selected)
        {
            var selConv = _conversations[selected];
            inputTextBox.Text = selConv.Input;
            messagesTextArea.Text = selConv.TextArea;
        }

        private void SaveConversation(string lastSelUser)
        {
            var lastSelUserConv = _conversations[lastSelUser];
            lastSelUserConv.TextArea = messagesTextArea.Text;
            lastSelUserConv.Input = inputTextBox.Text;
        }

        private string GetSelectedUserId()
        {
            return usersListView.SelectedItems.Count != 0
                   ? usersListView.SelectedItems[0].Name
                   : null;
        }
    }
}
