using MusicSchoolEF.Models.Db;

namespace MusicSchoolEF.Repositories.Interfaces
{
    public interface IGroupRepository
	{
		Task<List<Group>> GetAllGroupsAsync();
	}
}
