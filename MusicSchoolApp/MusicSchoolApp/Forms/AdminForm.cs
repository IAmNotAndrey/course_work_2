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
	public partial class AdminForm : Form
	{
		private EntryForm entryForm;

		private DB db = new DB();
		private MySqlDataAdapter adapter = null;

		private DataSet ds = new DataSet();

		private MySqlCommandBuilder sqlBuilder = null;

		string s;

		//List<uint> removingElems = new List<uint>();

	
		public AdminForm(EntryForm entryForm)
		{
			//
			// ME: добавить CRUD-функционал
			InitializeComponent();
			this.FormClosing += (s, args) => Application.Exit();
			this.entryForm = entryForm;

			//sqlBuilder.GetInsertCommand();
			//sqlBuilder.GetUpdateCommand();
			//sqlBuilder.GetDeleteCommand();

			tabMenu.SelectedIndexChanged += tabMenu_SelectedIndexChanged;

			//dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridView1.AllowUserToAddRows = false;

			// Вызов метода `tabMenu_SelectedIndexChanged` для отображения начальной таблицы
			tabMenu_SelectedIndexChanged(this, new EventArgs());
		}

		private void tabMenu_SelectedIndexChanged(object? sender, EventArgs e)
		{
			int selectedIndex = tabMenu.SelectedIndex;
			ds = new DataSet();
			switch (selectedIndex)
			{
				// Пользователи
				case 0:
					s = "SELECT * FROM `users`";
					adapter = new MySqlDataAdapter(s, db.GetConnection());
					adapter.Fill(ds);
					dataGridView1.DataSource = ds.Tables[0];
					dataGridView1.Columns["id"].ReadOnly = true;
					break;
				// Группы
				case 1:
					s = "SELECT * FROM `groups`";
					adapter = new MySqlDataAdapter(s, db.GetConnection());
					adapter.Fill(ds);
					dataGridView1.DataSource = ds.Tables[0];
					break;
				// Ученики-группы
				case 2:
					s = "SELECT * FROM `student_group_connections`";
					adapter = new MySqlDataAdapter(s, db.GetConnection());
					adapter.Fill(ds);
					dataGridView1.DataSource = ds.Tables[0];
					break;
			}
		}

		// Добавление
		private void button2_Click(object sender, EventArgs e)
		{
			DataRow row = ds.Tables[0].NewRow();
			ds.Tables[0].Rows.Add(row);
		}

		// Удаление
		private void button1_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dataGridView1.SelectedRows)
			{
				dataGridView1.Rows.Remove(row);
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			//adapter = new MySqlDataAdapter(s, db.GetConnection());
			//adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
			//adapter.InsertCommand.Parameters.Add(new MySqlParameter("@name", MySqlDbType.VarChar, 50, "Name"));
			//adapter.InsertCommand.Parameters.Add(new MySqlParameter("@age", MySqlDbType.Int32, 0, "Age"));


		}

		private void button4_Click(object sender, EventArgs e)
		{
			tabMenu_SelectedIndexChanged(sender, EventArgs.Empty);
		}

		private void button5_Click(object sender, EventArgs e)
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
