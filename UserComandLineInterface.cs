using System.Text;
using Garage.Bot.CustomExceptions;

namespace Garage.Bot
{
    // Класс пользователького интерфейса
    internal class UserComandLineInterface
    {
        private string? _userCommand;
        private readonly UserManager _userManager = new();
        private readonly string[] _commands = { "/start", "/addVehicle", "/showActiveVehicles", "/showAllVehicles", "/moveToService", "/removeVehicle", "/help", "/info", "/echo", "/exit" };
        private StringBuilder _userCommandBuilder = new();
        private List<string> _userCommandList = new List<string>();

        //Главное меню, от куда происходят переходы в подменю
        internal void MainMenu()
        {
            do
            {
                try
                {
                    _userCommandBuilder.Clear();
                    _userCommandList.Clear();
                    Console.Clear();

                    HelloMainMenu();

                    _userCommand = Console.ReadLine();

                    ValidateString(_userCommand);

                    _userCommandList.AddRange(_userCommand.Trim().Split());

                    switch (_userCommandList[0].ToLowerInvariant())
                    {
                        case "/start":
                            Console.Clear();
                            if (_userManager.GetUser() == null)
                            {
                                CommandStart();
                            }
                            break;
                        case "/help":
                            Console.Clear();
                            CommandHelp();
                            break;
                        case "/info":
                            Console.Clear();
                            CommandInfo();
                            break;
                        case "/echo":
                            Console.Clear();
                            _userCommandList.RemoveAt(0);
                            CommandEcho(_userCommandList);
                            break;
                        case "/addvehicle" when IsRegistred():
                            Console.Clear();
                            CommandAddVehicle();
                            break;
                        case "/showactivevehicles" when IsRegistred():
                            Console.Clear();
                            CommandShowVehicles(false);
                            break;
                        case "/showallvehicles" when IsRegistred():
                            Console.Clear();
                            CommandShowVehicles(true);
                            break;
                        case "/movetoservice" when IsRegistred():
                            Console.Clear();
                            _userCommandList.RemoveAt(0);
                            CommandMoveToService(_userCommandList);
                            break;
                        case "/removevehicle" when IsRegistred():
                            Console.Clear();
                            CommandRemoveVehicle();
                            break;
                        case "/exit":
                            break;
                        default:
                            Console.Clear();
                            if (IsRegistred())
                            {
                                Console.WriteLine("Введена неизвестная комманда! Попробуйте ещё раз :(");
                            }
                            else
                            {
                                Console.WriteLine("Вы не зарегистрированы :(");
                            }
                                Console.ReadKey(true);
                            break;

                    }
                }
                catch (ArgumentException argEx)
                {
                    Console.WriteLine("Произошла непредвиденная ошибка:\n" + argEx.GetType() + "\n" + argEx.Message + "\n" + argEx.StackTrace + "\n" + argEx.InnerException);
                    Console.ReadKey(true);
                }
                catch (VehicleCountLimitException countEx)
                {
                    Console.WriteLine(countEx.Message);
                    Console.ReadKey(true);

                }
                catch (VehicleNameLengthLimitException lenghtEx)
                {
                    Console.WriteLine(lenghtEx.Message);
                    Console.ReadKey(true);

                }
                catch (DuplicateVehicleException duplicateEx)
                {
                    Console.WriteLine(duplicateEx.Message);
                    Console.ReadKey(true);

                }
                _userCommandList.Insert(0, "");
            } while (!_userCommandList[0].Equals(_commands[9]));


        }

        //Приветствие в главном меню
        private void HelloMainMenu()
        {
            if (_userManager.GetUser() == null)
            {
                Console.WriteLine($"Вас приветствует Garage.Bot!\nВведите команду и нажмите любую клавишу:\n{_userCommandBuilder.AppendJoin("\n", _commands)}");
            }
            else
            {
                Console.WriteLine($"{_userManager.GetUserName()}, с возвращением в Garage.Bot :)\nВведите команду и нажмите любую клавишу:\n{_userCommandBuilder.AppendJoin("\n", _commands)}");
            }
        }

        //Проверка, что ввод пользователя не пустой
        private bool IsInputCorrect(string? _input)
        {
            if (string.IsNullOrWhiteSpace(_input))
            {
                Console.WriteLine("Прерван ввод, либо отправленно пустое поле");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void WriteUserNameAndText(string _text)
        {
            if (_userManager.GetUser() == null)
            {
                Console.WriteLine(_userManager.GetUserName() + _text);
            }
        }

        private bool IsRegistred()
        {
            if (_userManager.GetUser() == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // Комманда старт, для нового пользователя предлагает ввести имя, для пользователя с именем делает ничего
        private void CommandStart()
        {
            Console.WriteLine("Введите ваше имя:");
            _userCommand = Console.ReadLine();

            ValidateString(_userCommand);
            _userManager.AddUser(_userCommand);
        }

        // Описание работы рпограммы, пока не заполнено
        private void CommandHelp()
        {
            WriteUserNameAndText("");
            Console.WriteLine("Доступные команды:" +
                "\nКоманда /start - если пользователь ещё не вводил своё имя, то позволяет это сделать, если пользователь уже вводил своё имя, то возвращает его в главное меню без изменений" +
                "\nКоманда /help - данное меню с пояснениями :)" +
                "\nКоманда /info - выводит на экран дату создания программы и её версию" +
                "\nКоманда /echo - выводит на экран то, что было написано после самой команды, например \"/echo Hello\" выведет на экран \"Hello\"" +
                "\nКоманда /addVehicle - добавляет транспорт в ваш Garage" +
                "\nКоманда /showActiveVehicles - позволяет посмотреть активные транспортные средства у вас в Гараже" +
                "\nКоманда /showAllVehicles - позволяет посмотреть все транспортные средства у вас в Гараже" +
                "\nКоманда /moveToService - изменяет статус выбранного транспортного средства, переводя его в сервис. Вместе с командой необходимо передать Id транспортного средства" +
                "\nКоманда /removeVehicle - позволяет убрать транспорт из гаража" +
                "\nКоманда /exit - выход из программы" +
                "\nПосле каждого вывода на экран, необходимо нажать любую клавишу для продолжения");
            Console.ReadKey(true);
        }

        // Информация о программе, версия и дата создания
        private void CommandInfo()
        {
            WriteUserNameAndText("");
            Console.WriteLine($"Garage.Bot\nv0.5.0\nДата создания: {File.GetCreationTimeUtc(System.Reflection.Assembly.GetExecutingAssembly().Location)}");
            Console.ReadKey(true);
        }

        // Получает на вход текст, который был написан вместе с командой /echo и выводит его на экран
        private void CommandEcho(List<string> _echo)
        {
            if (_echo.Any())
            {
                WriteUserNameAndText(", вы ввели:");
                _userCommandBuilder.AppendJoin(" ", _echo);
                Console.WriteLine(_userCommandBuilder.ToString());                
            } else 
            {
                Console.WriteLine("Не задан текст для передачи");
            }
            _userCommandList.Insert(0, "");
            Console.ReadKey(true);
        }

        // Добавляет транспорт в список транспорта пользователя
        private void CommandAddVehicle()
        {
            if (_userManager.IsUserVehicleCountLimitNotSet())
            {
                SetUserVehicleCountLimit();
            }
            if (_userManager.IsUserVehicleNameLimitNotSet())
            {
                SetUserVehicleNameLimit();
            }

            Console.WriteLine("Введите название транспортного средства:");
            _userCommand = Console.ReadLine();
            if (IsInputCorrect(_userCommand))
            {
                _userManager.AddUserVehicle(_userCommand);
                Console.WriteLine("Транспорт успешно поставлен в Garage :)");
            }     
            Console.ReadKey(true);
        }

        //Добавление лимита на длинну названия траспортного срадства
        private void SetUserVehicleNameLimit()
        {
            Console.WriteLine("Введите ограниение на количество символов в название транспорта от 1 до 100");
            _userCommand = Console.ReadLine();
            _userManager.SetUserVehicleNameLimit(ParseAndValidateInt(_userCommand, 1, 100));
        }

        //Добавление лимита на кол-во траспорта пользователя
        private void SetUserVehicleCountLimit()
        {
            Console.WriteLine("Введите колличество транспорта от 1 до 100");
            _userCommand = Console.ReadLine();

            _userManager.SetUserVehicleCountLimit(ParseAndValidateInt(_userCommand, 1, 100));
        }

        //выводит либо все, либо только активные транспортные средства
        private bool CommandShowVehicles(bool all)
        {
            var _vehicleIndex = 0;
            if (all)
            {
                if (_userManager.GetUserVehicleList().Any())
                {
                    Console.WriteLine("В вашем гараже:\n");
                    _userManager.GetUserVehicleList().ForEach(Vehicle => Console.WriteLine($"{_vehicleIndex++}. cтатус: {Vehicle.State}, название: {Vehicle.Name},  дата создания: {Vehicle.CreatedAt}, Id: {Vehicle.Id}"));
                    Console.ReadKey(true);
                    return true;
                }
            }
            else
            {
               
                Console.WriteLine("В вашем гараже:\n");
                _userManager.GetUserActiveVehicleList().ForEach(Vehicle => Console.WriteLine($"{_vehicleIndex++}. название: {Vehicle.Name},  дата создания: {Vehicle.CreatedAt}, Id: {Vehicle.Id}"));
                Console.ReadKey(true);
                return true;
            }
            Console.WriteLine($"{_userManager.GetUserName()}, ваш гараж пока пуст :(");
            Console.ReadKey(true);
            return false;
        }

        //Метод для удаления транспорта пользователя
        private void CommandRemoveVehicle()
        {

            if (CommandShowVehicles(true))
            {
                Console.WriteLine($"{_userManager.GetUserName()}, введите номер транспорта для удаления");
                _userCommand = Console.ReadLine();
                ValidateString(_userCommand);
                if (_userManager.DeleteUserVehicle(ParseInt(_userCommand)))
                {
                    Console.WriteLine("Транспорт убран из Garage");
                }
                else
                {
                    Console.WriteLine("Транспорт не был найден");
                }
            }
            Console.ReadKey(true);
        }

        private void CommandMoveToService(List<string> id)
        {
            if (id.Any())
            {
                if (_userManager.MoveToService(id.ElementAt(0)))
                {
                    Console.WriteLine("Транспорта был перемещён в сервис");
                }
                else
                {
                    Console.WriteLine("Транспорт не найден");
                }
            }
            else
            {
                Console.WriteLine("Не задан Id");
            }
            Console.ReadKey(true);
        }

        //Проверка соответствия условиям и возможности преобразования строки в инт
        private int ParseAndValidateInt(string? userCommand, int minCount, int maxCount)
        {
            int index;
            bool _isInteger = int.TryParse(userCommand, out index);

            if (!_isInteger || index < minCount || maxCount < index)
            {
                throw new ArgumentException();
            }

            return index;
        }

        //Проверка валидности ввода
        private void ValidateString(string userCommand)
        {
            if (string.IsNullOrWhiteSpace(userCommand) || string.IsNullOrEmpty(userCommand))
            {
                throw new ArgumentException();
            }
        }

        //Проверка, что строка - это число
        private int ParseInt(string? userCommand)
        {
            int index;
            bool _isInteger = int.TryParse(userCommand, out index);

            if (!_isInteger)
            {
                throw new ArgumentException();
            }

            return index;
        }
    }
}