namespace ClearSpam
{
	partial class MailBoxesForm
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
			this.MailBoxesListBox = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// MailBoxesListBox
			// 
			this.MailBoxesListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MailBoxesListBox.FormattingEnabled = true;
			this.MailBoxesListBox.Location = new System.Drawing.Point(0, 0);
			this.MailBoxesListBox.Name = "MailBoxesListBox";
			this.MailBoxesListBox.Size = new System.Drawing.Size(284, 261);
			this.MailBoxesListBox.TabIndex = 1;
			this.MailBoxesListBox.SelectedValueChanged += new System.EventHandler(this.MailBoxesListBox_SelectedValueChanged);
			// 
			// MailBoxesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.MailBoxesListBox);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MailBoxesForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Mailboxes";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox MailBoxesListBox;
	}
}