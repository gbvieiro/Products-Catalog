using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Domain.Entities;
using Products.Catalog.Domain.Interfaces;

namespace Products.Catalog.Application.Services.Users
{
    public class UsersAppService(IRepository<User> usersRepository, IMapper mapper) : IUsersAppService
    {
        private readonly IRepository<User> _usersRepository = usersRepository;

        private readonly IMapper _mapper = mapper;

        public Task DeleteAsync(Guid id) => _usersRepository.DeleteAsync(id);

        public async Task<List<UserDto>> GetAllAsync(string filtertext, int skip, int take)
        {
            var users = await _usersRepository.FindAsync(filtertext, skip, take);
            return _mapper.Map<List<UserDto>>(users.ToList());
        }

        public async Task<UserDto?> GetAsync(Guid id)
        {
            var user = await _usersRepository.ReadAsync(id);
            return user != null ? _mapper.Map<UserDto>(user) : default;
        }

        public async Task SaveAsync(UserDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            dto.GenerateId();
            var user = _mapper.Map<User>(dto);
            
            var existingUser = await _usersRepository.ReadAsync(user.Id);
            if (existingUser == null)
            {
                await _usersRepository.CreateAsync(user);
            }
            else
            {
                await _usersRepository.UpdateAsync(user.Id, user);
            }
        }
    }
}