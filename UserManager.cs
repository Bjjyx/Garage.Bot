using Garage.Bot.Data;

namespace Garage.Bot
{
    internal class UserManager: IUserManager
    {
        private List<GarageUser> _users = new();
        
        public GarageUser RegisterUser(long telegramUserId, string telegramUserName)
        {
            GarageUser _user = new GarageUser(telegramUserId, telegramUserName);
            _users.Add(_user);
            return _user;
        }

        public GarageUser? GetUser(long telegramUserId)
        {
            GarageUser? _user = null;
            foreach (var user in _users)
            {
                if (user.TelegramUserId == telegramUserId)
                {
                    _user = user;
                    break;
                }
            }
            return _user;
        }
    }
}
