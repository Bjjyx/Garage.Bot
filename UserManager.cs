using System.Text;

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
        internal bool AddUserVehicle(string? UserVehicleName)
        {
            _user.AddVehicle(UserVehicleName);
            return true;

        }

        //Получение списка с транспортом
        internal List<Vehicle> GetUserVehicleList()
        {
            return _user.GetVehicleList();
        }

        //Удаление тс пользователя
        //Если не тс не найден возвращает false 
        internal bool DeleteUserVehicle(int VehicleNumber)
        {
            if (_user.GetVehicleList().Count() > VehicleNumber)
            {
                _user.RemoveVehicle(VehicleNumber);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
