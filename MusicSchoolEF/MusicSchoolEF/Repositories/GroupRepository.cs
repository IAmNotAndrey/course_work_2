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
	}
}
