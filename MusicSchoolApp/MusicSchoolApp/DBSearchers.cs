using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSchoolApp
{
	public static class DBSearchers
	{
		private static DB db = new DB();
		private static MySqlDataAdapter adapter = new MySqlDataAdapter();

		public static DataTable GetAllTasksByTeacher(uint teacherId)
		{
			// Находим все задания, создателем которых является текущий преподаватель
			var cmd = new MySqlCommand(
				"SELECT `id` FROM `nodes` " +
				"WHERE `owner` = @teacherId",
				db.GetConnection());
			cmd.Parameters.Add("@teacherId", MySqlDbType.UInt32).Value = teacherId;
			adapter.SelectCommand = cmd;
			var table = new DataTable();
			adapter.Fill(table);

			return table;
		}
	}
}
