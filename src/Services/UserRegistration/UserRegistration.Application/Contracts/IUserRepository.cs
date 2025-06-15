using UserRegistration.Domain.Entities;

namespace UserRegistration.Application.Contracts
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        UserEntity? FindByPhone(string phone);
        Task<long> Register(string name, string phone, string password, string salt);
        Task<long> UpdateUser(UserEntity user);
        Task<long> CreateUser(UserDto obj);
        Task<long> UpdateUser(long id, UserDto obj);
        Task UpdateUserData(UserEntity obj);
        Task<long> DeleteUser(long id);
        Task<long> BanUser(long id);
        Task<long> UnBanUser(long id);
        UserEntity? GetUserById(long id);
        (List<UserEntity>, int) GetUserList(FilterDto filterModel);
    }
}