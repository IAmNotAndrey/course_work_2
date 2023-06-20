using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.Defaults;

namespace MusicSchoolEF.Helpers.TreeBuilders
{
	public static partial class TreeBuilder
	{
		// note Замечания, сделанные в `StudentNodes.cs` справедливы и для `TeacherNodes.cs`
		// todo Сделать сортировку по `Priority`

		public static TreeNode<Node> GetTeacherNodesTree(ICollection<Node> collection)
		{
			// Создаём корневой элемент дерева с пустым `content`
			var treeRoot = new TreeNode<Node>();
			// Вызываем рекурсивную функцию, которая заполнит вершины дочерними элементами
			Rec(ref treeRoot, in collection);

			return treeRoot;
		}

		private static void Rec(ref TreeNode<Node> treeNode, in ICollection<Node> collection)
		{
			// Если передаётся корень
			if (treeNode.IsEmpty)
			{
				foreach (var node in collection)
				{
					// Ищем все корневые вершины: у которых `Node`.`Parent` = (null или вершине, которой нет в `collection`)
					if (node.Parent == null
						|| !collection.Where(_node => _node.Id == node.ParentId).Any())
					{
						// Заполняем первый уровень дерева
						var childTreeNode = new TreeNode<Node>(node);
						treeNode.AddChild(childTreeNode);
						// Рекурсивно добавляем детей к текущей вершине, если они (дети) есть
						Rec(ref childTreeNode, in collection);
					}
				}
				return;
			}
			// Если передаётся НЕ корень
			// Принимаем родителя, Value которого не может быть `null`, т.к. выше идёт проверка в условии `if`
			var parent = treeNode.Value!;
			// Проходимся по его детям в представлении `Node` и добавляем в `treeNode`.`Children` всех, кто есть в `collection`
			foreach (Node child in parent.InverseParent)
			{
				var found_child = collection.SingleOrDefault(snc => snc.Id == child.Id);
				if (found_child != null)
				{
					var childTreeNode = new TreeNode<Node>(found_child);
					treeNode.AddChild(childTreeNode);

					Rec(ref childTreeNode, in collection);
				}
			}
		}
	}
}
