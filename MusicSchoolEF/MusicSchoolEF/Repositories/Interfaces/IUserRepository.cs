using MusicSchoolEF.Models.Db;

namespace MusicSchoolEF.Repositories.Interfaces
{
	public interface IUserRepository
	{
		Task<User?> GetUserByLoginAsync(string login);
		Task<User?> GetUserByIdAsync(uint userId);
		Task<List<User>> GetSortedUsersByFullNameAsync(List<User> users);
	}
}
