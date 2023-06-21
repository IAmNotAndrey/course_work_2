using System;
using System.Collections.Generic;
using System.Drawing.Text;

namespace MusicSchoolEF.Models.Db;

public partial class User
{
    public uint Id { get; set; }

    public uint RoleId { get; set; }

    public string FirstName { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Node> Nodes { get; set; } = new List<Node>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<StudentNodeConnection> StudentNodeConnections { get; set; } = new List<StudentNodeConnection>();

    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

    public string FullName => _GetFullName();

    public string AllText => _GetAllText();

    private string _GetFullName()
    {
        return $"{Surname} {FirstName} {Patronymic}";
    }

    private string _GetAllText()
    {
        // fixme ошибка "System.InvalidOperationException: "This MySqlConnection is already in use. See https://fl.vu/mysql-conn-reuse" при попытке использовать `Role.Name`".
        // Кроме того, возникает ошибка с Role.Name = null при попытке создать новый `User.
        //return $"{FullName} {Role?.Name ?? ""} {Login} {Password}";
        return $"{FullName} {Login} {Password}";
    }
}
