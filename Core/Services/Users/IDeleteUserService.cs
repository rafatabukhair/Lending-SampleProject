using BusinessEntities;

namespace Core.Services.Users
{
    public interface IDeleteUserService
    {
        void Delete(User user);
        void DeleteAll();
    }
}