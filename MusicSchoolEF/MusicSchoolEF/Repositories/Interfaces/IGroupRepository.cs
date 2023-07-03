using MusicSchoolAsp.Models.Db;

namespace MusicSchoolAsp.Repositories.Interfaces
{
    public interface IGroupRepository
	{
		Task<List<Group>> GetAllGroupsAsync();
		//bool DoesGroupConsistOfSameStudents(Group group1, Group group2);
		//bool DoesGroupConsistOfSameStudents(Group group1, IEnumerable<Group> groups);
	}
}
