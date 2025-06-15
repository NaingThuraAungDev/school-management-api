using Shared.Encyptions;
using Shared.Extensions;
using Shared.Models;
using UserRegistration.Application.Contracts;
using UserRegistration.Domain.Entities;
using UserRegistration.Infrastructure.Data;

namespace UserRegistration.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        private readonly UserRegistrationDBContext _context;
        public UserRepository(UserRegistrationDBContext context) : base(context)
        {
            _context = context;
        }

        public UserEntity? FindByPhone(string phone)
        {
            var userObj = _context.User.Where(x => x.phone == phone).SingleOrDefault();
            return userObj;
        }

        public async Task<long> Register(string name, string phone, string password, string salt)
        {
            UserEntity user = new UserEntity()
            {
                Name = name,
                phone = phone,
                password = password,
                salt = salt,
                balance = 0,
                is_active = true,
                createddate = System.DateTime.UtcNow,
                lastmodifieddate = System.DateTime.UtcNow
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user.id;
        }

        public async Task<long> UpdateUser(UserEntity user)
        {
            _context.User.Update(user);
            await _context.SaveChangesAsync();
            return user.id;
        }

        public async Task<long> CreateUser(UserDto obj)
        {
            string salt = SaltedHash.GenerateSalt();
            string password = SaltedHash.ComputeHash(salt, obj.password.ToString());

            UserEntity user = new UserEntity
            {
                Name = obj.name,
                phone = obj.phone,
                createddate = DateTime.UtcNow,
                lastmodifieddate = DateTime.UtcNow,
                password = password,
                salt = salt,
                is_active = true
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user.id;
        }

        public async Task<long> UpdateUser(long id, UserDto obj)
        {
            UserEntity? user = _context.User.Where(x => x.id == id).FirstOrDefault();
            if (user is not null)
            {
                user.phone = obj.phone;
                user.Name = obj.name;
                user.lastmodifieddate = DateTime.UtcNow;
                _context.User.Update(user);
                await _context.SaveChangesAsync();
                return user.id;
            }
            return 0;
        }

        public async Task UpdateUserData(UserEntity obj)
        {
            _context.User.Update(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<long> DeleteUser(long id)
        {
            UserEntity? user = _context.User.Where(x => x.id == id).FirstOrDefault();
            if (user is not null)
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
                return user.id;
            }
            return 0;
        }

        public async Task<long> BanUser(long id)
        {
            UserEntity? user = _context.User.Where(x => x.id == id).FirstOrDefault();
            if (user is not null)
            {
                user.is_active = false;
                user.lastmodifieddate = DateTime.UtcNow;
                _context.User.Update(user);
                await _context.SaveChangesAsync();
                return user.id;
            }
            return 0;
        }

        public async Task<long> UnBanUser(long id)
        {
            UserEntity? user = _context.User.Where(x => x.id == id).FirstOrDefault();
            if (user is not null)
            {
                user.is_active = true;
                user.lastmodifieddate = DateTime.UtcNow;
                _context.User.Update(user);
                await _context.SaveChangesAsync();
                return user.id;
            }
            return 0;
        }

        public UserEntity? GetUserById(long id)
        {
            return _context.User.Find(id);
        }

        public (List<UserEntity>, int) GetUserList(FilterDto filterModel)
        {
            string? sortField = null;
            string sortBy = "";

            if (filterModel.sort is not null)
            {
                if (filterModel.sort.Count > 0)
                {
                    var sort = filterModel.sort[0];
                    sortBy = sort.dir == null ? sortBy : sort.dir;
                    sortField = sort.field;
                }
            }

            if (sortField == null || sortField == "")
                sortField = "lastmodifieddate";
            if (sortBy == null || sortBy == "")
                sortBy = "desc";

            var mainQuery = (from main in _context.User select main).AsQueryable();

            if (filterModel.filter?.filters != null)
            {
                for (int i = 0; i < filterModel.filter.filters.Count; i++)
                {
                    string filterName = filterModel.filter.filters[i].field;
                    string filterValue = filterModel.filter.filters[i].value;


                    if (filterName == "name")
                    {
                        string name = filterValue;
                        if (!String.IsNullOrEmpty(name))
                        {
                            mainQuery = mainQuery.Where(x => x.Name.Contains(name));
                        }
                    }

                    if (filterName == "phone")
                    {
                        string phone = filterValue;
                        if (!string.IsNullOrEmpty(phone))
                        {
                            mainQuery = mainQuery.Where(x => x.phone != null && x.phone.Contains(phone));
                        }
                    }
                }
            }


            var objSort = new SortModel
            {
                ColId = sortField,
                Sort = sortBy
            };

            var sortList = new List<SortModel>
            {
                objSort
            };
            mainQuery = mainQuery.OrderBy(sortList);
            var objTotal = mainQuery.Count();

            //Pagination
            int currentPage = filterModel.skip;
            int rowsPerPage = filterModel.take;
            mainQuery = mainQuery.Skip(currentPage * rowsPerPage).Take(rowsPerPage);

            return (mainQuery.ToList(), objTotal);
        }
    }
}