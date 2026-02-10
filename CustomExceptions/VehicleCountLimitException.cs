using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Bot.CustomExceptions
{
    internal class VehicleCountLimitException : Exception
    {
        private string _vehicleCountLimitMessage;
        public override string Message => _vehicleCountLimitMessage ?? base.Message;
        public VehicleCountLimitException(int? vehicleCountLimit)
        {
            _vehicleCountLimitMessage = $"Превышено максимальное количество транспорта равное {vehicleCountLimit}";
        }
    }
}
