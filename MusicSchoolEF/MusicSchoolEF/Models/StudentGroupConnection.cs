using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MusicSchoolEF.Models
{
	[PrimaryKey("Student", "Group")]
	public class StudentGroupConnection
	{
		public int Student { get; set; }
		public string Group { get; set; }
	}
}
