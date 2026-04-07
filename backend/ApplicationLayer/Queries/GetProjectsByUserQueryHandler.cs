using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Repositories;

namespace ApplicationLayer.Queries
{
    public class GetProjectsByUserQueryHandler
    {
        private readonly IProjectRepository _repository;
        private readonly Mapper _mapper;

        public GetProjectsByUserQueryHandler(IProjectRepository repository, Mapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ProjectResponse>> HandleAsync(Guid userId)
        {
            var entities = await _repository.GetByUserAsync(userId);
            return entities.Select(_mapper.ToProjectResponse).ToList();
        }
    }
}