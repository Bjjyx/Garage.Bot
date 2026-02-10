using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Bot.CustomExceptions
{
    internal class VehicleNameLengthLimitException : Exception
    {
        private string _vehicleNameLimitMessage;
        public override string Message => _vehicleNameLimitMessage ?? base.Message;
        public VehicleNameLengthLimitException(int? vehicleNameLengh ,int? vehicleNameLimit)
        {
            _vehicleNameLimitMessage = $"Длина названия траспорта {vehicleNameLengh} превышает максимально допустимое значение {vehicleNameLimit}";
        }
    }
}
