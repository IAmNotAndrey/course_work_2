using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSchoolApp
{
	public class StudentTaskTreeNode : TaskTreeNode
	{
		public int StudentId { get; set; }
		public int? Mark
		{
			get
			{
				return (int?)GetStudentNodeFieldValues(StudentId, NodeId)["mark"];
			}
			set
			{
				var cmd = new MySqlCommand(
					$"UPDATE `student_node_connections` SET " +
					$"`mark` = @mark " +
					$"WHERE `student_node_connections`.`node` = @nodeId " +
					$"AND `student_node_connections`.`student` = @studentId ",
					db.GetConnection());
				cmd.Parameters.Add("@mark", MySqlDbType.Int32).Value = value;
				cmd.Parameters.Add("@nodeId", MySqlDbType.Int32).Value = NodeId;
				cmd.Parameters.Add("@studentId", MySqlDbType.Int32).Value = StudentId;

				db.OpenConnection();
				int editedNum = cmd.ExecuteNonQuery();
				db.CloseConnection();
			}
		}

		public string? Comment
		{
			get
			{
				return (string?)GetStudentNodeFieldValues(StudentId, NodeId)["comment"];
			}
			set
			{
				var cmd = new MySqlCommand(
					$"UPDATE `student_node_connections` SET " +
					$"`comment` = @comment " +
					$"WHERE `student_node_connections`.`node` = @nodeId " +
					$"AND `student_node_connections`.`student` = @studentId ",
					db.GetConnection());
				cmd.Parameters.Add("@comment", MySqlDbType.VarChar).Value = value;
				cmd.Parameters.Add("@nodeId", MySqlDbType.Int32).Value = NodeId;
				cmd.Parameters.Add("@studentId", MySqlDbType.Int32).Value = StudentId;

				db.OpenConnection();
				int editedNum = cmd.ExecuteNonQuery();
				db.CloseConnection();
			}
		}

		public StudentTaskTreeNode(int studentId, int nodeId) : base(nodeId)
		{
			StudentId = studentId;
		}

		public static Dictionary<string, object> GetStudentNodeFieldValues(int studentId, int nodeId)
		{
			var cmd = new MySqlCommand(
					"SELECT * FROM `student_node_connections` " +
					"WHERE `node` = @nodeId AND `student` = @studentId",
					db.GetConnection());
			cmd.Parameters.Add("@nodeId", MySqlDbType.Int32).Value = nodeId;
			cmd.Parameters.Add("@studentId", MySqlDbType.Int32).Value = studentId;
			adapter.SelectCommand = cmd;
			var table = new DataTable();
			adapter.Fill(table);

			int? mark = table.Rows[0].Field<int?>("mark");
			string? comment = table.Rows[0].Field<string?>("comment");

			return new Dictionary<string, object>() {
				{"mark", mark},
				{"comment", comment}
			};
		}
	}
}
