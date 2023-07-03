using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MusicSchoolAsp.Models.Db;

public partial class Role
{
	[Display(Name = "Название")]
	public string Name { get; set; } = null!;

    public uint Id { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
