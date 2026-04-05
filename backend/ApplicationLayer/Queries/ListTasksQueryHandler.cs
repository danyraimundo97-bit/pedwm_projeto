using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Repositories;

namespace ApplicationLayer.Queries
{
    /// <summary>Read-side: list all tasks as <see cref="TaskResponse"/> DTOs for GraphQL.</summary>
    public sealed class ListTasksQueryHandler
    {
        private const int ListPageSize = 10_000;

        private readonly ITaskRepository _repository;
        private readonly Mapper _mapper;

        public ListTasksQueryHandler(ITaskRepository repository, Mapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<TaskResponse>> HandleAsync()
        {
            var entities = await _repository.GetPagedAsync(page: 1, size: ListPageSize);
            return entities.Select(_mapper.ToTaskResponse).ToList();
        }
    }
}
