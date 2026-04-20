using Garage.Bot.CustomExceptions;
using Garage.Bot.Data;
using Otus.ToDoList.ConsoleBot;
using Otus.ToDoList.ConsoleBot.Types;
using System.Text;

namespace Garage.Bot
{
    internal class UpdateHandler : IUpdateHandler

    {
        private IUserManager _userManager;
        private IVehicleManager _vehicleManager;
        private List<String> _textList = new();
        internal UpdateHandler(IUserManager userManager, IVehicleManager vehicleManager)
        {
            _userManager = userManager;
            _vehicleManager = vehicleManager;
        }
        private readonly string[] _commands = { "/start", "/addVehicle", "/ActiveVehicles", "/AllVehicles", "/moveToService", "/removeVehicle", "/help", "/info"};
        private MenuState _menuSate = MenuState.Main;

        public void HandleUpdateAsync(ITelegramBotClient botClient, Update update)
        {

            try
            {
                switch (_menuSate)
                {
                    case MenuState.Main:
                        MainMenu(botClient, update);
                        break;
                    case MenuState.CountLimit:
                        setCountLimit(botClient, update);
                        break;
                    case MenuState.NameLimit:
                        setNameLimit(botClient, update);
                        break;
                }
            }
            catch (ArgumentException argEx)
            {
                botClient.SendMessage(update.Message.Chat, "Произошла непредвиденная ошибка:\n" + 
                    argEx.GetType() + "\n" + argEx.Message + "\n" + argEx.StackTrace + "\n" + argEx.InnerException);
            }
            catch (VehicleCountLimitException countEx)
            {
                botClient.SendMessage(update.Message.Chat, countEx.Message);
            }
            catch (VehicleNameLengthLimitException lenghtEx)
            {
                botClient.SendMessage(update.Message.Chat, lenghtEx.Message);
            }
            catch (DuplicateVehicleException duplicateEx)
            {
                botClient.SendMessage(update.Message.Chat, duplicateEx.Message);
            }
            catch (Exception ex)
            {
                botClient.SendMessage(update.Message.Chat, "Произошла непредвиденная ошибка:\n" + 
                    ex.GetType() + "\n" + ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
            }
        }

        private void MainMenu(ITelegramBotClient botClient, Update update)
        {
            _textList.AddRange(update.Message.Text.Trim().Split());
            switch (_textList[0])
            {
                case "/start":
                    Start(botClient, update);
                    break;
                case "/help":
                    Help(botClient, update);
                    break;
                case "/info":
                    Info(botClient, update);
                    break;
                case "/addvehicle" when IsRegistered(update):
                    _textList.RemoveAt(0);
                    AddVehicle(botClient, update, string.Join(" ", _textList));
                    break;
                case "/activevehicles" when IsRegistered(update):
                    ActiveVehicles(botClient, update);
                    break;
                case "/allvehicles" when IsRegistered(update):
                    AllVehicles(botClient, update);
                    break;
                case "/movetoservice" when IsRegistered(update):
                    _textList.RemoveAt(0);
                    MoveToService(botClient, update, Guid.Parse(_textList[0]));
                    break;
                case "/removevehicle" when IsRegistered(update):
                    _textList.RemoveAt(0);
                    DeleteVehicle(botClient, update, Guid.Parse(_textList[0]));
                    break;
                default:
                    if (IsRegistered(update))
                    {
                        Console.WriteLine("Введена неизвестная комманда! Попробуйте ещё раз :(");
                    }
                    else
                    {
                        Console.WriteLine("Вы не зарегистрированы :(");
                    }
                    break;
            }
            _textList.Clear();
        }

        private void Start(ITelegramBotClient botClient, Update update)
        {
            if (!IsRegistered(update))
            {
                _userManager.RegisterUser(update.Message.From.Id, update.Message.From.Username);
                botClient.SendMessage(update.Message.Chat, "Введите лимит транспорта");
                _menuSate = MenuState.CountLimit;
            }
            else
            {
                botClient.SendMessage(update.Message.Chat, string.Join("\n", _commands));
                _menuSate = MenuState.Main;
            }
        }

        private bool IsRegistered(Update update)
        {
            return _userManager.GetUser(update.Message.From.Id) != null;
        }

        private void Help(ITelegramBotClient botClient, Update update)
        {
            botClient.SendMessage(update.Message.Chat, "Доступные команды:" +
                "\nКоманда /start -  главное меню и регистрация пользователя" +
                "\nКоманда /help - данное меню с пояснениями :)" +
                "\nКоманда /info - выводит на экран дату создания программы и её версию" +
                "\nКоманда /addVehicle - добавляет транспорт в ваш Garage" +
                "\nКоманда /showActiveVehicles - позволяет посмотреть активные транспортные средства у вас в Гараже" +
                "\nКоманда /showAllVehicles - позволяет посмотреть все транспортные средства у вас в Гараже" +
                "\nКоманда /moveToService - изменяет статус выбранного транспортного средства, переводя его в сервис. Вместе с командой необходимо передать Id транспортного средства" +
                "\nКоманда /removeVehicle - позволяет убрать транспорт из гаража");
        }

        private void Info(ITelegramBotClient botClient, Update update)
        {
            botClient.SendMessage(update.Message.Chat, $"Garage.Bot\nv0.6.0\nДата создания: {File.GetCreationTimeUtc(System.Reflection.Assembly.GetExecutingAssembly().Location)}");
        }

        // Добавляет транспорт в список транспорта пользователя
        private void AddVehicle(ITelegramBotClient botClient, Update update, String name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                botClient.SendMessage(update.Message.Chat, "Название не может быть пустым");
            }
            else
            {
                _vehicleManager.Add(_userManager.GetUser(update.Message.From.Id), name);
                botClient.SendMessage(update.Message.Chat, $"{name} добавлен в Garage");
            }
        }

        private void AllVehicles(ITelegramBotClient botClient, Update update)
        {
            StringBuilder _userVehicles = new();
            foreach (var _vehicle in _vehicleManager.GetAllByUserId(_userManager.GetUser(update.Message.From.Id).Id))
            {
                _userVehicles.AppendLine($"{_vehicle.Name}");
            }
            if (_userVehicles.Length == 0)
            {
                botClient.SendMessage(update.Message.Chat, "Ваш гараж пока пут :(");
            } else
            {
                botClient.SendMessage(update.Message.Chat, $"В вашем гараже:\n{_userVehicles}");
            }
        }

        private void ActiveVehicles(ITelegramBotClient botClient, Update update)
        {
            StringBuilder _userVehicles = new();
            foreach (var _vehicle in _vehicleManager.GetAllByUserId(_userManager.GetUser(update.Message.From.Id).Id))
            {
                _userVehicles.AppendLine($"{_vehicle.Name}");
            }
            if (_userVehicles.Length == 0)
            {
                botClient.SendMessage(update.Message.Chat, "Ваш гараж пока пут :(");
            }
            else
            {
                botClient.SendMessage(update.Message.Chat, $"В вашем гараже активны:\n{_userVehicles}");
            }
        }

        private void MoveToService(ITelegramBotClient botClient, Update update, Guid Id)
        {
            Vehicle? _vehicle = null;
            List<Vehicle> _vehicles = _vehicleManager.GetActiveByUserId(_userManager.GetUser(update.Message.From.Id).Id).ToList();
            _vehicles.ForEach(vehicle =>
            {
                if (vehicle.Id.Equals(Id))
                {
                    _vehicle = vehicle;
                }
            });

            if (_vehicle == null)
            {
                botClient.SendMessage(update.Message.Chat, "Транспорт не найден");
            } else
            {
                _vehicleManager.MoveToService(Id);
                botClient.SendMessage(update.Message.Chat, "Транспорт переведён в сервис");
            }
        }

        private void DeleteVehicle(ITelegramBotClient botClient, Update update, Guid Id)
        {
            Vehicle? _vehicle = null;
            List<Vehicle> _vehicles = _vehicleManager.GetActiveByUserId(_userManager.GetUser(update.Message.From.Id).Id).ToList();
            _vehicles.ForEach(vehicle =>
            {
                if (vehicle.Id.Equals(Id))
                {
                    _vehicle = vehicle;
                }
            });

            if (_vehicle == null)
            {
                botClient.SendMessage(update.Message.Chat, "Транспорт не найден");
            }
            else
            {
                _vehicleManager.Delete(Id);
                botClient.SendMessage(update.Message.Chat, "Транспорт удалён");
            }
        }

        private void setCountLimit(ITelegramBotClient botClient, Update update)
        {
            _userManager.GetUser(update.Message.From.Id).VehicleCountLimit = int.Parse(update.Message.Text);
            botClient.SendMessage(update.Message.Chat, "Введите лимит на название транспорта");
            _menuSate = MenuState.NameLimit;
        }

        private void setNameLimit(ITelegramBotClient botClient, Update update)
        {
            _userManager.GetUser(update.Message.From.Id).VehicleNameLimit = int.Parse(update.Message.Text);
            Start(botClient, update);
        }

    }
}
