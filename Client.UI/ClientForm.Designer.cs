namespace Client.UI
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.usersListView = new System.Windows.Forms.ListView();
            this.selectedUserLabel = new MetroFramework.Controls.MetroLabel();
            this.sendButton = new MetroFramework.Controls.MetroButton();
            this.nametTitle = new MetroFramework.Controls.MetroTile();
            this.messagesTextArea = new MetroFramework.Drawing.Html.HtmlPanel();
            this.connectProgressBar = new MetroFramework.Controls.MetroProgressBar();
            this.SuspendLayout();
            // 
            // inputTextBox
            // 
            this.inputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputTextBox.Location = new System.Drawing.Point(206, 326);
            this.inputTextBox.Multiline = true;
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(481, 54);
            this.inputTextBox.TabIndex = 2;
            // 
            // usersListView
            // 
            this.usersListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.usersListView.Location = new System.Drawing.Point(12, 63);
            this.usersListView.Name = "usersListView";
            this.usersListView.Size = new System.Drawing.Size(188, 317);
            this.usersListView.TabIndex = 5;
            this.usersListView.UseCompatibleStateImageBehavior = false;
            this.usersListView.View = System.Windows.Forms.View.List;
            this.usersListView.SelectedIndexChanged += new System.EventHandler(this.usersListView_SelectedIndexChanged);
            // 
            // selectedUserLabel
            // 
            this.selectedUserLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedUserLabel.AutoSize = true;
            this.selectedUserLabel.Location = new System.Drawing.Point(634, 37);
            this.selectedUserLabel.Name = "selectedUserLabel";
            this.selectedUserLabel.Size = new System.Drawing.Size(0, 0);
            this.selectedUserLabel.TabIndex = 6;
            // 
            // sendButton
            // 
            this.sendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sendButton.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.sendButton.FontWeight = MetroFramework.MetroButtonWeight.Regular;
            this.sendButton.Location = new System.Drawing.Point(693, 327);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(106, 53);
            this.sendButton.TabIndex = 7;
            this.sendButton.Text = "Send";
            this.sendButton.UseSelectable = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // nametTitle
            // 
            this.nametTitle.ActiveControl = null;
            this.nametTitle.Location = new System.Drawing.Point(12, 37);
            this.nametTitle.Name = "nametTitle";
            this.nametTitle.Size = new System.Drawing.Size(188, 19);
            this.nametTitle.Style = MetroFramework.MetroColorStyle.Blue;
            this.nametTitle.TabIndex = 9;
            this.nametTitle.Text = "FirstName LastName";
            this.nametTitle.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Regular;
            this.nametTitle.UseSelectable = true;
            // 
            // messagesTextArea
            // 
            this.messagesTextArea.AllowDrop = true;
            this.messagesTextArea.AutoScroll = true;
            this.messagesTextArea.AutoScrollMinSize = new System.Drawing.Size(593, 0);
            this.messagesTextArea.BackColor = System.Drawing.SystemColors.Window;
            this.messagesTextArea.Location = new System.Drawing.Point(206, 63);
            this.messagesTextArea.Name = "messagesTextArea";
            this.messagesTextArea.Size = new System.Drawing.Size(593, 257);
            this.messagesTextArea.TabIndex = 118;
            // 
            // connectProgressBar
            // 
            this.connectProgressBar.Location = new System.Drawing.Point(693, 386);
            this.connectProgressBar.Name = "connectProgressBar";
            this.connectProgressBar.Size = new System.Drawing.Size(106, 16);
            this.connectProgressBar.TabIndex = 119;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 403);
            this.Controls.Add(this.connectProgressBar);
            this.Controls.Add(this.messagesTextArea);
            this.Controls.Add(this.nametTitle);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.selectedUserLabel);
            this.Controls.Add(this.usersListView);
            this.Controls.Add(this.inputTextBox);
            this.MinimumSize = new System.Drawing.Size(400, 250);
            this.Name = "ClientForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.ListView usersListView;
        private MetroFramework.Controls.MetroLabel selectedUserLabel;
        private MetroFramework.Controls.MetroButton sendButton;
        private MetroFramework.Controls.MetroTile nametTitle;
        private MetroFramework.Drawing.Html.HtmlPanel messagesTextArea;
        private MetroFramework.Controls.MetroProgressBar connectProgressBar;
    }
}

