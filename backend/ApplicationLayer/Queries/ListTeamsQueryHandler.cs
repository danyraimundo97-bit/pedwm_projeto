using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Repositories;

namespace ApplicationLayer.Queries
{
    public class ListTeamsQueryHandler
    {
        private readonly ITeamRepository _repository;
        private readonly Mapper _mapper;

        public ListTeamsQueryHandler(ITeamRepository repository, Mapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<TeamSender>> HandleAsync()
        {
            var entities = await _repository.GetPagedAsync(1, 100);
            return entities.Select(_mapper.ToTeamSender).ToList();
        }
    }
}