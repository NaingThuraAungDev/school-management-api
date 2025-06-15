using UserRegistration.Application.Contracts;
using UserRegistration.Infrastructure.Data;

namespace UserRegistration.Infrastructure.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly UserRegistrationDBContext _context;
        public IUserRepository User { get; private set; }

        public RepositoryWrapper(UserRegistrationDBContext context)
        {
            _context = context;
            User = new UserRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}