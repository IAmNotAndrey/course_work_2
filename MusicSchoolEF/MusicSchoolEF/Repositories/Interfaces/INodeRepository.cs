using MusicSchoolEF.Models.Db;

namespace MusicSchoolEF.Repositories.Interfaces
{
	public interface INodeRepository
	{
		Task<List<Node>> GetNodesByOwnerIdAsync(uint ownerId);
		Task<Node?> GetNodeByIdAsync(uint nodeId);
		Task EditNodeNameAndDescriptionAsync(Node editingNode, string newName, string newDescription);
		Task AddAsync(Node node);
		Task<List<Node>> GetNodesByOwnerAndParentAsync(uint ownerId, uint? parentId);
		Task RemoveRangeAsync(IEnumerable<Node> nodes);
		Task RemoveAsync(Node node);
	}
}
