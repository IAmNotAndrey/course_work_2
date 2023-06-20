using Microsoft.EntityFrameworkCore;
using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.ViewModels;
using MusicSchoolEF.Repositories.Interfaces;

namespace MusicSchoolEF.Repositories
{
    public class NodeRepository : INodeRepository
    {
		private readonly Ms2Context _dbContext;

		public NodeRepository(Ms2Context dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Node>> GetNodesByOwnerIdAsync(uint ownerId)
        {
            List<Node> allTeacherTasks = await _dbContext.Nodes
                //.Include(node => node.InverseParentNavigation) // Подключаем детей у задания
                .Where(node => node.Owner == ownerId)
				.ToListAsync();

            return allTeacherTasks;
        }

        public async Task<Node?> GetNodeByIdAsync(uint nodeId)
        {
			return await _dbContext.Nodes
				//.Include(n => n.InverseParentNavigation)
				.SingleOrDefaultAsync(n => n.Id == nodeId);
		}

		public async Task EditNodeNameAndDescriptionAsync(Node editingNode, TeacherTaskEditViewModel model)
		{
			editingNode.Name = model.Name;
			editingNode.Description = model.Description;
			editingNode.Priority = model.Priority;

			await _dbContext.SaveChangesAsync();
		}

		public async Task AddAsync(Node node)
		{
			await _dbContext.Nodes.AddAsync(node);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<List<Node>> GetNodesByOwnerAndParentAsync(uint ownerId, uint? parentId)
		{
			return await _dbContext.Nodes
				//.Include(n => n.InverseParentNavigation)
				.Where(n => n.Owner == ownerId && n.ParentId == parentId)
				.ToListAsync(); 
		}

		public async Task RemoveRangeAsync(IEnumerable<Node> nodes)
		{
			_dbContext.Nodes.RemoveRange(nodes);
			await _dbContext.SaveChangesAsync();
		}

		public async Task RemoveAsync(Node node)
		{
			_dbContext.Nodes.Remove(node);
			await _dbContext.SaveChangesAsync();
		}
	}
}
