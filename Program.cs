using Garage.Bot;
using Otus.ToDoList.ConsoleBot;

internal class Program
{
    private static ConsoleBotClient botClient = new();
    private static UserManager _userManager = new();
    private static VehicleManager _vehicleManager = new();
    private static void Main(string[] args)
    {
        UpdateHandler updateHandler = new UpdateHandler(_userManager, _vehicleManager);
        botClient.StartReceiving(updateHandler);
    }
}