using Garage.Bot.Core.Data;
using Garage.Bot.Core.DataAccess;

namespace Garage.Bot.Core.Managers
{
    internal class UserManager: IUserManager
    {
        private IUserRepository _userRepository;
        public UserManager(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        
        public GarageUser RegisterUser(long telegramUserId, string telegramUserName)
        {
            GarageUser _user = new GarageUser(telegramUserId, telegramUserName);
            _userRepository.Add(_user);
            return _user;
        }

        public GarageUser? GetUser(long telegramUserId)
        {
            return _userRepository.GetUserByTelegramUserId(telegramUserId);
        }
    }
}
