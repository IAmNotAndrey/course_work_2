namespace MusicSchoolEF.Models.Defaults
{
    public class TreeNode<TValue>
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
    }
}
