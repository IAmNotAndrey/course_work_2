using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSchoolApp
{
	public class Group : List<uint>
	{
		private DB db = new DB();
		private MySqlDataAdapter adapter = new MySqlDataAdapter();

		//private CheckedListBox checkedListBox;

		public string Name { get; private set; }
		//private string name;

		//public Group(string groupName, CheckedListBox checkedListBox)
		public Group(string groupName)
		{
			//name = groupName;
			Name = groupName;
			//this.checkedListBox = checkedListBox;

			FillByStudents();
		}

		private void FillByStudents()
		{
			var cmd = new MySqlCommand(
				"SELECT `student` FROM `student_group_connections` " +
				"WHERE `group` = @groupName",
				db.GetConnection());
			cmd.Parameters.Add("@groupName", MySqlDbType.String).Value = Name;
			adapter.SelectCommand = cmd;
			var table = new DataTable();
			adapter.Fill(table);

			this.Clear();
			foreach (var item in table.Rows)
			{
				var row = (DataRow)item;
				uint studentId = row.Field<uint>("student");

				//this.Add(new User(row.Field<uint>("student")));
				this.Add(studentId);
			}
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
