using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace MusicSchoolApp
{
	public class DB
	{
		const string server = "localhost";
		const string port = "3306";
		const string username = "root";
		const string password = "root";
		const string databaseName = "ms_2";

		MySqlConnection connection = new MySqlConnection($"server={server};port={port};username={username};password={password};database={databaseName};");

		public void OpenConnection()
		{
			if (connection.State == System.Data.ConnectionState.Closed)
				connection.Open();
		}

		public void CloseConnection()
		{
			if (connection.State == System.Data.ConnectionState.Open)
				connection.Close();
		}

		public MySqlConnection GetConnection()
		{
			return connection;
		}
	}
}
