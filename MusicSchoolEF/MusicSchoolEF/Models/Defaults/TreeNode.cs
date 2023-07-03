using System.Collections;

namespace MusicSchoolAsp.Models.Defaults
{
    public class TreeNode<TValue> : IEnumerable<(TreeNode<TValue> TreeNode, int Level)>
    {
        public TValue? Value { get; set; }
        public List<TreeNode<TValue>> Children { get; set; } = new List<TreeNode<TValue>>();
        public bool IsEmpty => Value == null;
        public bool HasChildren => Children.Count != 0;

        public TreeNode() { }
        public TreeNode(TValue? value)
        {
            Value = value;
        }
        public TreeNode(TValue? value, List<TreeNode<TValue>> children) : this(value)
        {
            Children = children;
        }

        public void AddChild(TreeNode<TValue> node)
        {
            Children.Add(node);
        }

        public IEnumerator<(TreeNode<TValue> TreeNode, int Level)> GetEnumerator()
        {
            yield return (this, 0); // Возвращаем корневой узел с уровнем 0

            foreach (var child in Children)
            {
                foreach (var item in child.Traverse(1)) // Рекурсивно перечисляем дочерние узлы, начиная с уровня 1
                {
                    yield return item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<(TreeNode<TValue> TreeNode, int Level)> Traverse(int level)
        {
            yield return (this, level); // Возвращаем текущий узел с его уровнем

            foreach (var child in Children)
            {
                foreach (var item in child.Traverse(level + 1)) // Рекурсивно перечисляем дочерние узлы с уровнем level 1
                {
                    yield return item;
                }
            }
        }
    }
}
