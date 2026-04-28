using Garage.Bot.Core.CustomExceptions;
using Garage.Bot.Core.Data;
using Garage.Bot.Core.DataAccess;
using System.Collections.Immutable;

namespace Garage.Bot.Infrastructure.DataAccess
{
    internal class InMemoryVehicleRepository : IVehicleRepository
    {
        private List<Vehicle> _vehicleList = new();

        public void Add(Vehicle vehicle)
        {
            IsNameDuplicate(vehicle);

            _vehicleList.Add(vehicle);
        }

        public int CountActive(Guid userId)
        {
            int countActive = 0;

            _vehicleList.ForEach(vehicle =>
            {
                if (vehicle.User.Id.Equals(userId))
                {
                    countActive++;
                }
            });
            return countActive;
        }

        public void Delete(Guid id)
        {
            _vehicleList.ForEach((vehicle) =>
            {
                if (vehicle.Id.Equals(id))
                {
                    _vehicleList.Remove(vehicle);
                }
            });
        }

        public bool ExistsByName(Guid userId, string name)
        {
            bool _exist = false;
            _vehicleList.ForEach(vehicle =>
            {
                if(vehicle.User.Id.Equals(userId) && vehicle.Name.Equals(name))
                {
                    _exist = true;
                }
            });
            return _exist;
        }

        public IReadOnlyList<Vehicle> Find(Guid userId, Func<Vehicle, bool> predicate)
        {
            List<Vehicle> vehicles = GetAllByUserId(userId).ToList();
            return vehicles.Where(predicate).ToImmutableList();
        }

        public Vehicle? Get(Guid id)
        {
            Vehicle? _vehicle = null;
            _vehicleList.ForEach(vehicle =>
            {
                if (vehicle.Id.Equals(id))
                {
                    _vehicle = vehicle;
                }
            });
            return _vehicle;
        }

        public IReadOnlyList<Vehicle> GetActiveByUserId(Guid userId)
        {
            List<Vehicle> activeVehicles = new();
            _vehicleList.ForEach(vehicle =>
            {
                if (vehicle.User.Id.Equals(userId) && vehicle.State.Equals(VehicleState.Active))
                {
                    activeVehicles.Add(vehicle);
                }
            });
            return activeVehicles.ToImmutableList();
        }

        public IReadOnlyList<Vehicle> GetAllByUserId(Guid userId)
        {
            List<Vehicle> userVehicles = new();
            _vehicleList.ForEach(vehicle =>
            {
                if (vehicle.User.Id.Equals(userId))
                {
                    userVehicles.Add(vehicle);
                }
            });
            return userVehicles.ToImmutableList();
        }

        public void Update(Vehicle vehicle)
        {
            _vehicleList.ForEach(_vehicle =>
            {
                if (_vehicle.Id.Equals(vehicle.Id))
                {
                    if (_vehicle.State.Equals(VehicleState.Active))
                    {
                        _vehicle.State = VehicleState.Service;
                    } else
                    {
                        _vehicle.State = VehicleState.Active;
                    }
                }
            });
        }

        private void IsNameDuplicate(Vehicle vehicle)
        {
            _vehicleList.ForEach(_vehicle =>
            {
                if(_vehicle.User.Id.Equals(vehicle.User.Id) && _vehicle.Name.Equals(vehicle.Name))
                {
                    throw new DuplicateVehicleException(vehicle.Name);
                }
            });
        }
    }
}
