namespace Garage.Bot.Data
{
    internal class User
    {

        private List<Vehicle> _userVehicleList = new();

        internal string? Name { get; }
        internal Guid Id { get; set; }
        internal DateTime RegisteredAt { get; set; }

        internal int? VehicleCountLimit { get; set; }
        internal int? VehicleNameLimit { get; set; }

        internal User(string Name)
        {
            this.Name = Name;
            Id = Guid.NewGuid();
            RegisteredAt = DateTime.Now;
        }

        internal void AddVehicle(User _user, string _vehicleName)
        {
            Vehicle _userVehicle = new Vehicle(_user, _vehicleName);
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
