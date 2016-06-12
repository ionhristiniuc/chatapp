using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataServiceClient.Services;
using MetroFramework.Forms;

namespace Client.UI
{
    public partial class LogOnForm : MetroForm
    {        
        public LogOnForm()
        {
            InitializeComponent();
        }

        private async void connectBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.usernameTextBox.Text))
            {
                this.Hide();
                var usersSrv = new UsersService(null);
                var user = await usersSrv.Get(usernameTextBox.Text);

                if (user == null)
                {
                    MessageBox.Show(@"Invalid credentials", @"Log on failed", MessageBoxButtons.OK);
                    return;
                }

                var newForm = new ClientForm(user);
                newForm.Closed += (s, args) => this.Close();
                newForm.Show();
            }
            else
            {
                MessageBox.Show(@"Invalid credentials", @"Log on failed", MessageBoxButtons.OK);
            }
        }        
    }
}
