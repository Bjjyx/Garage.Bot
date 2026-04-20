using Garage.Bot.CustomExceptions;
using Garage.Bot.Data;
using System.Collections.Immutable;

namespace Garage.Bot
{
    internal class VehicleManager : IVehicleManager
    {
        private List<Vehicle> _vehicles = new();

        public Vehicle Add(GarageUser user, string name)
        {
            IsUserVehicleCountIn(user);
            IsUserVehicleNameInLimit(user, name);
            IsVehicleDuplicate(user, name);

            Vehicle vehicle = new(user, name);
            _vehicles.Add(vehicle);

            return vehicle;
        }

        public void Delete(Guid id)
        {
            foreach (var vehicle in _vehicles)
            {
                if (vehicle.Id.Equals(id))
                {
                    _vehicles.Remove(vehicle);
                    break;
                }
            }
        }

        public IReadOnlyList<Vehicle> GetActiveByUserId(Guid userId)
        {
            List<Vehicle> userVehicles = new();
            _vehicles.ForEach(vehicle =>
            {
                if (vehicle.User.Id.Equals(userId) && vehicle.State == VehicleState.Active)
                {
                    userVehicles.Add(vehicle);
                }
            });
            return userVehicles.ToImmutableList();
        }

        public IReadOnlyList<Vehicle> GetAllByUserId(Guid userId)
        {
            List<Vehicle> userVehicles = new();
            _vehicles.ForEach(vehicle =>
            {
                if (vehicle.User.Id.Equals(userId))
                {
                    userVehicles.Add(vehicle);
                }
            });
            return userVehicles.ToImmutableList();
        }

        public void MoveToService(Guid id)
        {
            foreach (var vehicle in _vehicles)
            {
                if (vehicle.Id.Equals(id))
                {
                    vehicle.State = VehicleState.Service;
                    break;
                }
            }
        }

        //Проверка, что название проходит по лимиту символов
        private void IsUserVehicleNameInLimit(GarageUser user, string name)
        {
            if (user.VehicleNameLimit < name.Length)
            {
                throw new VehicleNameLengthLimitException(name.Length, user.VehicleNameLimit);
            }
        }

        //Проверка, что не привышенно количество транспорта
        private void IsUserVehicleCountIn(GarageUser user)
        {
            if (user.GetVehicleCount() + 1 > user.VehicleCountLimit)
            {
                throw new VehicleCountLimitException(user.VehicleCountLimit);
            }
        }

        //Проверка уникальности названия транспорта
        private void IsVehicleDuplicate(GarageUser user, string name)
        {
            user.GetVehicleList().ForEach(vehicle =>
            {
                if (vehicle.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    throw new DuplicateVehicleException(name);
                }
            });

        }
    }
}
