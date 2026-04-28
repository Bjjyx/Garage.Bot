namespace Garage.Bot
{
    internal interface IVehicleReportService
    {
        (int total, int service, int active, DateTime generatedAt) GetUserStats(Guid userId, IVehicleManager vehicleManager);
    }
}
