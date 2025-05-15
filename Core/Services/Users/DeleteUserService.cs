using BusinessEntities;
using Common;
using Data.Repositories;

namespace Core.Services.Users
{
    [AutoRegister]
    public class DeleteUserService : IDeleteUserService
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void Delete(User user)
        {
            _userRepository.Delete(user);
        }

        public void DeleteAll()
        {
            _userRepository.DeleteAll();
        }
    }
}