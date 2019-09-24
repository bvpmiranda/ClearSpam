using System;
using System.Windows.Forms;

namespace ClearSpam
{
    public partial class MailBoxesForm : Form
    {
        private string[] _mailboxes;

        public event EventHandler<string> mailboxSelected;

        public string[] mailboxes
        {
            get => _mailboxes;

            set
            {
                _mailboxes = value;

                foreach (var m in _mailboxes)
                {
                    MailBoxesListBox.Items.Add(m);
                }
            }
        }

        public MailBoxesForm()
        {
            InitializeComponent();
        }

        private void MailBoxesListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            mailboxSelected?.Invoke(this, (string)MailBoxesListBox.SelectedItem);

            Close();
        }
    }
}
