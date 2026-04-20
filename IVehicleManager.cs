using Garage.Bot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Bot
{
    internal interface IVehicleManager
    {
        IReadOnlyList<Vehicle> GetAllByUserId(Guid userId);
        //Возвращает ToDoItem для UserId со статусом Active
        IReadOnlyList<Vehicle> GetActiveByUserId(Guid userId);
        Vehicle Add(GarageUser user, string name);
        void MoveToService(Guid id);
        void Delete(Guid id);
    }
}
