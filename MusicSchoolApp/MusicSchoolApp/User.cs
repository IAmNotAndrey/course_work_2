using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSchoolApp
{
	public class User
	{
		private static DB db = new DB();
		private static MySqlDataAdapter adapter = new MySqlDataAdapter();

		public uint Id { get; set; }
		public string FirstName
		{
			get { return GetStudentFieldValues(Id)["first_name"]; }
		}
		public string Surname
		{
			get { return GetStudentFieldValues(Id)["surname"]; }
		}
		public string Patronymic
		{
			get { return GetStudentFieldValues(Id)["patronymic"]; }
		}
		public string Login
		{
			get { return GetStudentFieldValues(Id)["login"]; }
		}

		public string FullName
		{
			get { return this.ToString(); }
		}
		///// <summary>
		///// Свойство для работы с CheckedListBox
		///// </summary>
		//public bool Checked { get; set; }

		public User(uint id)
		{
			this.Id = id;
		}

		public override string ToString()
		{
			//return $"{FirstName} {Surname} {Patronymic}";
			return $"{Surname} {FirstName} {Patronymic}";
		}

		/// <summary>
		/// Получает все значения полей из таблицы students по его id
		/// </summary>
		private static Dictionary<string, string> GetStudentFieldValues(uint studentId)
		{
			var cmd = new MySqlCommand(
					"SELECT * FROM `users` WHERE `id` = @studentId",
					db.GetConnection());
			cmd.Parameters.Add("@studentId", MySqlDbType.Int32).Value = studentId;
			adapter.SelectCommand = cmd;
			var table = new DataTable();
			adapter.Fill(table);

			string firstName = table.Rows[0].Field<string>("first_name");
			string surname = table.Rows[0].Field<string>("surname");
			string patronymic = table.Rows[0].Field<string>("patronymic");
			string login = table.Rows[0].Field<string>("login");


			return new Dictionary<string, string>() {
				{"first_name", firstName},
				{"surname", surname},
				{"patronymic", patronymic},
				{"login", login}
			};
		}
	}
}
