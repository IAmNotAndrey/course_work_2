using Microsoft.EntityFrameworkCore;
using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Repositories.Interfaces;

namespace MusicSchoolEF.Repositories
{
    public class GroupRepository : IGroupRepository
	{
		private readonly Ms2Context _dbContext;

		public GroupRepository(Ms2Context dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Group>> GetAllGroupsAsync()
		{
			return await _dbContext.Groups
				//.Include(g => g.Students)
				//.ThenInclude(s => s.StudentNodeConnections)
				.ToListAsync();
		}

        //public bool DoesGroupConsistOfSameStudents(Group group1, Group group2)
        //{
        //    // Получаем список Id пользователей из первой группы
        //    var usersInGroup1 = group1.Students.Select(u => u.Id);

        //    // Получаем список Id пользователей из второй группы
        //    var usersInGroup2 = group2.Students.Select(u => u.Id);

        //    // Проверяем, содержатся ли все пользователи из первой группы во второй группе
        //    var hasSameStudents = usersInGroup1.All(id => usersInGroup2.Contains(id));

        //    return hasSameStudents;
        //}

        //public bool DoesGroupConsistOfSameStudents(Group group, IEnumerable<Group> groups)
        //{
        //    foreach (var otherGroup in groups)
        //    {
        //        var hasSameStudents = DoesGroupConsistOfSameStudents(group, otherGroup);

        //        if (!hasSameStudents)
        //            return false;
        //    }
        //    // Если все группы содержат одних и тех же пользователей, возвращаем true
        //    return true;
        //}
    }
}
