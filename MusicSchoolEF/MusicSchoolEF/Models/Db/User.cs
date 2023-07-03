using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Text;
using System.Xml.Linq;

namespace MusicSchoolAsp.Models.Db;

public partial class User
{
    public uint Id { get; set; }

    public uint RoleId { get; set; }

	[Display(Name = "Имя")]
	public string FirstName { get; set; } = null!;

	[Display(Name = "Фамилия")]
	public string Surname { get; set; } = null!;

	[Display(Name = "Отчество")]
	public string Patronymic { get; set; } = null!;

	[Display(Name = "Логин")]
	public string Login { get; set; } = null!;

	[Display(Name = "Пароль")]
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
