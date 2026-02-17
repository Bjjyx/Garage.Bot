using Garage.Bot.CustomExceptions;
using Garage.Bot.Data;

namespace Garage.Bot
{
    internal class UserManager
    {
        private User _user;

        //Добавление имени пользователя
        internal bool AddUser(string UserName)
        {
            _user = new User(UserName);
            return true;
        }

        //Получение имени пользователя
        internal string GetUserName()
        {
            return _user.Name;
        }

        //Получение экземпляра пользователя
        internal User? GetUser()
        {
            return _user;
        }

        //Добавление траспорта
        //На данном этапе, название транспорта должно быть уникальным значением
        internal void AddUserVehicle(string UserVehicleName)
        {
            IsUserVehicleCountIn();
            IsUserVehicleNameInLimit(UserVehicleName);
            IsVehicleDuplicate(UserVehicleName);
            _user.AddVehicle(_user, UserVehicleName);
        }

        //Получение списка с транспортом
        internal List<Vehicle> GetUserVehicleList()
        {
            return _user.GetVehicleList();
        }

        //Получение только активного транспорта
        internal List<Vehicle> GetUserActiveVehicleList()
        {
            var _activeVehicleList = new List<Vehicle>();
            foreach (var vehicle in _user.GetVehicleList())
            {
                if (vehicle.State == VehicleState.Active)
                {
                    _activeVehicleList.Add(vehicle);
                }
            }
            return _activeVehicleList;
        }

        //Переводит статус выбранного транспортного средства из Active в Service
        internal bool MoveToService(string Id)
        {
            foreach (var vehicle in _user.GetVehicleList())
            {
                if (Id.Equals(vehicle.Id.ToString()))
                {
                    vehicle.State = VehicleState.Service;
                    vehicle.StateChangedAt = DateTime.Now;
                    return true;
                }
            }
            return false;
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

        //Установка лимита длинны названия транспорта
        internal void SetUserVehicleNameLimit(int NameLimit)
        {
            _user.VehicleNameLimit = NameLimit;
        }

        //Проверка, что лимит на длинну имени не установлен
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

        //Проверка, что лимит транспортных средств не установлен
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

        //Проверка, что название проходит по лимиту символов
        private void IsUserVehicleNameInLimit(string VehicleName)
        {
            if (_user.VehicleNameLimit < VehicleName.Length)
            {
                throw new VehicleNameLengthLimitException(VehicleName.Length, _user.VehicleNameLimit);
            }
        }

        //Проверка, что не привышенно количество транспорта
        private void IsUserVehicleCountIn() 
        {
            if (_user.GetVehicleCount() + 1 > _user.VehicleCountLimit)
            {
                throw new VehicleCountLimitException(_user.VehicleCountLimit);
            }
        }

        //Проверка уникальности названия транспорта
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
