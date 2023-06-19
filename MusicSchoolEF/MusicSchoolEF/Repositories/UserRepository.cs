using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Repositories.Interfaces;
using System.Collections.Generic;

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

		public IQueryable<User> GetSortedUsersByFullName(IQueryable<User> users)
		{
			return users
				.OrderBy(s => s.Surname)
				.ThenBy(s => s.FirstName)
				.ThenBy(s => s.Patronymic);
		}
	}

	public static class UserRepositoryExtensions
	{
		private static readonly IUserRepository _userRepository = new UserRepository(new Ms2Context());

		public static IQueryable<User> GetSortedUsersByFullName(this IQueryable<User> users)
		{
			return _userRepository
				.GetSortedUsersByFullName(users);
		}
	}
}
