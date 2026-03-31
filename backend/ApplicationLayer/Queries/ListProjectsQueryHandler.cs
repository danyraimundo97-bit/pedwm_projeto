using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Repositories;

namespace ApplicationLayer.Queries
{
    /// <summary>Read-side use case: list projects as application DTOs (no persistence or mapping details here).</summary>
    public class ListProjectsQueryHandler
    {
        private readonly IProjectRepository _repository;
        private readonly Mapper _mapper;

        public ListProjectsQueryHandler(IProjectRepository repository, Mapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ProjectSender>> HandleAsync()
        {
            
            var entities = await _repository.GetAllAsync();
            return entities.Select(_mapper.ToProjectSender).ToList();
        }
    }
}
