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
		private List<TaskTreeNode> removingNodes = new List<TaskTreeNode>();

		private void TaskEditTabSets()
		{
			// Временна отписка для не изменения значений в БД
			nameField.TextChanged -= nameField_TextChanged;
			descriptionField.TextChanged -= descriptionField_TextChanged;
			// Очистка полей при переходе на эту вкладку
			nameField.Text = "";
			descriptionField.Text = "";
			// Обратная подписка
			nameField.TextChanged += nameField_TextChanged;
			descriptionField.TextChanged += descriptionField_TextChanged;


			SetTrees(teacher.Id);

			ChangeNotify += MakeSaveEnabled;

			// hack: при создании всё равно остаётся Enabled. Связано с MakeSaveEnabled
			saveBtn.Enabled = false;
			cancelBtn.Enabled = false;

			// Делаем поля недоступными для записи, тк ни одна вершина не выбрана
			nameField.ReadOnly = true;
			descriptionField.ReadOnly = true;
		}


		#region Работа с отображением дерева
		private void SetTrees(uint teacherId)
		{
			treeView1.Nodes.Clear();

			var list = new List<TaskTreeNode>();

			var cmd = new MySqlCommand($"SELECT `id` FROM `nodes` WHERE `owner` = @teacherId", db.GetConnection());
			cmd.Parameters.Add("@teacherId", MySqlDbType.UInt32).Value = teacherId;
			adapter.SelectCommand = cmd;
			var table = new DataTable();
			adapter.Fill(table);

			foreach (var item in table.Rows)
			{
				var row = (DataRow)item;
				uint nodeId = row.Field<uint>("id");

				var node = new TaskTreeNode((int)nodeId);
				list.Add(node);
			}

			foreach (var inode in list)
			{
				var parent = list.Find(node => node.NodeId == inode.ParentId);
				parent?.Nodes.Insert((int)inode.Priority, inode);
			}

			foreach (var item in list.FindAll(node => node.ParentId == null))
				treeView1.Nodes.Add(item);
		}
		#endregion

		#region Обработчики событий
		private void addBtn_Click(object sender, EventArgs e)
		{
			// Создаём несуществующую в БД вершину с заглушкой id = -1. 
			// В БД id стоит как UNSIGNED, поэтому, случайно эту вершину мы не добавим
			var node = new TaskTreeNode(-1);
			node.NodeName = "*Название*";

			// Вставляем в потомки выбранной вершины
			if (treeView1.SelectedNode != null)
			{
				var selectedNode = (TaskTreeNode)treeView1.SelectedNode;
				node.ParentId = selectedNode.NodeId;
				// Вставляем в конец
				treeView1.SelectedNode.Nodes.Add(node);
			}
			// Вставляем в качестве корнеквой вершины
			else
			{
				// Вставляем в конец
				treeView1.Nodes.Add(node);
			}

			ChangeNotify?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Делает ранее выбранную вершину неактивной. Срабатывает при клике на пустую область
		/// </summary>
		private void treeView1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				var node = treeView1.GetNodeAt(e.X, e.Y);
				if (node == null)
				{
					treeView1.SelectedNode = null;
					// Делаем все текстовые поля пустыми
					nameField.Text = "";
					descriptionField.Text = "";
					// И делаем их доступными только для чтения
					nameField.ReadOnly = true;
					descriptionField.ReadOnly = true;
				}
			}
		}

		// TODO: заглушки для работы с перетаскиванием вершин
		private void treeView1_ItemDrag(object sender, ItemDragEventArgs e) { }
		private void treeView1_DragOver(object sender, DragEventArgs e) { }
		private void treeView1_DragDrop(object sender, DragEventArgs e) { }

		private void nameField_TextChanged(object? sender, EventArgs e)
		{
			if (treeView1.SelectedNode != null)
			{
				var selectedNode = (TaskTreeNode)treeView1.SelectedNode;
				//var selectedNode = (UnaddedTaskTreeNode)treeView1.SelectedNode;

				selectedNode.NodeName = nameField.Text;

				ChangeNotify?.Invoke(this, EventArgs.Empty);
			}
		}

		private void descriptionField_TextChanged(object? sender, EventArgs e)
		{
			if (treeView1.SelectedNode != null)
			{
				var selectedNode = (TaskTreeNode)treeView1.SelectedNode;
				//var selectedNode = (UnaddedTaskTreeNode)treeView1.SelectedNode;

				selectedNode.Description = descriptionField.Text;

				ChangeNotify?.Invoke(this, EventArgs.Empty);
			}
		}

		private void cancelBtn_Click(object sender, EventArgs e)
		{
			SetTrees(teacher.Id);
			// ! Возможно не стоит очищать текстовые поля
			nameField.Text = "";
			descriptionField.Text = "";

			removingNodes.Clear();

			saveBtn.Enabled = false;
			cancelBtn.Enabled = false;
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			var selectedNode = (TaskTreeNode)treeView1.SelectedNode;
			//nameField.Text = selectedNode.Text;

			// Делаем поля доступными для записи
			nameField.ReadOnly = false;
			descriptionField.ReadOnly = false;

			// Временно отписываемся от событий
			nameField.TextChanged -= nameField_TextChanged;
			descriptionField.TextChanged -= descriptionField_TextChanged;

			nameField.Text = selectedNode.NodeName;
			descriptionField.Text = selectedNode.Description;

			// Подписываемся на собыьтя обратно
			nameField.TextChanged += nameField_TextChanged;
			descriptionField.TextChanged += descriptionField_TextChanged;
		}

		private void MakeSaveEnabled(object sender, EventArgs e)
		{
			saveBtn.Enabled = true;
			cancelBtn.Enabled = true;
		}

		private void saveBtn_Click(object sender, EventArgs e)
		{
			// Уведомляем пользователя, что он собирается удалить вершины и за этим последуют последствия
			if (removingNodes.Count > 0)
			{
				var confirmRes = MessageBox.Show("Вы уверены, что хотите удалить записи? Связанные данные будут потеряны!\n\nНажмите \"Да\", чтобы продолжить и \"Нет\", чтобы отменить действие",
												"Подтверждение удаленияе",
												MessageBoxButtons.YesNo);
				if (confirmRes == DialogResult.No)
				{
					cancelBtn_Click(this, EventArgs.Empty);
					return;
				}
			}

			// Удаление всех удалённых вершин из БД
			foreach (var removingNode in removingNodes)
			{
				// Если значение id < 0, значит вершина и так не была добавлена в БД и проводить доп.действий не надо
				if (removingNode.NodeId < 0)
					continue;

				var cmd = new MySqlCommand($"DELETE FROM `nodes` WHERE `id` = @nodeId", db.GetConnection());
				cmd.Parameters.Add("@nodeId", MySqlDbType.Int32).Value = removingNode.NodeId;
				db.OpenConnection();
				int removedNum = cmd.ExecuteNonQuery();
				db.CloseConnection();
			}
			removingNodes.Clear();

			// Вызываем рекурсивную функцию, чтобы добавить или обновить вершины и всех их потомков
			RecToAddOrUpdateNodesToDB(null, treeView1.Nodes);

			SetTrees(teacher.Id);

			// Делаем кнопки сохранить и отменить недоступными
			saveBtn.Enabled = false;
			cancelBtn.Enabled = false;
		}

		private void removeBtn_Click(object sender, EventArgs e)
		{
			if (treeView1.SelectedNode != null)
			{
				removingNodes.Add((TaskTreeNode)treeView1.SelectedNode);
				treeView1.SelectedNode.Remove();

				ChangeNotify?.Invoke(this, EventArgs.Empty);
			}
		}
		#endregion


		#region Рекурсивные функции для работы с treeView

		private void RecToAddOrUpdateNodesToDB(uint? parentId, TreeNodeCollection nodesToAdd)
		{
			if (nodesToAdd.Count == 0)
				return;

			// Используем индексатор для того, чтобы установить в БД корретное значение priority
			uint i = 0;
			// Добавление и/или обновление изменённых вершин в БД
			foreach (var rootNodeObj in nodesToAdd)
			{
				var node = (TaskTreeNode)rootNodeObj;
				node.ParentId =(int?)parentId;
				// Если id < 0, то вершина ещё не записана в БД => записываем её
				if (node.NodeId < 0)
				{
					//INSERT INTO `nodes` (`id`, `name`, `owner`, `description`, `parent`, `priority`) VALUES(NULL, 'Удалите меня', '2', NULL, NULL, '0');
					// ! Под id ставим NULL и вроде должно автоматически установитсься самой БД, но может и нет
					var cmd = new MySqlCommand(
						$"INSERT INTO `nodes` (`id`, `name`, `owner`, `description`, `parent`, `priority`) " +
						$"VALUES (NULL, @name, @ownerId, @description, @parentId, @priority);" +
						$"SELECT LAST_INSERT_ID();", // Вызываем функцию для того, чтобы получить автоматически присвоенный SQL индекс
						db.GetConnection());
					cmd.Parameters.Add("@name", MySqlDbType.String).Value = node.Text;
					//cmd.Parameters.Add("@ownerId", MySqlDbType.Int32).Value = node.Owner;
					cmd.Parameters.Add("@ownerId", MySqlDbType.UInt32).Value = teacher.Id;
					cmd.Parameters.Add("@description", MySqlDbType.Int32).Value = node.Description;
					cmd.Parameters.Add("@parentId", MySqlDbType.Int32).Value = node.ParentId;
					cmd.Parameters.Add("@priority", MySqlDbType.UInt32).Value = i;


					db.OpenConnection();
					//int removedNum = cmd.ExecuteNonQuery();
					node.NodeId = Convert.ToInt32(cmd.ExecuteScalar());
					db.CloseConnection();
				}
				// Обновляем вершины, которые уже записаны в БД
				else
				{
					var cmd = new MySqlCommand(
					$"UPDATE `nodes` SET " +
					$"`name` = @name, " +
					$"`description` = @description, " +
					$"`parent` = @parentId, " +
					$"`priority` = @priority " +
					$"WHERE `nodes`.`id` = @nodeId;",
					db.GetConnection());

					cmd.Parameters.Add("@name", MySqlDbType.String).Value = node.Text;
					cmd.Parameters.Add("@description", MySqlDbType.VarChar).Value = node.Description;
					cmd.Parameters.Add("@parentId", MySqlDbType.Int32).Value = node.ParentId;
					cmd.Parameters.Add("@priority", MySqlDbType.UInt32).Value = i;
					cmd.Parameters.Add("@nodeId", MySqlDbType.Int32).Value = node.NodeId;

					db.OpenConnection();
					int removedNum = cmd.ExecuteNonQuery();
					db.CloseConnection();
				}
				// Добавляем или редактируем всех потомков в БД
				// ! 
				RecToAddOrUpdateNodesToDB((uint)node.NodeId, node.Nodes);
				i++;
			}
		}
		#endregion
	}
}
