using MusicSchoolAsp.Models.Db;
using MusicSchoolAsp.Models.Defaults;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MusicSchoolAsp.Helpers.JsonStrings
{
	public static class TreeRender
	{
		public static string RenderTree(TreeNode<StudentNodeConnection> node, string rootName)
		{
			var jsonTree = GenerateTreeNodeJson(node, rootName);
			var treeScript = $"var tree = {jsonTree};";
			return treeScript;
		}

		private static string GenerateTreeNodeJson(TreeNode<StudentNodeConnection> node, string rootName = "")
		{
			var treeNode = new
			{
				text = node.Value?.Node.Name ?? rootName,
				id = node.Value?.NodeId.ToString() ?? "",
				description = node.Value?.Node.Description ?? "",
				mark = node.Value?.Mark?.ToString() ?? "",
				comment = node.Value?.Comment ?? "",
				teacher_name = node.Value?.Node.OwnerNavigation.FullName,

				nodes = node.Children?.Select(child => GenerateTreeNodeJson(child)).ToArray()
			};

			JsonSerializerOptions options = new JsonSerializerOptions
			{
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
				WriteIndented = true,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
			};

			return JsonSerializer.Serialize(treeNode, options);
		}

		private static string FormatJsonString(string jsonString)
		{
			using (var doc = JsonDocument.Parse(jsonString))
			{
				return doc.RootElement.ToString();
			}
		}
	}
}
