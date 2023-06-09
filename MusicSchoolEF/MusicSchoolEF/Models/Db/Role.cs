using System;
using System.Collections.Generic;

namespace MusicSchoolEF.Models.Db;

public partial class Role
{
    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
