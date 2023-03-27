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
	public partial class EntryForm : Form
	{
		private static Dictionary<string, string> roles = new Dictionary<string, string>()
		{
			{ "admin", "admin" },
			{ "student", "student" },
			{ "teacher", "teacher" },
		};

		public EntryForm()
		{
			InitializeComponent();
		}


		private void button1_Click(object sender, EventArgs e)
		{
			string login = loginField.Text.Trim();
			string password = passwordField.Text.Trim();

			DB db = new DB();
			DataTable table = new DataTable();
			MySqlDataAdapter adapter = new MySqlDataAdapter();
			MySqlCommand cmd = new MySqlCommand(
				"SELECT * FROM `users` " +
				"WHERE `login` = @userLogin " +
				"AND `password` = @userPassword",
				db.GetConnection());
			cmd.Parameters.Add("@userLogin", MySqlDbType.String).Value = login;
			cmd.Parameters.Add("@userPassword", MySqlDbType.String).Value = password;

			adapter.SelectCommand = cmd;
			adapter.Fill(table);

			if (table.Rows.Count == 0)
			{
				// TODO: добавить анимацию изменения цвета и уведомления
				MessageBox.Show("Неправильный логин или пароль");
			}
			else
			{
				// Достаём id найденного пользователя
				uint userId = table.Rows[0].Field<uint>("id");
				// Узнаём его роль
				string role = table.Rows[0].Field<string>("role");

				this.Hide();
				switch (role)
				{
					// hack: не работает с roles
					case "admin":
						var adminForm = new AdminForm(this);
						adminForm.Show();
						break;

					case "student":
						var studentForm = new StudentForm(userId, this);
						studentForm.Show();
						break;

					case "teacher":
						var teacherForm = new TeacherForm(userId, this);
						teacherForm.Show();
						break;

					case default(string):
						throw new Exception("Необратаываемая роль");
				}
			}
		}
	}
}
