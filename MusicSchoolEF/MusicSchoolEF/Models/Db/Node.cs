using System;
using System.Collections.Generic;

namespace MusicSchoolEF.Models.Db;

public partial class Node
{
    public uint Id { get; set; }

    public string Name { get; set; } = null!;

    public uint Owner { get; set; }

    public string? Description { get; set; }

    public uint? ParentId { get; set; }

    public uint Priority { get; set; }

    public virtual ICollection<Node> InverseParent { get; set; } = new List<Node>();

    public virtual User OwnerNavigation { get; set; } = null!;

    public virtual Node? Parent { get; set; }

    public virtual ICollection<StudentNodeConnection> StudentNodeConnections { get; set; } = new List<StudentNodeConnection>();

    public static List<Node> GetNodeAndDescendants(Node parent)
    {
        List<Node> nodes = new();
        GetNodeAndDescendantsRecursive(parent, ref nodes);
        return nodes;
    }

    private static void GetNodeAndDescendantsRecursive(Node node, ref List<Node> nodes)
    {
        nodes.Add(node);
        foreach (var childNode in node.InverseParent)
        {
            GetNodeAndDescendantsRecursive(childNode, ref nodes);
        }
    }
}
