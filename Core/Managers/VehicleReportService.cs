using Garage.Bot.Core.Data;

namespace Garage.Bot.Core.Managers
{
    internal class VehicleReportService : IVehicleReportService
    {
        public (int total, int service, int active, DateTime generatedAt) GetUserStats(Guid userId, IVehicleManager vehicleManager)
        {
            int total = 0;
            int service = 0;
            int active = 0;
            List<Vehicle> vehicleList = vehicleManager.GetAllByUserId(userId).ToList();
            vehicleList.ForEach(vehicle =>
            {
                if (vehicle.State.Equals(VehicleState.Active))
                {
                    active++;
                }
                else
                {
                    service++;
                }
                total++;
            });
            return (total, service, active, DateTime.Now);
        }
    }
}
