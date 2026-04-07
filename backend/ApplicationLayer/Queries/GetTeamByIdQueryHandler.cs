using System;
using System.Threading.Tasks;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Repositories;

namespace ApplicationLayer.Queries
{
    public class GetTeamByIdQueryHandler
    {
        private readonly ITeamRepository _repository;
        private readonly Mapper _mapper;

        public GetTeamByIdQueryHandler(ITeamRepository repository, Mapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TeamResponse> HandleAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            return _mapper.ToTeamResponse(entity);
        }
    }
}