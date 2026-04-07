using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Repositories;

namespace ApplicationLayer.Queries
{
    public class GetTasksByUserQueryHandler
    {
        private readonly ITaskRepository _repository;
        private readonly Mapper _mapper;

        public GetTasksByUserQueryHandler(ITaskRepository repository, Mapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<TaskResponse>> HandleAsync(Guid userId)
        {
            var entities = await _repository.GetByUserAsync(userId);
            return entities.Select(_mapper.ToTaskResponse).ToList();
        }
    }
}