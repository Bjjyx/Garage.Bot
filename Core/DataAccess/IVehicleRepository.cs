using Garage.Bot.Core.Data;

namespace Garage.Bot.Core.DataAccess
{
    internal interface IVehicleRepository
    {
        IReadOnlyList<Vehicle> GetAllByUserId(Guid userId);
        //Возвращает ToDoItem для UserId со статусом Active
        IReadOnlyList<Vehicle> GetActiveByUserId(Guid userId);
        IReadOnlyList<Vehicle> Find(Guid userId, Func<Vehicle, bool> predicate);
        Vehicle? Get(Guid id);
        void Add(Vehicle vehicle);
        void Update(Vehicle vehicle);
        void Delete(Guid id);
        //Проверяет есть ли задача с таким именем у пользователя
        bool ExistsByName(Guid userId, string name);
        //Возвращает количество активных задач у пользователя
        int CountActive(Guid userId);
    }
}
