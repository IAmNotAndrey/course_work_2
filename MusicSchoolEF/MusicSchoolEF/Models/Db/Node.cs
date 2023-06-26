using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicSchoolEF.Models.Db;

public partial class Node
{
    public uint Id { get; set; }

    [Display(Name = "Название")]
	public string Name { get; set; } = null!;

	[Display(Name = "Преподаватель")]
	public uint Owner { get; set; }

	[Display(Name = "Описание")]
	public string? Description { get; set; }

	public uint? ParentId { get; set; }

	[Display(Name = "Приоритет")]
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
