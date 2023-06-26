using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MusicSchoolEF.Models.Db;

public partial class StudentNodeConnection
{
    public uint NodeId { get; set; }

    public uint StudentId { get; set; }

	[Display(Name = "Оценка")]
	public int? Mark { get; set; }

	[Display(Name = "Комментарий")]
	public string? Comment { get; set; }

    public virtual Node Node { get; set; } = null!;

    public virtual User Student { get; set; } = null!;
}
