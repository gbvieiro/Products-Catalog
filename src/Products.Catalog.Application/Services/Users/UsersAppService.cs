using AutoMapper;
using Products.Catalog.Application.DTOs;
using Products.Catalog.Domain.Entities.Stocks;
using Products.Catalog.Domain.Entities.Users;
using Products.Catalog.Domain.RepositoriesInterfaces;

namespace Products.Catalog.Application.Services.Users
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="usersRepository">A user repository interface.</param>
    /// <param name="mapper">A mapper service.</param>
    public class UsersAppService(IUsersRepository usersRepository, IMapper mapper) : IUsersAppService
    {
        /// <summary>
        /// A order repository interface.
        /// </summary>
        private readonly IUsersRepository _usersRepository = usersRepository;

        /// <summary>
        /// A mapper service.
        /// </summary>
        private readonly IMapper _mapper = mapper;

        /// <inheritdoc/>
        public Task DeleteAsync(Guid id) => _usersRepository.DeleteAsync(id);

        /// <inheritdoc/>
        public async Task<List<UserDto>> GetAllAsync(string filtertext, int skip, int take)
        {
            var users = await _usersRepository.GetAllAsync(filtertext, skip, take);
            return _mapper.Map<List<UserDto>>(users.ToList());
        }

        /// <inheritdoc/>
        public async Task<UserDto?> GetAsync(Guid id)
        {
            var user = await _usersRepository.GetAsync(id);
            return user != null ? _mapper.Map<UserDto>(user) : default;
        }

        /// <inheritdoc/>
        public Task SaveAsync(UserDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            dto.GenerateId();
            var user = _mapper.Map<User>(dto);
            return _usersRepository.SaveAsync(user);
        }
    }
}