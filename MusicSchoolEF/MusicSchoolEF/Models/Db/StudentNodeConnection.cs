using System;
using System.Collections.Generic;

namespace MusicSchoolEF.Models.Db;

public partial class StudentNodeConnection
{
    public uint Node { get; set; }

    public uint Student { get; set; }

    public int? Mark { get; set; }

    public string? Comment { get; set; }

    public virtual Node NodeNavigation { get; set; } = null!;

    public virtual User StudentNavigation { get; set; } = null!;
}
