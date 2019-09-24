using S22.Imap;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace ClearSpam
{
    public partial class ClearSpamForm : Form
    {
        private const string title = "Clear Spam";

        private enum field
        {
            from = 1,
            replyTo = 2,
            subject = 3,
            to = 4
        }

        public ClearSpamForm()
        {
            InitializeComponent();
        }

        private void frmClearSpam_Load(object sender, EventArgs e)
        {
            accountTableAdapter.Fill(clearSpamDataSet.Account);
            fieldTableAdapter.Fill(clearSpamDataSet.Field);
            ruleTableAdapter.Fill(clearSpamDataSet.Rule);

            if (clearSpamDataSet.Account.Count == 0)
            {
                EditAccountButton.Enabled = false;
                DeleteAccountButton.Enabled = false;

                ProcessRulesButton.Enabled = false;
            }
            else
            {
                ProcessRulesTimer.Interval = Properties.Settings.Default.TimerInterval;

                ProcessRulesTimer.Enabled = true;
            }

            IconCreditsLinkLabel.Links.Add(new LinkLabel.Link(13, 7, "http://www.flaticon.com/authors/freepik"));
            IconCreditsLinkLabel.Links.Add(new LinkLabel.Link(26, 8, "http://www.flaticon.com/free-icon/mail_194350"));
        }

        #region Accounts

        private void NewAccountButton_Click(object sender, EventArgs e)
        {
            AccountsBindingSource.AddNew();

            var row = ((DataRowView)AccountsBindingSource.Current).Row as ClearSpamDataSet.AccountRow;

            row.Port = 143;
            PortTextBox.Value = 143;

            row.SSL = false;
            SSLCheckBox.Checked = false;

            ServerTextBox.Enabled = true;
            PortTextBox.Enabled = true;
            SSLCheckBox.Enabled = true;
            LoginTextBox.Enabled = true;
            PasswordTextBox.Enabled = true;
            WatchedMailboxTextBox.Enabled = true;
            WatchedMailboxesButton.Enabled = true;
            TrashMailBoxTextBox.Enabled = true;
            TrashMailboxesButton.Enabled = true;

            ServerTextBox.Focus();

            NewAccountButton.Enabled = false;
            EditAccountButton.Enabled = false;
            DeleteAccountButton.Enabled = false;

            ProcessRulesButton.Enabled = false;

            CancelAccountButton.Enabled = true;
            SaveAccountButton.Enabled = true;

            AccountsGridView.Enabled = false;
        }
        private void EditAccountButton_Click(object sender, EventArgs e)
        {
            ServerTextBox.Enabled = true;
            PortTextBox.Enabled = true;
            SSLCheckBox.Enabled = true;
            LoginTextBox.Enabled = true;
            PasswordTextBox.Enabled = true;
            WatchedMailboxTextBox.Enabled = true;
            WatchedMailboxesButton.Enabled = true;
            TrashMailBoxTextBox.Enabled = true;
            TrashMailboxesButton.Enabled = true;

            ServerTextBox.Focus();

            NewAccountButton.Enabled = false;
            EditAccountButton.Enabled = false;
            DeleteAccountButton.Enabled = false;

            ProcessRulesButton.Enabled = false;

            CancelAccountButton.Enabled = true;
            SaveAccountButton.Enabled = true;

            AccountsGridView.Enabled = false;
        }
        private void DeleteAccountButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to Delete the Selected Account?", title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Enabled = false;
                Cursor = System.Windows.Forms.Cursors.WaitCursor;

                var account = ((DataRowView)AccountsBindingSource.Current).Row as ClearSpamDataSet.AccountRow;

                AccountsBindingSource.RemoveCurrent();
                accountTableAdapter.Update(clearSpamDataSet.Account);

                if (clearSpamDataSet.Account.Count == 0)
                {
                    EditAccountButton.Enabled = false;
                    DeleteAccountButton.Enabled = false;

                    ProcessRulesButton.Enabled = false;
                }

                Cursor = System.Windows.Forms.Cursors.Default;
                Enabled = true;
            }
        }

        private void PasswordTextBox_Leave(object sender, EventArgs e)
        {
            var row = ((DataRowView)AccountsBindingSource.Current).Row as ClearSpamDataSet.AccountRow;

            if (row.IsNull("Password") || (row.Password != PasswordTextBox.Text))
            {
                PasswordTextBox.Text = (new SimpleAES()).EncryptToString(PasswordTextBox.Text);
            }
        }

        private void WatchedMailboxesButton_Click(object sender, EventArgs e)
        {
            OpenMailboxesDialog((source, mailbox) => {
                WatchedMailboxTextBox.Text = mailbox;
            });
        }
        private void TrashMailBoxesButton_Click(object sender, EventArgs e)
        {
            OpenMailboxesDialog((source, mailbox) => {
                TrashMailBoxTextBox.Text = mailbox;
            });
        }
        private void OpenMailboxesDialog(System.EventHandler<string> mailboxSelected)
        {
            var frm = new MailBoxesForm();

            frm.mailboxSelected += mailboxSelected;

            var row = ((DataRowView)AccountsBindingSource.Current).Row as ClearSpamDataSet.AccountRow;

            try
            {
                var ic = new ImapClient(row.Server, row.Port, row.Login, (new SimpleAES()).DecryptString(row.Password), AuthMethod.Login, row.SSL);

                var mailboxes = ic.ListMailboxes().ToArray();

                frm.mailboxes = mailboxes;

                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                frm.Dispose();
            }
        }

        private void CancelAccountButton_Click(object sender, EventArgs e)
        {
            ServerTextBox.Enabled = false;
            PortTextBox.Enabled = false;
            SSLCheckBox.Enabled = false;
            LoginTextBox.Enabled = false;
            PasswordTextBox.Enabled = false;
            WatchedMailboxTextBox.Enabled = false;
            WatchedMailboxesButton.Enabled = false;
            TrashMailBoxTextBox.Enabled = false;
            TrashMailboxesButton.Enabled = false;

            AccountsBindingSource.CancelEdit();

            ServerTextBox.BackColor = System.Drawing.SystemColors.Window;
            PortTextBox.BackColor = System.Drawing.SystemColors.Window;
            LoginTextBox.BackColor = System.Drawing.SystemColors.Window;
            PasswordTextBox.BackColor = System.Drawing.SystemColors.Window;
            WatchedMailboxTextBox.BackColor = System.Drawing.SystemColors.Window;
            TrashMailBoxTextBox.BackColor = System.Drawing.SystemColors.Window;

            NewAccountButton.Enabled = true;
            EditAccountButton.Enabled = true;
            DeleteAccountButton.Enabled = true;

            ProcessRulesButton.Enabled = true;

            CancelAccountButton.Enabled = false;
            SaveAccountButton.Enabled = false;

            AccountsGridView.Enabled = true;
        }
        private void SaveAccountButton_Click(object sender, EventArgs e)
        {
            if (!AccountValid())
                return;

            var row = ((DataRowView)AccountsBindingSource.Current).Row as ClearSpamDataSet.AccountRow;

            ServerTextBox.Enabled = false;
            PortTextBox.Enabled = false;
            SSLCheckBox.Enabled = false;
            LoginTextBox.Enabled = false;
            PasswordTextBox.Enabled = false;
            WatchedMailboxTextBox.Enabled = false;
            WatchedMailboxesButton.Enabled = false;
            TrashMailBoxTextBox.Enabled = false;
            TrashMailboxesButton.Enabled = false;

            AccountsBindingSource.EndEdit();
            accountTableAdapter.Update(clearSpamDataSet.Account);

            NewAccountButton.Enabled = true;
            EditAccountButton.Enabled = true;
            DeleteAccountButton.Enabled = true;

            ProcessRulesButton.Enabled = true;

            CancelAccountButton.Enabled = false;
            SaveAccountButton.Enabled = false;

            AccountsGridView.Enabled = true;
        }
        private bool AccountValid()
        {
            var ret = true;

            ServerTextBox.BackColor = System.Drawing.SystemColors.Window;
            PortTextBox.BackColor = System.Drawing.SystemColors.Window;
            LoginTextBox.BackColor = System.Drawing.SystemColors.Window;
            PasswordTextBox.BackColor = System.Drawing.SystemColors.Window;
            WatchedMailboxTextBox.BackColor = System.Drawing.SystemColors.Window;
            TrashMailBoxTextBox.BackColor = System.Drawing.SystemColors.Window;

            Control focusedControl = null;

            if (string.IsNullOrWhiteSpace(ServerTextBox.Text))
            {
                ServerTextBox.BackColor = Color.MistyRose;

                if (focusedControl == null)
                    focusedControl = ServerTextBox;

                ret = false;
            }

            if (string.IsNullOrWhiteSpace(PortTextBox.Text))
            {
                PortTextBox.BackColor = Color.MistyRose;

                if (focusedControl == null)
                    focusedControl = PortTextBox;

                ret = false;
            }

            if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                LoginTextBox.BackColor = Color.MistyRose;

                if (focusedControl == null)
                    focusedControl = LoginTextBox;

                ret = false;
            }

            if (string.IsNullOrWhiteSpace(PasswordTextBox.Text))
            {
                PasswordTextBox.BackColor = Color.MistyRose;

                if (focusedControl == null)
                    focusedControl = PasswordTextBox;

                ret = false;
            }

            if (string.IsNullOrWhiteSpace(WatchedMailboxTextBox.Text))
            {
                WatchedMailboxTextBox.BackColor = Color.MistyRose;

                if (focusedControl == null)
                    focusedControl = WatchedMailboxTextBox;

                ret = false;
            }

            if (string.IsNullOrWhiteSpace(TrashMailBoxTextBox.Text))
            {
                TrashMailBoxTextBox.BackColor = Color.MistyRose;

                if (focusedControl == null)
                    focusedControl = TrashMailBoxTextBox;

                ret = false;
            }


            if (ret)
            {
                var row = ((DataRowView)AccountsBindingSource.Current).Row as ClearSpamDataSet.AccountRow;

                if (clearSpamDataSet.Account.Where(a => a.Server == row.Server && a.Login == row.Login && a.WatchedMailbox == row.WatchedMailbox && a.Id != row.Id).Count() > 0)
                {
                    ServerTextBox.BackColor = Color.MistyRose;
                    LoginTextBox.BackColor = Color.MistyRose;
                    WatchedMailboxTextBox.BackColor = Color.MistyRose;

                    if (focusedControl == null)
                        focusedControl = WatchedMailboxTextBox;

                    ret = false;
                }
            }

            if (!ret)
                focusedControl.Focus();

            return ret;
        }

        private void AccountsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            var id = 0;

            if (AccountsBindingSource.Current != null)
                id = ((ClearSpamDataSet.AccountRow)((DataRowView)AccountsBindingSource.Current).Row).Id;

            RulesBindingSource.Filter = "AccountId = " + id.ToString();
        }

        #endregion

        #region Rules

        private void NewRuleButton_Click(object sender, EventArgs e)
        {
            RulesBindingSource.AddNew();

            var accountRow = ((DataRowView)AccountsBindingSource.Current).Row as ClearSpamDataSet.AccountRow;

            var row = ((DataRowView)RulesBindingSource.Current).Row as ClearSpamDataSet.RuleRow;

            row.AccountId = accountRow.Id;

            FieldComboBox.SelectedIndex = 0;
            row.FieldId = (int)FieldComboBox.SelectedValue;

            FieldComboBox.Enabled = true;
            ContentTextBox.Enabled = true;

            ContentTextBox.Focus();

            NewRuleButton.Enabled = false;
            EditRuleButton.Enabled = false;
            DeleteRuleButton.Enabled = false;

            CancelRuleButton.Enabled = true;
            SaveRuleButton.Enabled = true;

            RulesGridView.Enabled = false;
        }
        private void EditRuleButton_Click(object sender, EventArgs e)
        {
            FieldComboBox.Enabled = true;
            ContentTextBox.Enabled = true;

            ContentTextBox.Focus();

            NewRuleButton.Enabled = false;
            EditRuleButton.Enabled = false;
            DeleteRuleButton.Enabled = false;

            CancelRuleButton.Enabled = true;
            SaveRuleButton.Enabled = true;

            RulesGridView.Enabled = false;
        }
        private void DeleteRuleButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Delete the Selected Rule?", title, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                RulesBindingSource.RemoveCurrent();
                ruleTableAdapter.Update(clearSpamDataSet.Rule);

                if (clearSpamDataSet.Rule.Count == 0)
                {
                    EditRuleButton.Enabled = false;
                    DeleteRuleButton.Enabled = false;
                }
            }
        }

        private void CancelRuleButton_Click(object sender, EventArgs e)
        {
            FieldComboBox.Enabled = false;
            ContentTextBox.Enabled = false;

            RulesBindingSource.CancelEdit();

            FieldComboBox.BackColor = System.Drawing.SystemColors.Window;
            ContentTextBox.BackColor = System.Drawing.SystemColors.Window;

            NewRuleButton.Enabled = true;
            EditRuleButton.Enabled = true;
            DeleteRuleButton.Enabled = true;

            CancelRuleButton.Enabled = false;
            SaveRuleButton.Enabled = false;

            RulesGridView.Enabled = true;
        }
        private void SaveRuleButton_Click(object sender, EventArgs e)
        {
            if (!RuleValid())
                return;

            FieldComboBox.Enabled = false;
            ContentTextBox.Enabled = false;

            RulesBindingSource.EndEdit();
            ruleTableAdapter.Update(clearSpamDataSet.Rule);

            NewRuleButton.Enabled = true;
            EditRuleButton.Enabled = true;
            DeleteRuleButton.Enabled = true;

            CancelRuleButton.Enabled = false;
            SaveRuleButton.Enabled = false;

            RulesGridView.Enabled = true;
        }
        private bool RuleValid()
        {
            var ret = true;

            FieldComboBox.BackColor = System.Drawing.SystemColors.Window;
            ContentTextBox.BackColor = System.Drawing.SystemColors.Window;

            Control focusedControl = null;

            if (FieldComboBox.SelectedValue == null)
            {
                FieldComboBox.BackColor = Color.MistyRose;

                if (focusedControl == null)
                    focusedControl = FieldComboBox;

                ret = false;
            }

            if (string.IsNullOrWhiteSpace(ContentTextBox.Text))
            {
                ContentTextBox.BackColor = Color.MistyRose;

                if (focusedControl == null)
                    focusedControl = ContentTextBox;

                ret = false;
            }

            if (ret)
            {
                var accountRow = ((DataRowView)AccountsBindingSource.Current).Row as ClearSpamDataSet.AccountRow;
                var row = ((DataRowView)RulesBindingSource.Current).Row as ClearSpamDataSet.RuleRow;


                if (clearSpamDataSet.Rule.Where(r => r.AccountId == accountRow.Id && r.FieldId == row.FieldId && r.Content == row.Content && r.Id != row.Id).Count() > 0)
                {
                    ContentTextBox.BackColor = Color.MistyRose;

                    if (focusedControl == null)
                        focusedControl = ContentTextBox;

                    ret = false;
                }
            }

            if (!ret)
                focusedControl.Focus();

            return ret;
        }

        #endregion

        #region Process Rules

        private void ProcessRulesButton_Click(object sender, EventArgs e)
        {
            ProcessRules();
        }

        private void ProcessRules()
        {
            ProcessRulesTimer.Enabled = false;

            Enabled = false;
            Cursor = System.Windows.Forms.Cursors.WaitCursor;
            contextMenuStrip.Enabled = false;

            var accountsCount = clearSpamDataSet.Account.Count;

            AccountsProgressBar.Value = 0;
            AccountsProgressBar.Maximum = accountsCount;

            RulesProgressBar.Value = 0;

            for (var a = 0; a < accountsCount; a++)
            {
                ProcessRules(clearSpamDataSet.Account[a]);

                AccountsProgressBar.Value += 1;

                Application.DoEvents();
            }

            contextMenuStrip.Enabled = true;
            Cursor = System.Windows.Forms.Cursors.Default;
            Enabled = true;

            ProcessRulesTimer.Enabled = true;
        }

        private void ProcessRules(ClearSpamDataSet.AccountRow account)
        {
            ImapClient ic = null;

            try
            {
                ic = new ImapClient(account.Server, account.Port, account.Login, (new SimpleAES()).DecryptString(account.Password), AuthMethod.Login, account.SSL)
                {
                    DefaultMailbox = account.WatchedMailbox
                };

                var rules = clearSpamDataSet.Rule.Where(r => r.AccountId == account.Id).ToArray();

                var info = ic.GetMailboxInfo();

                var messages = ic.Search(SearchCondition.Undeleted()).ToArray();

                MailMessage message;

                var messagesCount = messages.Length;

                RulesProgressBar.Value = 0;
                RulesProgressBar.Maximum = messagesCount;

                if (messagesCount > 0)
                {
                    for (var m = 0; m < messagesCount; m++)
                    {
                        message = ic.GetMessage(messages[m], false);

                        if (ProcessRules(message, rules))
                        {
                            ic.MoveMessage(messages[m], account.TrashMailBox);
                        }

                        RulesProgressBar.Value += 1;

                        Application.DoEvents();
                    }
                }
                else
                {
                    RulesProgressBar.Maximum = 1;
                    RulesProgressBar.Value = 1;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                try
                {
                    ic.Logout();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
                finally
                {
                    try
                    {
                        ic.Dispose();
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                    }
                }
            }
        }

        private bool ProcessRules(MailMessage message, ClearSpamDataSet.RuleRow[] rules)
        {
            var ret = false;

            foreach (var r in rules)
            {
                switch (r.FieldId)
                {
                    case (int)field.from:
                        ret = ProcessMessageFrom(message, r.Content);
                        break;

                    case (int)field.replyTo:
                        ret = ProcessMessageReplyTo(message, r.Content);
                        break;

                    case (int)field.subject:
                        ret = ProcessMessageSubject(message, r.Content);
                        break;

                    case (int)field.to:
                        ret = ProcessMessageTo(message, r.Content);
                        break;
                }

                if (ret)
                    break;
            }

            return ret;
        }

        private bool ProcessMessageFrom(MailMessage message, string content)
        {
            var from = string.Join("", message.Headers.GetValues("From")).Replace("\"", "");

            if (from.Contains(content))
            {
                return true;
            }

            return false;
        }

        private bool ProcessMessageReplyTo(MailMessage message, string content)
        {
            foreach (var ma in message.ReplyToList)
            {
                if (ma.Address.Contains(content) || ma.DisplayName.Contains(content))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ProcessMessageSubject(MailMessage message, string content)
        {
            var contentEncoding = Encoding.UTF8;
            var contentBytes = contentEncoding.GetBytes(content);

            var subjectEncoding = message.SubjectEncoding;
            var subjectBytes = Encoding.Convert(contentEncoding, subjectEncoding, contentBytes);

            var subject = subjectEncoding.GetString(subjectBytes);

            if (message.Subject.Contains(subject))
            {
                return true;
            }

            return false;
        }

        private bool ProcessMessageTo(MailMessage message, string content)
        {
            foreach (var ma in message.To)
            {
                if (ma.Address.Contains(content) || ma.DisplayName.Contains(content))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Notifications Icon

        private void ClearSpamForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.ApplicationExitCall)
                return;

            e.Cancel = true;

            Hide();
            ShowInTaskbar = false;
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
            else
                Show();

            Activate();
            ShowInTaskbar = true;
        }

        private void processRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessRules();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion

        private void IconCreditsLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(e.Link.LinkData as string);
        }

        private void ProcessRulesTimer_Tick(object sender, EventArgs e)
        {
            ProcessRules();
        }

    }
}
