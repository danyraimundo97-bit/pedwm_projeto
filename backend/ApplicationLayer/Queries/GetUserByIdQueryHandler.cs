using System;
using System.Threading.Tasks;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Repositories;

namespace ApplicationLayer.Queries
{
    public class GetUserByIdQueryHandler
    {
        private readonly IUserRepository _repository;
        private readonly Mapper _mapper;

        public GetUserByIdQueryHandler(IUserRepository repository, Mapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserResponse> HandleAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            return _mapper.ToUserSender(entity);
        }
    }
}