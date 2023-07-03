using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicSchoolAsp.Models.Db;

public partial class Group
{
    [Display(Name = "Название")]
    public string Name { get; set; } = null!;

    public uint Id { get; set; }

    public virtual ICollection<User> Students { get; set; } = new List<User>();
}
