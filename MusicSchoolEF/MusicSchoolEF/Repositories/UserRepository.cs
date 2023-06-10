using Microsoft.EntityFrameworkCore;
using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Repositories.Interfaces;

namespace MusicSchoolEF.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly Ms2Context _dbContext;

		public UserRepository(Ms2Context dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<User?> GetUserByLoginAsync(string login)
		{
			return await _dbContext.Users
				.SingleOrDefaultAsync(u => u.Login == login);
		}

		public async Task<User?> GetUserByIdAsync(uint userId)
		{
			return await _dbContext.Users
				.SingleOrDefaultAsync(u => u.Id == userId);
		}

		public async Task<List<User>> GetSortedUsersByFullNameAsync(List<User> users)
		{
			return await Task.Run(() =>
			{
				return users
					.OrderBy(s => s.Surname)
					.ThenBy(s => s.FirstName)
					.ThenBy(s => s.Patronymic)
					.ToList();
			});
		}
	}
}
