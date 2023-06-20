using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.Defaults;

namespace MusicSchoolEF.Helpers.HtmlStrings.TreeRenders
{
    public static class TreeRender
    {
        public static string RenderTree(TreeNode<StudentNodeConnection> node, string rootName)
        {
            using (var writer = new StringWriter())
            {
                var snc = node.Value;
                // Название для корневого элемента дерева, который выступает в роли заглушки
                string nodeName = snc?.Node.Name ?? rootName;
                uint? nodeId = snc?.NodeId;
                string nodeDescription = snc?.Node.Description ?? "";
                string nodeMark = snc?.Mark.ToString() ?? "";
                string nodeComment = snc?.Comment ?? "";
                
                string teacherFullName = "";
                User? teacher = snc?.Node.OwnerNavigation;
                if (teacher != null)
                    teacherFullName += $"{teacher.Surname} {teacher.FirstName} {teacher.Patronymic}";

                writer.WriteLine(
                        $@"<li class='tree-node' 
					data-nodeid='{nodeId}' 
					data-nodename='{nodeName}' 
                    data-nodedescription='{nodeDescription}' 
                    data-nodemark='{nodeMark}' 
                    data-nodecomment='{nodeComment}' 
                    data-nodeteachername='{teacherFullName}'>"
                    );
                writer.WriteLine($"{nodeName}");

                if (node.HasChildren)
                {
                    writer.WriteLine("<ul>");
                    foreach (var child in node.Children)
                    {
                        writer.WriteLine(RenderTree(child, ""));
                    }
                    writer.WriteLine("</ul>");
                }
                writer.WriteLine("</li>");

                return writer.ToString();
            }
        }

        public static string RenderTree(TreeNode<Node> _node, string rootName)
        {
            using (var writer = new StringWriter())
            {
                Node? node = _node.Value;
                uint? nodeId = node?.Id;
                string nodeName = node?.Name ?? rootName;
                string nodeDescription = node?.Description ?? "";

                writer.WriteLine(
                                $@"<li class='tree-node'
                    data-nodeid='{nodeId}'
                    data-nodename='{nodeName}'
                    data-nodedescription='{nodeDescription}'>
			        {nodeName}
                    <span>
                        <button class='add-button' title='Добавить дочернюю вершину'>
                            <i class='fas fa-plus'></i>
                        </button>
                        <button class='delete-button' title='Удалить вершину и потомков'>
                            <i class='fas fa-minus'></i>
                        </button>
                    </span>"
                );

                if (_node.HasChildren)
                {
                    writer.WriteLine("<ul>");
                    foreach (var child in _node.Children)
                    {
                        writer.WriteLine(RenderTree(child, ""));
                    }
                    writer.WriteLine("</ul>");
                }
                writer.WriteLine("</li>");

                return writer.ToString();
            }
        }
    }
}
