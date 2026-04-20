using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garage.Bot.Data;

namespace Garage.Bot
{
    internal interface IUserManager
    {
        GarageUser RegisterUser(long telegramUserId, string telegramUserName);
        GarageUser? GetUser(long telegramUserId);
    }
}
