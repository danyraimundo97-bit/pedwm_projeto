using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Repositories;

namespace ApplicationLayer.Queries
{
    public class GetTasksByProjectQueryHandler
    {
        private readonly ITaskRepository _repository;
        private readonly Mapper _mapper;

        public GetTasksByProjectQueryHandler(ITaskRepository repository, Mapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<TaskResponse>> HandleAsync(Guid projectId)
        {
            var entities = await _repository.GetByProjectAsync(projectId);
            return entities.Select(_mapper.ToTaskResponse).ToList();
        }
    }
}