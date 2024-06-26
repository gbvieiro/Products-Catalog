using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Domain.Entities.Users;
using Products.Catalog.Domain.RepositoriesInterfaces;

namespace Products.Catalog.Application.Services.Users
{
    public class UsersAppService(IUsersRepository usersRepository, IMapper mapper) : IUsersAppService
    {
        private readonly IUsersRepository _usersRepository = usersRepository;

        private readonly IMapper _mapper = mapper;

        public Task DeleteAsync(Guid id) => _usersRepository.DeleteAsync(id);

        public async Task<List<UserDto>> GetAllAsync(string filtertext, int skip, int take)
        {
            var users = await _usersRepository.GetAllAsync(filtertext, skip, take);
            return _mapper.Map<List<UserDto>>(users.ToList());
        }

        public async Task<UserDto?> GetAsync(Guid id)
        {
            var user = await _usersRepository.GetAsync(id);
            return user != null ? _mapper.Map<UserDto>(user) : default;
        }

        public Task SaveAsync(UserDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            dto.GenerateId();
            var user = _mapper.Map<User>(dto);
            return _usersRepository.SaveAsync(user);
        }
    }
}