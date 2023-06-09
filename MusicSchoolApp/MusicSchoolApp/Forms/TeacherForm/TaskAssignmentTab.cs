using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSchoolApp.Forms
{
	partial class TeacherForm
	{
		public void TaskAssignmentTabSets()
		{
			// Очистка полей при переходе на эту вкладку
			nameField3.Text = "";
			descriptionField3.Text = "";


			SetNodeTree3(teacher.Id);

			// ! Можно вприцнипе удалить, но наверняка выбираем вершину
			treeView3.SelectedNode = treeView3.SelectedNode;

			checkedListBoxGroups3.DisplayMember = "Name";

			SetCheckedListBoxGroups3();
			SetCheckedListBoxStudents3();

			// вызываем функцию для обработки нажатия кнопки "Отмена" для того, чтобы установить readonly = true для полей
			cancelBtn3_Click(this, EventArgs.Empty);
		}

		#region Установки полей
		private void SetCheckedListBoxStudents3()
		{
			checkedListBoxStudents3.Items.Clear();

			// Добавляем всех учеников в CheckedListBox
			var cmd = new MySqlCommand(
				"SELECT * FROM `users` " +
				"WHERE `role` = 'student'",
				db.GetConnection());
			adapter.SelectCommand = cmd;
			var table = new DataTable();
			adapter.Fill(table);

			foreach (var item in table.Rows)
			{
				var row = (DataRow)item;
				uint studentId = row.Field<uint>("id");
				var student = new User(studentId);
				checkedListBoxStudents3.Items.Add(student);
			}
			// Сортируем по ФИО
			checkedListBoxStudents3.Sorted = true;
		}

		private void SetCheckedListBoxGroups3()
		{
			checkedListBoxGroups3.Items.Clear();

			// Добавляем групп учеников, которые содержат >= 1 ученика, в CheckedListBox

			// Находим все группы
			var cmd = new MySqlCommand(
				"SELECT * FROM `groups`",
				db.GetConnection());
			adapter.SelectCommand = cmd;
			var table = new DataTable();
			adapter.Fill(table);

			foreach (var item in table.Rows)
			{
				var row = (DataRow)item;
				string groupName = row.Field<string>("name");

				// Проверяем, есть ли в этой группе хотя бы 1 ученик
				var cmd2 = new MySqlCommand(
					"SELECT * FROM `student_group_connections` " +
					"WHERE `group` = @groupName", 
					db.GetConnection());
				cmd2.Parameters.Add("@groupName", MySqlDbType.VarChar).Value = groupName;
				adapter.SelectCommand = cmd2;
				var table2 = new DataTable();
				adapter.Fill(table2);
				// Если группа пустая, то не добавляем её
				if (table2.Rows.Count == 0)
					continue;

				Group group = new Group(groupName);
				checkedListBoxGroups3.Items.Add(group);
			}
			// Сортируем по названиям групп
			checkedListBoxGroups3.Sorted = true;
		}

		private void SetNodeTree3(uint teacherId)
		{
			treeView3.Nodes.Clear();

			//var list = new List<TaskTreeNode>();

			var cmd = new MySqlCommand(
				$"SELECT `id` FROM `nodes` " +
				$"WHERE `owner` = @teacherId " +
				$"AND `parent` IS NULL",
				db.GetConnection());
			cmd.Parameters.Add("@teacherId", MySqlDbType.Int32).Value = teacherId;
			adapter.SelectCommand = cmd;
			var table = new DataTable();
			adapter.Fill(table);

			foreach (var item in table.Rows)
			{
				var row = (DataRow)item;
				uint nodeId = row.Field<uint>("id");

				var node = new TaskTreeNode((int)nodeId);
				//list.Add(node);

				treeView3.Nodes.Add(node);
			}
		}
		#endregion

		private bool? GetStudentStateById(uint id)
		{
			//foreach (var item in checkedListBoxStudents3.Items)
			for (int i = 0; i < checkedListBoxStudents3.Items.Count; i++)
			{
				//var student = (User)item;
				var student = (User)checkedListBoxStudents3.Items[i];

				if (student.Id == id)
				{
					var studentMode = checkedListBoxStudents3.GetItemCheckState(i);
					switch (studentMode)
					{
						case CheckState.Unchecked:
							return false;
						case CheckState.Checked:
							return true;
						case CheckState.Indeterminate:
							return null;
					}
				}
			}
			return null;
		}

		#region Обработчики событий
		private void treeView3_AfterSelect(object? sender, TreeViewEventArgs e)
		{
			// Разблокируем поля студентов и групп
			checkedListBoxStudents3.Enabled = true;
			checkedListBoxGroups3.Enabled = true;

			var selectedNode = (TaskTreeNode)treeView3.SelectedNode;
			nameField3.Text = selectedNode.Text;
			descriptionField3.Text = selectedNode.Description;

			// Проставляем галочки всем Student, если у них есть записи с выбранной корневой вершиной
			//int i = 0;
			//foreach (var item in checkedListBox1.Items)
			for (int i = 0; i < checkedListBoxStudents3.Items.Count; i++)
			{
				//var student = (Student)item;
				var student = (User)checkedListBoxStudents3.Items[i];
				var cmd = new MySqlCommand(
					"SELECT * FROM `student_node_connections` " +
					"WHERE `node` = @nodeId " +
					"AND `student` = @studentId",
					db.GetConnection());
				cmd.Parameters.Add("@nodeId", MySqlDbType.Int32).Value = selectedNode.NodeId;
				cmd.Parameters.Add("@studentId", MySqlDbType.Int32).Value = student.Id;
				adapter.SelectCommand = cmd;
				var table = new DataTable();
				adapter.Fill(table);

				if (table.Rows.Count > 0)
					//student.Checked = true;
					checkedListBoxStudents3.SetItemChecked(i, true);
				else
					//student.Checked = false;
					checkedListBoxStudents3.SetItemChecked(i, false);

				// Разрешаем редактирование
				checkedListBoxStudents3.Enabled = true;
				checkedListBoxGroups3.Enabled = true;

				//i++;
			}
			//checkedListBox1.Refresh();
		}

		private void treeView3_MouseDown(object? sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				var node = treeView3.GetNodeAt(e.X, e.Y);
				if (node == null)
				{
					treeView3.SelectedNode = null;
					// Делаем все текстовые поля пустыми
					nameField3.Text = "";
					descriptionField3.Text = "";
					// Убираем галочки со всех учеников
					for (int i = 0; i < checkedListBoxStudents3.Items.Count; i++)
					{
						checkedListBoxStudents3.SetItemChecked(i, false);
					}
					// Блокируем поля студентов и групп
					checkedListBoxStudents3.Enabled = false;
					checkedListBoxGroups3.Enabled = false;
				}
			}
		}

		private void saveBtn3_Click(object? sender, EventArgs e)
		{
			if (treeView3.SelectedNode == null)
				return;

			var selectedNode = (TaskTreeNode)treeView3.SelectedNode;

			// ADDME: Добавить MBox или что-то другое для спрашивания пользователя о том, "уверен ли он в своём действии, т.к. будут добавлены, отредактированы или удалены все связанные записи"

			// Проходимся по всем ученикам checkedListBox и смотрим, изменилось ли их состояние относительно БД
			//int i = 0;
			//foreach (var item in checkedListBox1.Items)
			for (int i = 0; i < checkedListBoxStudents3.Items.Count; i++)
			{
				//var student = (Student)item;
				var student = (User)checkedListBoxStudents3.Items[i];

				var cmd = new MySqlCommand(
					"SELECT * FROM `student_node_connections` " +
					"WHERE `node` = @nodeId " +
					"AND `student` = @studentId",
					db.GetConnection());
				cmd.Parameters.Add("@nodeId", MySqlDbType.UInt32).Value = selectedNode.NodeId;
				cmd.Parameters.Add("@studentId", MySqlDbType.UInt32).Value = student.Id;
				adapter.SelectCommand = cmd;
				var table = new DataTable();
				adapter.Fill(table);

				// Есть 4 возможных случая

				// (Был ли до этого в БД (student_node_connections), Выбран ли теперь)
				// (0,0) -> ничего не делаем
				// (0,1) -> добавляем в БД (student_node_connections), в т.ч. всех потомков
				// (1,0) -> удаляем из БД (student_node_connections), в т.ч. всех потомков 
				// (1,1) -> ничего не делаем

				// Ничего не делаем
				if (
					//	(table.Rows.Count == 0 && !student.Checked)
					//|| (table.Rows.Count == 1 && student.Checked)
					(table.Rows.Count == 0 && !checkedListBoxStudents3.GetItemChecked(i))
					|| (table.Rows.Count == 1 && checkedListBoxStudents3.GetItemChecked(i))
				)
				{

				}
				// Добавляем в БД (student_node_connections), в т.ч. всех потомков
				else if (table.Rows.Count == 0 && checkedListBoxStudents3.GetItemChecked(i))
				{
					AddAllStudentNodesWithChildren(student.Id, (uint)selectedNode.NodeId);
				}
				// Удаляем из БД (student_node_connections), в т.ч. всех потомков 
				else if (table.Rows.Count > 0 && !checkedListBoxStudents3.GetItemChecked(i))
				{
					RemoveAllStudentNodesWithChildren(student.Id, (uint)selectedNode.NodeId);
				}
				// ! Такого случая не должно быть, но на всякий случай ставлю вызов ошибки
				else
				{
					throw new Exception("Произошёл невозможный случай");
				}

				//i++;
			}

			// Инициируем вызов AfterSelect "псевдоизменением" текущей выбранной вершины для того, чтобы обновились галочки у Student
			treeView3.SelectedNode = treeView3.SelectedNode;
			//checkedListBox1.Refresh();

			saveBtn3.Enabled = false;
			cancelBtn3.Enabled = false;
		}

		private void cancelBtn3_Click(object? sender, EventArgs e)
		{
			SetCheckedListBoxStudents3();
			
			saveBtn3.Enabled = false;
			cancelBtn3.Enabled = false;
		}

		/// <summary>
		/// Обновляет состояние всех связанных со студентом групп
		/// </summary>
		private void checkedListBoxStudents3_ItemCheck(object? sender, ItemCheckEventArgs e)
		{
			// Получаем выбранного студента
			var selectedStudent = (User)checkedListBoxStudents3.Items[e.Index];
			// Узнаём текущее состояние студента
			//var studentMode = checkedListBoxStudents3.GetItemCheckState(e.Index);
			var studentMode = e.NewValue;

			//var groups = new List<Group>();

			// Отписываемся от события `checkedListBoxGroups3_ItemCheck` во избежании рекурсивных вызовов друг друга и последующим переполнением стека
			checkedListBoxGroups3.ItemCheck -= checkedListBoxGroups3_ItemCheck;

			// Находим все группы, которые связаны со студентом
			for (int i = 0; i < checkedListBoxGroups3.Items.Count; i++)
			{
				//var group = (Group)item;
				var group = (Group)checkedListBoxGroups3.Items[i];

				// Если группа включает студента, то проверяем, нужно ли изменять состояние группы и изменяем, если да
				if (group.Any(student => student == selectedStudent.Id))
				{
					// Узнаём состояние группы
					//var mode = checkedListBoxGroups3.GetItemCheckState(groupIndex);
					var mode = checkedListBoxGroups3.GetItemCheckState(i);

					// Заполняем список булевых значений
					var boolList = new List<bool?>();
					foreach (var item in group)
						boolList.Add(GetStudentStateById(item));

					// Данная функция вызывается ПЕРЕД сменой значения.
					// Поэтому меняем один `bool` в `list` на противоположный
					if (boolList.Count > 0)
					{
						if (studentMode == CheckState.Checked)
						{
							// Находим индекс первого попавшегося `false` и меняем на `true`
							boolList[boolList.IndexOf(false)] = true;
						}
						else if (studentMode == CheckState.Unchecked)
						{
							// Находим индекс первого попавшегося `true` и меняем на `false`
							boolList[boolList.IndexOf(true)] = false;
						}
						else
						{
							throw new Exception("Произошло невозможное событие");
						}
					}

					// Проверяем устанавливать ли группу в положение Checked, Indeterminate или Unchecked
					bool allTrue = boolList.All(b => b == true);
					bool allFalse = boolList.All(b => b == false);
					bool containsTrueAndFalse = boolList.Any(b => b == true) && boolList.Any(b => b == false);

					if (allTrue)
						checkedListBoxGroups3.SetItemCheckState(i, CheckState.Checked);
					else if (containsTrueAndFalse)
						checkedListBoxGroups3.SetItemCheckState(i, CheckState.Indeterminate);
					else if (allFalse)
						checkedListBoxGroups3.SetItemCheckState(i, CheckState.Unchecked);
					// Такой случай невозможен
					else
						throw new Exception();
				}
			}

			// Подисываемся обратно
			checkedListBoxGroups3.ItemCheck += checkedListBoxGroups3_ItemCheck;

			saveBtn3.Enabled = true;
			cancelBtn3.Enabled = true;
		}

		/// <summary>
		/// Обновляет состояние всех связанных с выбранной групп студентов
		/// </summary>
		private void checkedListBoxGroups3_ItemCheck(object? sender, ItemCheckEventArgs e)
		{
			// Получаем выбранную группу
			var selectedGroup = (Group)checkedListBoxGroups3.Items[e.Index];

			//// Отписываемся от события `checkedListBoxStudents3_ItemCheck` во избежании рекурсивных вызовов друг друга
			//checkedListBoxStudents3.ItemCheck -= checkedListBoxStudents3_ItemCheck;

			// Изменяем состояние всех связанных элементов в другом списке
			for (int i = 0; i < checkedListBoxStudents3.Items.Count; i++)
			{
				if (checkedListBoxStudents3.Items[i] is User studentId && selectedGroup.Any(studentId_ => studentId_ == studentId.Id))
				{
					checkedListBoxStudents3.SetItemCheckState(i, e.NewValue);
				}
			}

			//// Обратно подписываемся
			//checkedListBoxStudents3.ItemCheck += checkedListBoxStudents3_ItemCheck;

			saveBtn3.Enabled = true;
			cancelBtn3.Enabled = true;
		}
		#endregion

		#region Рекурсивные функции, выполняющие операции с таблицей `student_node_connections`
		private void RemoveAllStudentNodesWithChildren(uint studentId, uint nodeId)
		{
			var cmd = new MySqlCommand(
				"DELETE FROM `student_node_connections` " +
				"WHERE `node` = @nodeId " +
				"AND `student` = @studentId",
				db.GetConnection());
			cmd.Parameters.Add("@studentId", MySqlDbType.Int32).Value = studentId;
			cmd.Parameters.Add("@nodeId", MySqlDbType.Int32).Value = nodeId;

			db.OpenConnection();
			int removedNum = cmd.ExecuteNonQuery();
			db.CloseConnection();

			// Находим всех потомков
			cmd = new MySqlCommand(
				"SELECT `id` FROM `nodes` " +
				"WHERE `parent` = @parentId",
				db.GetConnection());
			cmd.Parameters.Add("@parentId", MySqlDbType.Int32).Value = nodeId;
			adapter.SelectCommand = cmd;
			var table = new DataTable();
			adapter.Fill(table);

			foreach (var item in table.Rows)
			{
				var row = (DataRow)item;

				uint nextNodeId = row.Field<uint>("id");
				// И рекурсивно повторяем действия
				RemoveAllStudentNodesWithChildren(studentId, nextNodeId);
			}
		}
		private void AddAllStudentNodesWithChildren(uint studentId, uint nodeId)
		{
			var cmd = new MySqlCommand(
				$"INSERT INTO `student_node_connections` (`node`, `student`, `mark`, `comment`) " +
				$"VALUES (@nodeId, @studentId, NULL, NULL)",
				db.GetConnection());
			cmd.Parameters.Add("@nodeId", MySqlDbType.Int32).Value = nodeId;
			cmd.Parameters.Add("@studentId", MySqlDbType.Int32).Value = studentId;

			db.OpenConnection();
			int addedNum = cmd.ExecuteNonQuery();
			db.CloseConnection();

			// Находим всех потомков
			cmd = new MySqlCommand(
				"SELECT `id` FROM `nodes` " +
				"WHERE `parent` = @parentId",
				db.GetConnection());
			cmd.Parameters.Add("@parentId", MySqlDbType.Int32).Value = nodeId;
			adapter.SelectCommand = cmd;
			var table = new DataTable();
			adapter.Fill(table);

			foreach (var item in table.Rows)
			{
				var row = (DataRow)item;

				uint nextNodeId = row.Field<uint>("id");
				// И рекурсивно повторяем действия
				AddAllStudentNodesWithChildren(studentId, nextNodeId);
			}
		}
		#endregion
	}
}