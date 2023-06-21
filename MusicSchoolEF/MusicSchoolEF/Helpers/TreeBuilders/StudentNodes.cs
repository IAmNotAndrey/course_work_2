using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.Defaults;

namespace MusicSchoolEF.Helpers.TreeBuilders
{
    public static partial class TreeBuilder
    {
		// note Возможна ситуация, когда дерево отображено в `nodes` (БД) корректно: все вершины образуют целостное дерево - однако в `student_node_connections` могут быть добавлены не все вершины из этого целостного дерева. Таким образом, образуются разорванные цепочки, которые будут представлены в виде разных деревьев в функции `GetStudentNodeConnectionsTree`.

		/// <param name="collection">Коллекция всех `StudentNodeConnections`, связанных с определённым студентом.
		/// Для корректной работы требуется подключить `StudentNodeConnection`.`NodeNavigation` и  `StudentNodeConnection`.`NodeNavigation`.`InverseParentNavigation`</param>
		/// <returns>Дерево "Родитель-Ребёнок" `StudentNodeConnections`-элементов</returns>
		public static TreeNode<StudentNodeConnection> GetStudentNodesTree(ICollection<StudentNodeConnection> collection)
        {
            // Создаём корневой элемент дерева с пустым `content`
            var treeRoot = new TreeNode<StudentNodeConnection>();
            // Вызываем рекурсивную функцию, которая заполнит вершины дочерними элементами
            Rec(ref treeRoot, in collection);

            return treeRoot;
        }

        private static void Rec(ref TreeNode<StudentNodeConnection> treeNode, in ICollection<StudentNodeConnection> collection)
        {
            // Если передаётся корень
            if (treeNode.IsEmpty)
            {
                // Сортируем по приоритету, потом по названию
                var orderedCollection = collection
                    .OrderBy(snc => snc.Node.Priority)
                    .ThenBy(snc => snc.Node.Name);

                foreach (var snc in orderedCollection)
                {
                    // Ищем все корневые вершины: у которых `Node`.`Parent` = (null или вершине, которой нет в `collection`)
                    if (snc.Node.Parent == null
                        || !collection.Where(_snc => _snc.Node == snc.Node.Parent).Any())
                    {
                        // Заполняем первый уровень дерева
                        var childTreeNode = new TreeNode<StudentNodeConnection>(snc);
                        treeNode.AddChild(childTreeNode);
                        // Рекурсивно добавляем детей к текущей вершине, если они (дети) есть
                        Rec(ref childTreeNode, in collection);
                    }
                }
                return;
            }
            // Если передаётся НЕ корень
            // Принимаем родителя, Value которого не может быть `null`, т.к. выше идёт проверка в условии `if`
            var parent_snc = treeNode.Value!;
            // Проходимся по его детям в представлении `Node` и добавляем в `treeNode`.`Children` всех, кто есть в `collection`
            foreach (Node child in parent_snc.Node.InverseParent)
            {
                var found_snc_child = collection.SingleOrDefault(snc => snc.NodeId == child.Id);
                if (found_snc_child != null)
                {
                    var childTreeNode = new TreeNode<StudentNodeConnection>(found_snc_child);
                    treeNode.AddChild(childTreeNode);

                    Rec(ref childTreeNode, in collection);
                }
            }
        }
    }
}
