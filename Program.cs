using Garage.Bot;
using Otus.ToDoList.ConsoleBot;

internal class Program
{
    private static ConsoleBotClient botClient = new();
    private static UserManager _userManager = new(new InMemoryUserRepository());
    private static VehicleManager _vehicleManager = new(new InMemoryVehicleRepository());
    private static VehicleReportService _vehicleReportService = new();
    private static void Main(string[] args)
    {
        UpdateHandler updateHandler = new UpdateHandler(_userManager, _vehicleManager, _vehicleReportService);
        botClient.StartReceiving(updateHandler);
    }
}