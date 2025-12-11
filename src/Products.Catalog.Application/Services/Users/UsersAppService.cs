using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Domain.Entities;
using Products.Catalog.Domain.Interfaces;

namespace Products.Catalog.Application.Services.Users;

public class UsersAppService(IRepository<User> usersRepository, IMapper mapper) : IUsersAppService
{
    private readonly IRepository<User> _usersRepository = usersRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Guid> CreateAsync(UserDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        var user = _mapper.Map<User>(dto);
        await _usersRepository.CreateAsync(user);
        return user.Id;
    }

    public async Task<UserDto?> ReadAsync(Guid id)
    {
        var user = await _usersRepository.ReadAsync(id);
        return user != null ? _mapper.Map<UserDto>(user) : default;
    }

    public async Task UpdateAsync(Guid id, UserDto dto)
    {
        var entity = await _usersRepository.ReadAsync(id);
        if (entity is not null)
        {
            var user = _mapper.Map<User>(dto);
            await _usersRepository.UpdateAsync(id, user);
        }
    }

    public Task DeleteAsync(Guid id) => _usersRepository.DeleteAsync(id);

    public async Task<IReadOnlyCollection<UserDto>> FindAsync(string filtertext)
    {
        var users = await _usersRepository.FindAsync(filtertext, 0, 100);
        return _mapper.Map<List<UserDto>>(users.ToList());
    }
}