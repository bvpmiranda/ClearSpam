using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClearSpam
{
	public partial class MailBoxesForm : Form
	{
		private string[] _mailboxes;

		public event EventHandler<string> mailboxSelected;

		public string[] mailboxes
		{
			get { return _mailboxes; }

			set
			{
				_mailboxes = value;

				foreach (string m in _mailboxes)
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

			this.Close();
		}
	}
}
