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
	/*
	 * Форма создана для выполняния Лабораторной работы 3 по Консутрированию ПО
	 * 
	 * Форма представляет собой дерево с CRUD-функионалом для работы с БД
	 * 
	 * Используемые сущности: Группы -> Студенты (Из `Users`) -> Задания студентов (Из `student_node_connections`)
	 */

	// note Один и тот же студент может находиться в нескольких группах одноверменно, поэтому для демонстрации стоит опустить этот момент

	public partial class Lab3TaskForm : Form
	{
		private delegate void ChangeHandler(object sender, EventArgs e);
		private event ChangeHandler ChangeNotify;

		private DB db = new DB();
		MySqlDataAdapter adapter = new MySqlDataAdapter();

		public Lab3TaskForm()
		{
			InitializeComponent();

			ChangeNotify += TreeChanged;

			TreeViewUpdate();
		}

		private void TreeViewUpdate()
		{
			treeView1.Nodes.Clear();

			GroupSetting();
			// После установки всех групп итерируемся по ним и выставляем к ним всех учеников
			foreach (var rGroupNode in treeView1.Nodes)
			{
				var groupNode = (TreeNode)rGroupNode;
				StudentSetting(ref groupNode);

				// После установки всех студентов итерируемся по ним и выставляем все задания
				foreach (var rStudentNode in groupNode.Nodes)
				{
					var studentNode = (TreeNode)rStudentNode;
					TaskSetting(ref studentNode);
				}
			}
		}

		#region Установка вершин дерева
		private void GroupSetting()
		{
			// Находим все группы
			var cmd = new MySqlCommand(
				"SELECT * FROM `groups`",
				db.GetConnection());
			var table = new DataTable();
			adapter.SelectCommand = cmd;
			adapter.Fill(table);

			// Устанавливаем группы в 0-й (корневой) уровень дерева
			foreach (var item in table.Rows)
			{
				var row = (DataRow)item;
				string groupName = row.Field<string>("name");
				var node = new TreeNode(groupName);
				// Установим `groupName` в `Tag` вершины
				node.Tag = groupName;
				treeView1.Nodes.Add(node);
			}
		}

		/// <summary>
		/// Устанавливает вершины-потомки студентов у конкретной группы
		/// </summary>
		private void StudentSetting(ref TreeNode groupNode)
		{
			string groupName = (string)groupNode.Tag;
			// Находим всех студентов данной группы
			var cmd = new MySqlCommand(
				"SELECT * FROM `student_group_connections` " +
				"WHERE `group` = @groupName",
				db.GetConnection());
			cmd.Parameters.Add("@groupName", MySqlDbType.VarChar).Value = groupName;
			var table = new DataTable();
			adapter.SelectCommand = cmd;
			adapter.Fill(table);

			// Устанавливаем студентов в 1-й уровень дерева, но для конкретной вершины
			foreach (var item in table.Rows)
			{
				var row = (DataRow)item;
				// Берём `id` студента
				uint studentId = row.Field<uint>("student");
				// Для понятного отображения идём в таблицу `users` и берём оттуда ФИО студента
				cmd = new MySqlCommand(
					"SELECT `first_name`, `surname`, `patronymic` FROM `users` " +
					"WHERE `id` = @studentId",
					db.GetConnection());
				cmd.Parameters.Add("@studentId", MySqlDbType.UInt32).Value = studentId;
				table = new DataTable();
				adapter.SelectCommand = cmd;
				adapter.Fill(table);
				// Получаем только одно совпадение по первичному ключу
				string studentFirstName = table.Rows[0].Field<string>("first_name");
				string studentSurname = table.Rows[0].Field<string>("surname");
				string studentPatronymic = table.Rows[0].Field<string>("patronymic");
				string studentFullName = $"{studentSurname} {studentFirstName} {studentPatronymic}";

				// Устанавливаем ФИО студента в качестве `Text` вершины
				var node = new TreeNode(studentFullName);
				// Кроме того, устанавливаем в качестве `Tag` `id` студента для удобства выполнения последующих операций
				node.Tag = studentId;
				// Добавляем подвершину студента к вершине группы
				groupNode.Nodes.Add(node);
			}
		}

		private void TaskSetting(ref TreeNode studentNode)
		{
			uint studentId = (uint)studentNode.Tag;
			// Находим все задания для студента с заданным `id`
			var cmd = new MySqlCommand(
				"SELECT * FROM `student_node_connections` " +
				"WHERE `student` = @studentId",
				db.GetConnection());
			cmd.Parameters.Add("@studentId", MySqlDbType.UInt32).Value = studentId;
			var table = new DataTable();
			adapter.SelectCommand = cmd;
			adapter.Fill(table);

			// Устанавливаем задания во 2-й уровень дерева, но для конкретной вершины
			foreach (var item in table.Rows)
			{
				var row = (DataRow)item;
				// Берём `id` студента
				uint taskId = row.Field<uint>("node");
				// Для понятного отображения идём в таблицу `nodes` и берём оттуда `name`
				cmd = new MySqlCommand(
					"SELECT `name` FROM `nodes` " +
					"WHERE `id` = @taskId",
					db.GetConnection());
				cmd.Parameters.Add("@taskId", MySqlDbType.UInt32).Value = taskId;
				table = new DataTable();
				adapter.SelectCommand = cmd;
				adapter.Fill(table);
				// Получаем только одно совпадение по первичному ключу
				string taskName = table.Rows[0].Field<string>("name");

				// Устанавливаем `name` в качестве `Text` вершины
				var node = new TreeNode(taskName);
				// Кроме того, устанавливаем в качестве `Tag` `id` задания для удобства выполнения последующих операций
				node.Tag = taskId;
				// Добавляем подвершину студента к вершине группы
				studentNode.Nodes.Add(node);
			}
		}
		#endregion

		#region Удаление вершин
		private void GroupRemoving(TreeNode groupNode)
		{
			// note Логика работы программы подразумевает, что удаление группы не приводит к удалению связанных с ней учеников. Здесь группа удалится и не будет отображаться.

			string groupName = (string)groupNode.Tag;
			var cmd = new MySqlCommand(
				"DELETE FROM `groups` " +
				"WHERE `name` = @groupName",
				db.GetConnection());
			cmd.Parameters.Add("@groupName", MySqlDbType.VarChar).Value = groupName;

			db.OpenConnection();
			int removedNum = cmd.ExecuteNonQuery();
			db.CloseConnection();
		}

		private void StudentRemoving(TreeNode studentNode)
		{
			uint studentId = (uint)studentNode.Tag;
			var cmd = new MySqlCommand(
				"DELETE FROM `student_group_connections` " +
				"WHERE `student` = @studentId",
				db.GetConnection());
			cmd.Parameters.Add("@studentId", MySqlDbType.UInt32).Value = studentId;

			db.OpenConnection();
			int removedNum = cmd.ExecuteNonQuery();
			db.CloseConnection();
		}

		private void TaskRemoving(TreeNode taskNode)
		{
			uint taskId = (uint)taskNode.Tag;
			var cmd = new MySqlCommand(
				"DELETE FROM `student_node_connections` " +
				"WHERE `node` = @taskId",
				db.GetConnection());
			cmd.Parameters.Add("@taskId", MySqlDbType.UInt32).Value = taskId;

			db.OpenConnection();
			int removedNum = cmd.ExecuteNonQuery();
			db.CloseConnection();
		}
		#endregion

		#region Добавление вершин дерева
		private void GroupAdding()
		{
			using (GroupInputForm inputDialog = new GroupInputForm())
			{
				if (inputDialog.ShowDialog() == DialogResult.OK)
				{
					string groupName = inputDialog.GroupName;

					var cmd = new MySqlCommand(
						$"INSERT INTO `groups` (`name`) " +
						$"VALUES (@groupName)",
						db.GetConnection());
					cmd.Parameters.Add("@groupName", MySqlDbType.VarChar).Value = groupName;

					db.OpenConnection();
					// Возможно добавление дупликата, поэтому обратаываем это событие
					try
					{
						int addedNum = cmd.ExecuteNonQuery();
					}
					catch (MySqlException ex)
					{
						MessageBox.Show(ex.Message);
						db.CloseConnection();
						return;
					}
					db.CloseConnection();

					ChangeNotify(this, EventArgs.Empty);
				}
			}
		}

		private void StudentAdding(TreeNode groupNode)
		{
			using (StudentInputForm inputDialog = new StudentInputForm(groupNode))
			{
				if (inputDialog.ShowDialog() == DialogResult.OK)
				{
					// Получаем выбранного учащегося
					User student = inputDialog.ChosenStudent;

					var cmd = new MySqlCommand(
						$"INSERT INTO `student_group_connections` (`student`, `group`) " +
						$"VALUES (@studentId, @groupName)",
						db.GetConnection());
					cmd.Parameters.Add("@studentId", MySqlDbType.UInt32).Value = student.Id;
					cmd.Parameters.Add("@groupName", MySqlDbType.VarChar).Value = (string)groupNode.Tag;

					db.OpenConnection();
					int addedNum = cmd.ExecuteNonQuery();
					db.CloseConnection();

					ChangeNotify(this, EventArgs.Empty);
				}
			}
		}

		private void TaskAdding(TreeNode studentNode)
		{
			using (TaskInputForm inputDialog = new TaskInputForm(studentNode))
			{
				if (inputDialog.ShowDialog() == DialogResult.OK)
				{
					// Получаем выбранное задание
					var taskWithParams = inputDialog.ChosenTaskWithParams;
					TaskTreeNode node = taskWithParams.Item1;
					int? mark = taskWithParams.Item2;
					string comment = taskWithParams.Item3;

					var cmd = new MySqlCommand(
						$"INSERT INTO `student_node_connections` (`node`, `student`, `mark`, `comment`) " +
						$"VALUES (@nodeId, @studentId, @mark, @comment)",
						db.GetConnection());
					cmd.Parameters.Add("@nodeId", MySqlDbType.UInt32).Value = node.NodeId;
					cmd.Parameters.Add("@studentId", MySqlDbType.UInt32).Value = (uint)studentNode.Tag;
					cmd.Parameters.Add("@mark", MySqlDbType.Int32).Value = mark;
					cmd.Parameters.Add("@comment", MySqlDbType.VarChar).Value = comment;

					db.OpenConnection();
					int addedNum = cmd.ExecuteNonQuery();
					db.CloseConnection();

					ChangeNotify(this, EventArgs.Empty);
				}
			}

			ChangeNotify(this, EventArgs.Empty);
		}

		#endregion

		private void removeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (treeView1.SelectedNode == null)
				return;

			// Производим удаление исходя из уровня вершины
			switch (treeView1.SelectedNode.Level)
			{
				// Удаление группы
				case 0:
					GroupRemoving(treeView1.SelectedNode);
					break;
				// Удаление студента
				case 1:
					StudentRemoving(treeView1.SelectedNode);
					break;
				// Удаление задания
				case 2:
					TaskRemoving(treeView1.SelectedNode);
					break;

				default:
					throw new Exception();
			}

			ChangeNotify(this, EventArgs.Empty);
		}

		private void TreeChanged(object sender, EventArgs e)
		{
			TreeViewUpdate();
		}

		private void addToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (treeView1.SelectedNode == null)
			{
				// Добавление группы
				GroupAdding();
				return;
			}

			// Производим добавление исходя из уровня вершины
			switch (treeView1.SelectedNode.Level)
			{
				// Добавление студента
				case 0:
					StudentAdding(treeView1.SelectedNode);
					break;
				// Добавление задания
				case 1:
					TaskAdding(treeView1.SelectedNode);
					break;
				// Запрещаем добавление
				case 2:
					throw new Exception();

				default:
					throw new Exception();
			}
		}

		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			if (treeView1.SelectedNode == null)
			{
				addToolStripMenuItem.Enabled = true;
				редактироватьToolStripMenuItem.Enabled = false;
				removeToolStripMenuItem.Enabled = false;
			}
			else
			{
				switch (treeView1.SelectedNode.Level)
				{
					case 0:
						addToolStripMenuItem.Enabled = true;
						редактироватьToolStripMenuItem.Enabled = true;
						removeToolStripMenuItem.Enabled = true;
						break;
					case 1:
						addToolStripMenuItem.Enabled = true;
						редактироватьToolStripMenuItem.Enabled = false;
						removeToolStripMenuItem.Enabled = true;
						break;
					case 2:
						addToolStripMenuItem.Enabled = false;
						редактироватьToolStripMenuItem.Enabled = false;
						removeToolStripMenuItem.Enabled = true;
						break;

					default:
						throw new Exception();
				}

				//if (treeView1.SelectedNode.Level == 2)
				//{
				//	addToolStripMenuItem.Enabled = false;
				//}
				//else
				//{
				//	addToolStripMenuItem.Enabled = true;
				//}
				//removeToolStripMenuItem.Enabled = true;
			}
		}

		private void treeView1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				var node = treeView1.GetNodeAt(e.X, e.Y);
				if (node == null)
				{
					// При нажатии на пустую область
					treeView1.SelectedNode = null;
				}
			}
		}

		private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (treeView1.SelectedNode.Level == null)
			{
				return;
			}

			switch (treeView1.SelectedNode.Level)
			{
				// Редактирование названия группы
				case 0:
					GroupEdit(treeView1.SelectedNode);
					break;

				default:
					throw new Exception();
			}
		}

		#region Редактирвоание вершин
		private void GroupEdit(TreeNode groupNode)
		{
			using (GroupInputForm inputDialog = new GroupInputForm())
			{
				if (inputDialog.ShowDialog() == DialogResult.OK)
				{
					string newGroupName = inputDialog.GroupName;
					string oldGroupName = (string)groupNode.Tag;

					var cmd = new MySqlCommand(
						"UPDATE `groups` SET " +
						"`name` = @newGroupName " +
						"WHERE `name` = @oldGroupName",
					db.GetConnection());
					cmd.Parameters.Add("@newGroupName", MySqlDbType.VarChar).Value = newGroupName;
					cmd.Parameters.Add("@oldGroupName", MySqlDbType.VarChar).Value = oldGroupName;

					db.OpenConnection();
					// Возможно добавление дупликата, поэтому обрабатываем это событие
					try
					{
						int editedNum = cmd.ExecuteNonQuery();
					}
					catch (MySqlException ex)
					{
						MessageBox.Show(ex.Message);
						db.CloseConnection();
						return;
					}
					db.CloseConnection();

					ChangeNotify(this, EventArgs.Empty);
				}
			}
		}
		#endregion
	}
}
