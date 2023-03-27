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

namespace MusicSchoolApp.Forms
{
	public partial class StudentForm : Form
	{
		private EntryForm entryForm;
		private User student;

		public StudentForm(uint studentId, EntryForm entryForm)
		{
			InitializeComponent();
			this.FormClosing += (s, args) => Application.Exit();
			this.entryForm = entryForm;

			student = new User(studentId);

			studentNameField.Text = student.ToString();

			SetTrees((int)studentId);
		}

		private void SetTrees(int studentId)
		{
			treeView1.Nodes.Clear();

			var list = GetStudentNodes(studentId);

			foreach (var studentNode in list)
			{
				var parent = list.Find(node => node.NodeId == studentNode.ParentId);
				parent?.Nodes.Insert((int)studentNode.Priority, studentNode);
			}

			foreach (var item in list.FindAll(node => node.ParentId == null))
				treeView1.Nodes.Add(item);
		}

		public static List<StudentTaskTreeNode> GetStudentNodes(int studentId)
		{
			var db = new DB();
			var adapter = new MySqlDataAdapter();
			var list = new List<StudentTaskTreeNode>();

			var cmd = new MySqlCommand($"SELECT `node` FROM `student_node_connections` WHERE `student` = @studentId", db.GetConnection());
			cmd.Parameters.Add("@studentId", MySqlDbType.Int32).Value = studentId;
			adapter.SelectCommand = cmd;
			var table = new DataTable();
			adapter.Fill(table);

			foreach (var item in table.Rows)
			{
				var row = (DataRow)item;
				uint nodeId = row.Field<uint>("node");

				var studentNode = new StudentTaskTreeNode(studentId, (int)nodeId);
				list.Add(studentNode);
			}
			return list;
		}

		#region Обработчики событий
		/// <summary>
		/// Вызывается после выбора элемента
		/// </summary>
		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			var selectedNode = (StudentTaskTreeNode)treeView1.SelectedNode;
			nameField.Text = selectedNode.Text;
			teacherField.Text = new User(selectedNode.Owner).ToString();
			descriptionField.Text = selectedNode.Description;

			markField.Text = selectedNode.Mark.ToString();
			commentField.Text = selectedNode.Comment;
		}

		/// <summary>
		/// Обработка нажатия на пустую область
		/// </summary>
		private void treeView1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				var node = treeView1.GetNodeAt(e.X, e.Y);
				if (node == null)
				{
					treeView1.SelectedNode = null;
					// Делаем все поля пустыми
					nameField.Text = "";
					teacherField.Text = "";
					descriptionField.Text = "";
					markField.Text = "";
					commentField.Text = "";
				}
			}
		}
		#endregion

		private void logOutBtn_Click(object sender, EventArgs e)
		{
			var dialogRes = MessageBox.Show("Вы действительно хотите выйти из аккаунта?", "Выход из аккаунта", MessageBoxButtons.YesNo);
			if (dialogRes == DialogResult.No)
				return;

			entryForm.Show();
			// fixme здесь форма должна закрываться а не Hide()
			this.Hide();
			// Отписываемся, чтобы не закрывать всё приложение целиком
			//this.FormClosing -= (s, args) => Application.Exit();
			//this.Close();
		}
	}
}
