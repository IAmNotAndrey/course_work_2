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
	public partial class TaskInputForm : Form
	{
		private DB db = new DB();
		private MySqlDataAdapter adapter = new MySqlDataAdapter();

		public (TaskTreeNode, int?, string?) ChosenTaskWithParams { get; private set; }

		public TaskInputForm(TreeNode studentNode)
		{
			InitializeComponent();

			SetComboBox(studentNode);
		}

		private void SetComboBox(TreeNode studentNode)
		{
			uint studentId = (uint)studentNode.Tag;
			// Находим все занятия, которые никак не связаны с текущим студентом
			var cmd = new MySqlCommand(
				"SELECT * " +
				"FROM `nodes` " +
				"WHERE `id` NOT IN " +
				"(SELECT `node` FROM `student_node_connections` " +
				"WHERE `student` = @studentId)",
				db.GetConnection());
			cmd.Parameters.Add("@studentId", MySqlDbType.UInt32).Value = studentId;
			adapter.SelectCommand = cmd;
			var table = new DataTable();
			adapter.Fill(table);

			// Добалвяем найденные задания в combobox
			foreach (var item in table.Rows)
			{
				var row = (DataRow)item;
				uint nodeId = row.Field<uint>("id");
				var node = new TaskTreeNode((int)nodeId);
				comboBox1.Items.Add(node);
			}
		}

		private void okBtn_Click(object sender, EventArgs e)
		{
			if (comboBox1.SelectedItem == null)
			{
				MessageBox.Show("[!] Выберите элемент");
				return;
			}
			int? mark;
			if (!TryParseExtensions.TryParseNullableInt(markField.Text, out mark))
			{
				MessageBox.Show("[!] Выберите корректное значение оценки");
				return;
			}

			ChosenTaskWithParams = ((TaskTreeNode)comboBox1.SelectedItem, mark, commentField.Text);
			DialogResult = DialogResult.OK;
			Close();
		}

		private void cancelBtn_Click(object sender, EventArgs e)
		{
			// Закрытие окна с результатом DialogResult.Cancel
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
