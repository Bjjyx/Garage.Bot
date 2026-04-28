using Garage.Bot.Core.CustomExceptions;
using Garage.Bot.Core.Data;
using Garage.Bot.Core.DataAccess;

namespace Garage.Bot.Core.Managers
{
    internal class VehicleManager : IVehicleManager
    {
        private IVehicleRepository _vehicleRepository;

        public VehicleManager(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public Vehicle Add(GarageUser user, string name)
        {
            IsUserVehicleCountIn(user);
            IsUserVehicleNameInLimit(user, name);

            Vehicle vehicle = new(user, name);
            _vehicleRepository.Add(vehicle);

            return vehicle;
        }

        public void Delete(Guid id)
        {
            _vehicleRepository.Delete(id);
        }

        public IReadOnlyList<Vehicle> GetActiveByUserId(Guid userId)
        {
            return _vehicleRepository.GetActiveByUserId(userId);
        }

        public IReadOnlyList<Vehicle> GetAllByUserId(Guid userId)
        {
            return _vehicleRepository.GetAllByUserId(userId);
        }

        public void MoveToService(Guid id)
        {
            Vehicle? _vehicle = _vehicleRepository.Get(id);
            if (_vehicle != null)
            {
                _vehicleRepository.Update(_vehicle);
            }
        }

        public IReadOnlyList<Vehicle> Find(GarageUser user, string namePrefix)
        {
            return _vehicleRepository.Find(user.Id, x => x.Name.Contains(namePrefix));
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
    }
}
