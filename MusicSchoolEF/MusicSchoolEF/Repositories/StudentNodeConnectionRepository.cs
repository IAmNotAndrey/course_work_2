using Microsoft.EntityFrameworkCore;
using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.ViewModels;
using MusicSchoolEF.Repositories.Interfaces;

namespace MusicSchoolEF.Repositories
{
	public class StudentNodeConnectionRepository : IStudentNodeConnectionRepository
	{
		private readonly Ms2Context _dbContext;

		public StudentNodeConnectionRepository(Ms2Context dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<StudentNodeConnection>> GetStudentNodeConnectionsByStudentIdAsync(uint studentId)
		{
			var allStudentTasks = await _dbContext.StudentNodeConnections
				//.Include(snc => snc.NodeNavigation)
				//.Include(snc => snc.NodeNavigation.InverseParentNavigation) // Подключение потомков
				//.Include(snc => snc.NodeNavigation.OwnerNavigation)
				.Where(snc => snc.Student == studentId)
				.ToListAsync();

			return allStudentTasks;
		}

		public async Task<List<StudentNodeConnection>> GetStudentNodeConnectionsByStudentIdAndTeacherIdAsync(uint studentId, uint teacherId)
		{
			return await _dbContext.StudentNodeConnections
				//.Include(snc => snc.NodeNavigation)
				.Where(snc => snc.Student == studentId
							&& snc.NodeNavigation.Owner == teacherId)
				.ToListAsync();
		}

		public async Task<StudentNodeConnection?> GetStudentNodeConnectionByPrimaryKey(uint studentId, uint nodeId)
		{
			return await _dbContext.StudentNodeConnections
				//.Include(snc => snc.NodeNavigation)
				.SingleOrDefaultAsync(snc => snc.Student == studentId && snc.Node == nodeId);
		}

		public async Task EditAsync(StudentNodeConnection editingSnc, int? mark, string? comment)
		{
			editingSnc.Mark = mark;
			editingSnc.Comment = comment;
			await _dbContext.SaveChangesAsync();
		}

		public async Task AddAsync(StudentNodeConnection studentNodeConnection)
		{
			await _dbContext.StudentNodeConnections.AddAsync(studentNodeConnection);
			await _dbContext.SaveChangesAsync();
		}

		public async Task RemoveAsync(StudentNodeConnection studentNodeConnection)
		{
			_dbContext.StudentNodeConnections.Remove(studentNodeConnection);
			await _dbContext.SaveChangesAsync();
		}

		/// <summary>
		/// Обновляет таблицу `StudentNodeConnection` по `IsChecked` у `students`
		/// </summary>
		public async Task UpdateTable(List<StudentCheckBoxViewModel> students, List<Node> nodeAndDescendants)
		{
			foreach (var student in students)
			{
				// Пытаемся подписываем на все них выбранных студентов
				foreach (var node in nodeAndDescendants)
				{
					bool isInDb = await GetStudentNodeConnectionByPrimaryKey(student.Id, node.Id) != null;

					if (student.IsChecked)
					{
						// Если ещё не добавлено, то добавляем
						if (!isInDb)
						{
							await AddAsync(
								new StudentNodeConnection()
								{
									Node = node.Id,
									Student = student.Id
								});
						}
					}
					// Пытаемся отписать не выбранных студентов
					else
					{
						// Если уже есть, то удаляем
						if (isInDb)
						{
							await RemoveAsync(
								await GetStudentNodeConnectionByPrimaryKey(student.Id, node.Id)
									?? throw new NullReferenceException()
							);
						}
					}
				}
			}
		}
	}
}
