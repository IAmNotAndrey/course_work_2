using MySqlConnector;
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
	public partial class StudentInputForm : Form
	{
		public User ChosenStudent { get; private set; }

		private DB db = new DB();
		private MySqlDataAdapter adapter = new MySqlDataAdapter();

		public StudentInputForm(TreeNode groupNode)
		{
			InitializeComponent();

			SetComboBox(groupNode);
		}

		private void SetComboBox(TreeNode groupNode)
		{
			string groupName = (string)groupNode.Tag;
			// Находим всех учащихся, которые никак не связаны с текущей группой
			var cmd = new MySqlCommand(
				//"SELECT u.* " +
				//"FROM `users` u " +
				//"LEFT JOIN `student_group_connections` c ON u.id = c.student " +
				//"LEFT JOIN groups g ON c.group = @groupName " +
				//"WHERE c.group IS NULL " +
				//"AND u.role = 'student'",
				"SELECT * " +
				"FROM `users` " +
				"WHERE `role` = 'student'" +
				"AND `id` NOT IN " +
				"(SELECT `student` FROM `student_group_connections` " +
				"WHERE `group` = @groupName)",
				db.GetConnection());
			cmd.Parameters.Add("@groupName", MySqlDbType.VarChar).Value = groupName;
			adapter.SelectCommand = cmd;
			var table = new DataTable();
			adapter.Fill(table);

			// Добалвяем найденных учеников в combobox
			foreach (var item in table.Rows)
			{
				var row = (DataRow)item;
				uint studentId = row.Field<uint>("id");
				var student = new User(studentId);
				comboBox1.Items.Add(student);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (comboBox1.SelectedItem == null)
			{
				MessageBox.Show("[!] Выберите элемент");
				return;
			}

			ChosenStudent = (User)comboBox1.SelectedItem;
			DialogResult = DialogResult.OK;
			Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			// Закрытие окна с результатом DialogResult.Cancel
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
