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

        public bool DoesConsistOfSameStudents(IEnumerable<uint> studentIds1, IEnumerable<uint> studentIds2)
        {
			//// Получаем список Id пользователей из первой группы
			//var usersInGroup1 = students1.Select(u => u.Id);

			//// Получаем список Id пользователей из второй группы
			//var usersInGroup2 = students2.Select(u => u.Id);

			// Проверяем, содержатся ли все пользователи из первой группы во второй группе
			//var hasSameStudents = usersInGroup1.All(id => usersInGroup2.Contains(id));
			var hasSameStudents = studentIds1.All(id => studentIds2.Contains(id));

            return hasSameStudents;
        }

        public bool DoesConsistOfSameStudents(IEnumerable<uint> studentIds1, IEnumerable<IEnumerable<uint>> otherStudentIds)
        {
			if (otherStudentIds.Count() == 0)
				return false;

            foreach (var otherGroup in otherStudentIds)
            {
                var hasSameStudents = DoesConsistOfSameStudents(studentIds1, otherGroup);

                if (!hasSameStudents)
                    return false;
            }
            // Если все группы содержат одних и тех же пользователей, возвращаем true
            return true;
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
