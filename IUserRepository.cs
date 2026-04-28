using Garage.Bot.Data;

namespace Garage.Bot
{
    internal interface IUserRepository
    {
        GarageUser? GetUser(Guid userId);
        GarageUser? GetUserByTelegramUserId(long telegramUserId);
        void Add(GarageUser user);
    }
}
