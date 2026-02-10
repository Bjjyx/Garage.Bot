using System.Xml.Linq;

namespace Garage.Bot
{
    internal class User
    {

        private List<Vehicle> _userVehicleList = new();

        internal string? Name { get; set; }
        internal int? VehicleCountLimit { get; set; }
        internal int? VehicleNameLimit { get; set; }

        internal void AddVehicle(string _vehicleName)
        {
            Vehicle _userVehicle = new Vehicle();
            _userVehicle.Name = _vehicleName;
            _userVehicleList.Add(_userVehicle);
        }

        internal int GetVehicleCount()
        {
            return _userVehicleList.Count;
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
