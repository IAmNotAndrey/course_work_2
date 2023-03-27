using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSchoolApp
{
	public class TaskTreeNode : TreeNode
	{
		protected static DB db = new DB();
		protected static MySqlDataAdapter adapter = new MySqlDataAdapter();

		//public virtual int NodeId { get; set; }
		public int NodeId { get; set; }

		//private string name;
		public string NodeName
		{
			get
			{
				if (NodeId < 0)
					//return name;
					return Text;
				else
					return (string)GetNodeFieldValues(NodeId)["name"];
			}
			set
			{
				if (NodeId < 0) { }
				//name = value;
				else
				{
					var cmd = new MySqlCommand(
					$"UPDATE `nodes` SET " +
					$"`name` = @name " +
					$"WHERE `nodes`.`id` = @nodeId;",
					db.GetConnection());
					cmd.Parameters.Add("@name", MySqlDbType.String).Value = value;
					cmd.Parameters.Add("@nodeId", MySqlDbType.Int32).Value = NodeId;

					db.OpenConnection();
					int editedNum = cmd.ExecuteNonQuery();
					db.CloseConnection();
				}
				Text = value;
			}
		}
		//private int owner;
		public uint Owner
		{
			get
			{
				//if (NodeId < 0)
				//	return owner;
				//else
				return (uint)GetNodeFieldValues(NodeId)["owner"];
			}
			set
			{
				//if (NodeId < 0)
				//	owner = value;
				//else
				//{
				var cmd = new MySqlCommand(
				$"UPDATE `nodes` SET " +
				$"`owner` = @teacherId " +
				$"WHERE `nodes`.`id` = @nodeId;",
				db.GetConnection());
				cmd.Parameters.Add("@teacherId", MySqlDbType.Int32).Value = value;
				cmd.Parameters.Add("@nodeId", MySqlDbType.Int32).Value = NodeId;

				db.OpenConnection();
				int editedNum = cmd.ExecuteNonQuery();
				db.CloseConnection();
				//}
			}
		}
		private string description;
		public string Description
		{
			get
			{
				if (NodeId < 0)
					return description;
				else
					return (string)GetNodeFieldValues(NodeId)["description"];
			}
			set
			{
				if (NodeId < 0)
					description = value;
				else
				{
					var cmd = new MySqlCommand(
						$"UPDATE `nodes` SET " +
						$"`description` = @description " +
						$"WHERE `nodes`.`id` = @nodeId;",
				db.GetConnection());
					cmd.Parameters.Add("@description", MySqlDbType.VarChar).Value = value;
					cmd.Parameters.Add("@nodeId", MySqlDbType.Int32).Value = NodeId;

					db.OpenConnection();
					int editedNum = cmd.ExecuteNonQuery();
					db.CloseConnection();
				}
			}
		}
		private int? parentId;
		public int? ParentId
		{
			get
			{
				if (NodeId < 0)
					return parentId;
				else
					return (int?)(uint?)GetNodeFieldValues(NodeId)["parent"];
			}
			set
			{
				if (NodeId < 0)
					parentId = value;
				else
				{
					var cmd = new MySqlCommand(
					$"UPDATE `nodes` SET " +
					$"`parent` = @parentId " +
					$"WHERE `nodes`.`id` = @nodeId;",
					db.GetConnection());
					// ! Возможна ошибка при передаче null
					cmd.Parameters.Add("@parentId", MySqlDbType.Int32).Value = (uint?)value;
					cmd.Parameters.Add("@nodeId", MySqlDbType.Int32).Value = NodeId;

					db.OpenConnection();
					int editedNum = cmd.ExecuteNonQuery();
					db.CloseConnection();
				}
			}
		}
		public uint Priority
		{
			get
			{
				return (uint)GetNodeFieldValues(NodeId)["priority"];
			}
			set
			{
				if (NodeId >= 0)
				{
					var cmd = new MySqlCommand(
					$"UPDATE `nodes` SET " +
					$"`priority` = @priority " +
					$"WHERE `nodes`.`id` = @nodeId;",
					db.GetConnection());
					cmd.Parameters.Add("@priority", MySqlDbType.UInt32).Value = value;
					cmd.Parameters.Add("@nodeId", MySqlDbType.Int32).Value = NodeId;

					db.OpenConnection();
					int editedNum = cmd.ExecuteNonQuery();
					db.CloseConnection();
				}
			}
		}

		public TaskTreeNode(int id)
		{
			NodeId = id;
			Text = NodeName;
		}

		/// <summary>
		/// Получает все значения полей из таблицы nodes по его id
		/// </summary>
		public static Dictionary<string, object>? GetNodeFieldValues(int nodeId)
		{
			if (nodeId < 0)
				return null;

			var cmd = new MySqlCommand(
					"SELECT * FROM `nodes` WHERE `id` = @nodeId",
					db.GetConnection());
			cmd.Parameters.Add("@nodeId", MySqlDbType.Int32).Value = nodeId;
			adapter.SelectCommand = cmd;
			var table = new DataTable();
			adapter.Fill(table);

			string name = table.Rows[0].Field<string>("name");
			uint owner = table.Rows[0].Field<uint>("owner");
			string description = table.Rows[0].Field<string>("description");
			uint? parent = table.Rows[0].Field<uint?>("parent");
			uint priority = table.Rows[0].Field<uint>("priority");

			return new Dictionary<string, object>() {
				{"name", name},
				{"owner", owner},
				{"description", description},
				{"parent", parent},
				{"priority", priority}
			};
		}
	}
}
