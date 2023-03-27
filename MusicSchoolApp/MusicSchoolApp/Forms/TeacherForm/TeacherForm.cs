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
	public partial class TeacherForm : Form
	{
		private EntryForm entryForm;
		private User teacher;

		private delegate void ChangeHandler(object sender, EventArgs e);
		private event ChangeHandler ChangeNotify;


		private DB db = new DB();
		private MySqlDataAdapter adapter = new MySqlDataAdapter();

		public TeacherForm(uint teacherId, EntryForm entryForm)
		{
			InitializeComponent();
			this.FormClosing += (s, args) => Application.Exit();
			this.entryForm = entryForm;


			teacher = new User(teacherId);

			// Установка ФИО в поля
			tName1.Text = teacher.ToString();
			tName2.Text = teacher.ToString();
			tName3.Text = teacher.ToString();

			tabMenu_SelectedIndexChanged(this, EventArgs.Empty);

			// Установка обработчиков событий
			// Общие
			tabMenu.SelectedIndexChanged += tabMenu_SelectedIndexChanged;
			//ChangeNotify += MakeSaveEnabled;
			// tab2 
			treeView2.MouseDown += treeView2_MouseDown;
			treeView2.AfterSelect += treeView2_AfterSelect;
			listBox2.SelectedIndexChanged += listBox2_SelectedIndexChanged;
			listBox2.MouseDown += listBox2_MouseDown;
			markField2.TextChanged += markField2_TextChanged;
			commentField2.TextChanged += commentField2_TextChanged;
			// tab3
			treeView3.AfterSelect += treeView3_AfterSelect;
			treeView3.MouseDown += treeView3_MouseDown;
			saveBtn3.Click += saveBtn3_Click;
			cancelBtn3.Click += cancelBtn3_Click;
			checkedListBoxStudents3.ItemCheck += checkedListBoxStudents3_ItemCheck;
			checkedListBoxGroups3.ItemCheck += checkedListBoxGroups3_ItemCheck;
		}

		private void tabMenu_SelectedIndexChanged(object? sender, EventArgs e)
		{
			int selectedIndex = tabMenu.SelectedIndex;
			switch (selectedIndex)
			{
				case 0:
					TaskEditTabSets();
					break;

				case 1:
					TaskRatingTabSets();
					break;

				case 2:
					TaskAssignmentTabSets();
					break;
			}
		}

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
