using Garage.Bot.Data;

namespace Garage.Bot
{
    internal class InMemoryUserRepository : IUserRepository
    {
        private List<GarageUser> _users = new();
        public void Add(GarageUser user)
        {
            _users.Add(user);
        }

        public GarageUser? GetUser(Guid userId)
        {
            GarageUser? _user = null;
            _users.ForEach(user =>
            {
                if (user.Id.Equals(userId))
                {
                    _user = user;
                }
            });
            return _user;
        }

        public GarageUser? GetUserByTelegramUserId(long telegramUserId)
        {
            GarageUser? _user = null;
            _users.ForEach(user =>
            {
                if (user.TelegramUserId == telegramUserId)
                {
                    _user = user;
                }
            });
            return _user;
        }
    }
}
