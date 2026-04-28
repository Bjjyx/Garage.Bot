using Garage.Bot.Core.Data;

namespace Garage.Bot.Core.Managers
{
    internal interface IUserManager
    {
        GarageUser RegisterUser(long telegramUserId, string telegramUserName);
        GarageUser? GetUser(long telegramUserId);
    }
}
