using System;
using System.Threading.Tasks;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Repositories;

namespace ApplicationLayer.Queries
{
    public class GetTaskByIdQueryHandler
    {
        private readonly ITaskRepository _repository;
        private readonly Mapper _mapper;

        public GetTaskByIdQueryHandler(ITaskRepository repository, Mapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TaskSender> HandleAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            return _mapper.ToTaskSender(entity);
        }
    }
}