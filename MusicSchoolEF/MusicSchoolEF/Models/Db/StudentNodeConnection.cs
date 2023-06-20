using System;
using System.Collections.Generic;

namespace MusicSchoolEF.Models.Db;

public partial class StudentNodeConnection
{
    public uint NodeId { get; set; }

    public uint StudentId { get; set; }

    public int? Mark { get; set; }

    public string? Comment { get; set; }

    public virtual Node Node { get; set; } = null!;

    public virtual User Student { get; set; } = null!;
}
