using System.Collections.Generic;
using BusinessEntities;

namespace Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> Get(UserTypes? userType = null, string name = null, string email = null);
        void DeleteAll();
    }
}