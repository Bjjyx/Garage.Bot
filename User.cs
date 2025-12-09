using System.Xml.Linq;

namespace Garage.Bot
{
    internal class User
    {
        private List<Vehicle> _userVehicleList = new();

        internal string? Name { get; set; }

        internal void AddVehicle(string _vehicleName)
        {
            Vehicle _userVehicle = new Vehicle(_vehicleName);
            _userVehicleList.Add(_userVehicle);
        }

        internal List<Vehicle> GetVehicleList()
        {
            return _userVehicleList;
        }

        internal void RemoveVehicle(int _number)
        {
            _userVehicleList.RemoveAt(_number);
        }
    }
}
