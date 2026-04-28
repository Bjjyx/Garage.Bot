using Garage.Bot.Core.Data;

namespace Garage.Bot.Core.Managers
{
    internal interface IVehicleManager
    {
        IReadOnlyList<Vehicle> GetAllByUserId(Guid userId);
        //Возвращает ToDoItem для UserId со статусом Active
        IReadOnlyList<Vehicle> GetActiveByUserId(Guid userId);
        IReadOnlyList<Vehicle> Find(GarageUser user, string namePrefix);
        Vehicle Add(GarageUser user, string name);
        void MoveToService(Guid id);
        void Delete(Guid id);
    }
}
