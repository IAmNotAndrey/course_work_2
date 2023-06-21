using MusicSchoolEF.Models.Db;

namespace MusicSchoolEF.Repositories.Interfaces
{
	public interface IUserRepository
	{
		Task<User?> GetUserByLoginAsync(string login);
		Task<User?> GetUserByIdAsync(uint userId);
		IQueryable<User> GetSortedUsersByFullName(IQueryable<User> users);
        bool DoesConsistOfSameStudents(IEnumerable<uint> studentIds1, IEnumerable<uint> studentIds2);
		bool DoesConsistOfSameStudents(IEnumerable<uint> studentIds1, IEnumerable<IEnumerable<uint>> otherStudentIds);
    }
}
