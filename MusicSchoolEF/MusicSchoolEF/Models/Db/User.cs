using System;
using System.Collections.Generic;

namespace MusicSchoolEF.Models.Db;

public partial class User
{
    public uint Id { get; set; }

    public string Role { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Node> Nodes { get; set; } = new List<Node>();

    public virtual Role RoleNavigation { get; set; } = null!;

    public virtual ICollection<StudentNodeConnection> StudentNodeConnections { get; set; } = new List<StudentNodeConnection>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
}
