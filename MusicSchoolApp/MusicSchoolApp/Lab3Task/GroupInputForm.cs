using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicSchoolApp.Lab3Task
{
	public partial class GroupInputForm : Form
	{
		public string GroupName { get; set; }

		public GroupInputForm()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			GroupName = textBox1.Text.Trim();
			DialogResult = DialogResult.OK;
			Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			// Закрытие окна с результатом DialogResult.Cancel
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
