using System;
using System.Threading.Tasks;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Repositories;

namespace ApplicationLayer.Queries
{
    public class GetProjectByIdQueryHandler
    {
        private readonly IProjectRepository _repository;
        private readonly Mapper _mapper;

        public GetProjectByIdQueryHandler(IProjectRepository repository, Mapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProjectSender> HandleAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null; // Devolvemos null e o GraphQL lança o erro

            return _mapper.ToProjectSender(entity);
        }
    }
}