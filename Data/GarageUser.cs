namespace Garage.Bot.Data
{
    internal class GarageUser
    {

        private List<Vehicle> _userVehicleList = new();


        internal long TelegramUserId { get; }
        internal string? TelegramUserName { get; }
        internal Guid Id { get; set; }
        internal DateTime RegisteredAt { get; set; }

        internal int? VehicleCountLimit { get; set; }
        internal int? VehicleNameLimit { get; set; }

        internal GarageUser(long telegramUserId, string telegramUserName)
        {
            TelegramUserName = telegramUserName;
            Id = Guid.NewGuid();
            RegisteredAt = DateTime.Now;
            TelegramUserId = telegramUserId;
        }

        internal void AddVehicle(GarageUser _user, string _vehicleName)
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
