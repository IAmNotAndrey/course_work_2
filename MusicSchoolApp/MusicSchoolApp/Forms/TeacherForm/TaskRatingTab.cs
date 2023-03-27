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
		private void TaskRatingTabSets()
		{
			// Временна отписка для не изменения значений в БД
			markField2.TextChanged -= markField2_TextChanged;
			commentField2.TextChanged -= commentField2_TextChanged;
			// Очистка полей при переходе на эту вкладку
			treeView2.Nodes.Clear();

			nameField2.Text = "";
			descriptionField2.Text = "";
			markField2.Text = "";
			commentField2.Text = "";
			// Обратная подписка
			markField2.TextChanged += markField2_TextChanged;
			commentField2.TextChanged += commentField2_TextChanged;


			FillListBox();

			markField2.ReadOnly = true;
			commentField2.ReadOnly = true;
		}


		#region Заполнение полей
		private void FillListBox()
		{
			listBox2.Items.Clear();

			var cmd = new MySqlCommand(
				"SELECT * FROM `users` " +
				"WHERE `role` = 'student'",
				db.GetConnection());
			adapter.SelectCommand = cmd;
			var table = new DataTable();
			adapter.Fill(table);

			foreach (var rowObj in table.Rows)
			{
				var row = (DataRow)rowObj;
				uint studentId = row.Field<uint>("id");
				var student = new User(studentId);
				listBox2.Items.Add(student);
			}
			// Сортируем элементы по возрастанию по ФИО
			listBox2.Sorted = true;
		}
		#endregion

		#region Обработчики событий
		private void treeView2_MouseDown(object? sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				var node = treeView2.GetNodeAt(e.X, e.Y);
				if (node == null)
				{
					treeView2.SelectedNode = null;

					// Делаем поля оценки и комментария неизменяемыми
					markField2.ReadOnly = true;
					commentField2.ReadOnly = true;

					// Отписываемся
					nameField.TextChanged -= nameField_TextChanged;
					descriptionField.TextChanged -= descriptionField_TextChanged;
					// Делаем все текстовые поля пустыми
					nameField.Text = "";
					descriptionField.Text = "";
					// Подписываемся
					nameField.TextChanged += nameField_TextChanged;
					descriptionField.TextChanged += descriptionField_TextChanged;
				}
			}
		}

		private void listBox2_SelectedIndexChanged(object? sender, EventArgs e)
		{
			treeView2.Nodes.Clear();

			if (listBox2.SelectedItem == null)
				return;

			// Построение дерева по выбранному ученику
			var student = (User)listBox2.SelectedItem;

			var list = StudentForm.GetStudentNodes((int)student.Id);
			// Мы возвращаем список всех заданий ученика всех учителей. Чтобы работать только с вершинами, которые созданы текущим учителем, урезаем список.
			list.RemoveAll(node => node.Owner != teacher.Id);


			foreach (var studentNode in list)
			{
				var parent = list.Find(node => node.NodeId == studentNode.ParentId);
				parent?.Nodes.Insert((int)studentNode.Priority, studentNode);
			}

			foreach (var item in list.FindAll(node => node.ParentId == null))
				treeView2.Nodes.Add(item);
		}

		private void listBox2_MouseDown(object? sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				int index = listBox2.IndexFromPoint(e.Location);
				if (index == ListBox.NoMatches)
				{
					listBox2.SelectedItem = null;


					// ADDME: Сделать поля значений вершин пустыми
					nameField2.Text = "";
					descriptionField2.Text = "";

					// Делаем поля оценки и комментариев недоступными
					markField2.ReadOnly = true;
					commentField2.ReadOnly = true;

					// Отписываемся
					markField2.TextChanged -= markField2_TextChanged;
					commentField2.TextChanged -= commentField2_TextChanged;
					// Делаем все текстовые поля пустыми
					markField2.Text = "";
					commentField2.Text = "";
					// Подписываемся
					markField2.TextChanged += markField2_TextChanged;
					commentField2.TextChanged += commentField2_TextChanged;
				}
			}
		}

		private void treeView2_AfterSelect(object? sender, TreeViewEventArgs e)
		{
			// Делаем поля оценки и комментария изменяемыми
			markField2.ReadOnly = false;
			commentField2.ReadOnly = false;

			var selectedNode = (StudentTaskTreeNode)treeView2.SelectedNode;
			nameField2.Text = selectedNode.Text;
			descriptionField2.Text = selectedNode.Description;
			markField2.Text = selectedNode.Mark.ToString();
			commentField2.Text = selectedNode.Comment;
		}
		#endregion

		private void markField2_TextChanged(object? sender, EventArgs e)
		{
			if (treeView2.SelectedNode != null)
			{
				var selectedNode = (StudentTaskTreeNode)treeView2.SelectedNode;
				//var selectedNode = (UnaddedTaskTreeNode)treeView1.SelectedNode;

				int? mark;
				if (TryParseExtensions.TryParseNullableInt(markField2.Text, out mark))
				{
					markField2.BackColor = Color.White;
					selectedNode.Mark = mark;
				}
				else
				{
					// todo: Должна быть проверка на ввод только чисел 
					//MessageBox.Show("Введено недопустимое значение. Изменения не будут сохранены.");
					markField2.BackColor = Color.Red;
				}

				//ChangeNotify?.Invoke(this, new EventArgs());
			}
		}

		private void commentField2_TextChanged(object? sender, EventArgs e)
		{
			if (treeView2.SelectedNode != null)
			{
				var selectedNode = (StudentTaskTreeNode)treeView2.SelectedNode;
				//var selectedNode = (UnaddedTaskTreeNode)treeView1.SelectedNode;

				selectedNode.Comment = commentField2.Text;

				//ChangeNotify?.Invoke(this, new EventArgs());
			}
		}
	}
}
