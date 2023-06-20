﻿using System;
using System.Collections.Generic;

namespace MusicSchoolEF.Models.Db;

public partial class Group
{
    public string Name { get; set; } = null!;

    public uint Id { get; set; }

    public virtual ICollection<User> Students { get; set; } = new List<User>();
}
