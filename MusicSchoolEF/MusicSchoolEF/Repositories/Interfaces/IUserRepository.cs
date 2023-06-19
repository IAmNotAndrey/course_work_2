using MusicSchoolEF.Models.Db;

namespace MusicSchoolEF.Repositories.Interfaces
{
	public interface IUserRepository
	{
		Task<User?> GetUserByLoginAsync(string login);
		Task<User?> GetUserByIdAsync(uint userId);
		IQueryable<User> GetSortedUsersByFullName(IQueryable<User> users);
	}
}
