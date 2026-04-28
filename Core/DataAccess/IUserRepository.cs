using Garage.Bot.Core.Data;

namespace Garage.Bot.Core.DataAccess
{
    internal interface IUserRepository
    {
        GarageUser? GetUser(Guid userId);
        GarageUser? GetUserByTelegramUserId(long telegramUserId);
        void Add(GarageUser user);
    }
}
