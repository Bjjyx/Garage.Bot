using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Bot.CustomExceptions
{
    internal class DuplicateVehicleException : Exception
    {
        private string _vehicleNameMessage;
        public override string Message => _vehicleNameMessage ?? base.Message;
        public DuplicateVehicleException(string VehicleName)
        {
            _vehicleNameMessage = $"Транспорт ‘{VehicleName}’ уже существует";
        }
    }
}
