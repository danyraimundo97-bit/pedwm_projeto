using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationLayer.Queries
{
    public sealed class ListUsersQueryHandler
    {
        private const int ListPageSize = 10_000;

        private readonly IUserRepository _repository;
        private readonly Mapper _mapper;

        public ListUsersQueryHandler(IUserRepository repository, Mapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<UserResponse>> HandleAsync()
        {
            var entities = await _repository.GetPagedAsync(page: 1, size: ListPageSize);
            return entities.Select(_mapper.ToUserResponse).ToList();
        }

    }
}
