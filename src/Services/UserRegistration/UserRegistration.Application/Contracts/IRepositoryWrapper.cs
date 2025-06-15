namespace UserRegistration.Application.Contracts
{
    public interface IRepositoryWrapper : IDisposable
    {
        IUserRepository User { get; }
        Task SaveAsync();
    }
}