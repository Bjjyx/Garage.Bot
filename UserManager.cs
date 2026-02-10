using System.Text;
using Garage.Bot.CustomExceptions;

namespace Garage.Bot
{
    internal class UserManager
    {
        private User _user = new();

        //Добавление имени пользователя
        internal bool AddUserName(string? UserName)
        {
            _user.Name = UserName;
            return true;
        }

        //Получение имени пользователя
        internal string GetUserName()
        {
            if (string.IsNullOrEmpty(_user.Name))
            {
                return "";
            }
            else
            {
                return _user.Name;
            }
        }

        //Добавление траспорта
        //На данном этапе, название транспорта должно быть уникальным значением
        internal void AddUserVehicle(string UserVehicleName)
        {
            IsUserVehicleCountIn();
            IsUserVehicleNameInLimit(UserVehicleName);
            IsVehicleDuplicate(UserVehicleName);
            _user.AddVehicle(UserVehicleName);
        }

        //Получение списка с транспортом
        internal List<Vehicle> GetUserVehicleList()
        {
            return _user.GetVehicleList();
        }

        //Удаление тс пользователя
        //Если тс не найден возвращает false 
        internal bool DeleteUserVehicle(int VehicleNumber)
        {
            if (_user.GetVehicleCount() > VehicleNumber && VehicleNumber >= 0)
            {
                _user.RemoveVehicle(VehicleNumber);
                return true;
            }
            else
            {
                return false;
            }
        }

        //Установка лимита транспорта
        internal void SetUserVehicleCountLimit(int VehicleCount)
        {
            _user.VehicleCountLimit = VehicleCount;
        }

        internal void SetUserVehicleNameLimit(int NameLimit)
        {
            _user.VehicleNameLimit = NameLimit;
        }

        internal bool IsUserVehicleNameLimitNotSet()
        {
            if (_user.VehicleNameLimit == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool IsUserVehicleCountLimitNotSet()
        {
            if (_user.VehicleCountLimit == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void IsUserVehicleNameInLimit(string VehicleName)
        {
            if (_user.VehicleNameLimit < VehicleName.Length)
            {
                throw new VehicleNameLengthLimitException(VehicleName.Length, _user.VehicleNameLimit);
            }
        }

        private void IsUserVehicleCountIn() 
        {
            if (_user.GetVehicleCount() + 1 > _user.VehicleCountLimit)
            {
                throw new VehicleCountLimitException(_user.VehicleCountLimit);
            }
        }

        private void IsVehicleDuplicate(string VehicleName)
        {
            _user.GetVehicleList().ForEach(vehicle =>
            {
                if (vehicle.Name.Equals(VehicleName, StringComparison.OrdinalIgnoreCase))
                {
                    throw new DuplicateVehicleException(VehicleName);
                }
            });

        }
    }
}
