using System;
using System.Text;
using Garage.Bot.CustomExceptions;

namespace Garage.Bot
{
    // Класс пользователького интерфейса
    internal class UserComandLineInterface
    {
        private string? _userCommand;
        private bool _userRegistred = false;
        private UserManager _userManager = new();
        private string[] _commands = { "/start", "/addVehicle", "/showVehicles", "/removeVehicle", "/help", "/info", "/echo", "/exit" };
        private StringBuilder _userCommandBuilder = new();
        private List<string> _userCommandList = new List<string>();

        //Главное меню, от куда происходят переходы в подменю
        internal void MaimMenu()
        {
            do
            {
                try
                {
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
                            if (!_userRegistred)
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
                            _userCommandList.Remove("/echo");
                            CommandEcho(_userCommandList);
                            break;
                        case "/addvehicle":
                            Console.Clear();
                            CommandAddVehicle();
                            break;
                        case "/showvehicles":
                            Console.Clear();
                            CommandShowAllVehicles();
                            break;
                        case "/removevehicle":
                            Console.Clear();
                            CommandRemoveVehicle();
                            break;
                        case "/exit":
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Введена неизвестная комманда! Попробуйте ещё раз :(");
                            _userCommand = "";
                            Console.ReadKey();
                            break;

                    }
                }
                catch (ArgumentException argEx)
                {
                    Console.WriteLine("Произошла непредвиденная ошибка:\n" + argEx.GetType() + "\n" + argEx.Message + "\n" + argEx.StackTrace + "\n" + argEx.InnerException);
                    Console.ReadKey();
                }
                catch (VehicleCountLimitException countEx)
                {
                    Console.WriteLine(countEx.Message);
                    Console.ReadKey();

                }
                catch (VehicleNameLengthLimitException lenghtEx)
                {
                    Console.WriteLine(lenghtEx.Message);
                    Console.ReadKey();

                }
                catch (DuplicateVehicleException duplicateEx)
                {
                    Console.WriteLine(duplicateEx.Message);
                    Console.ReadKey();

                }
                _userCommandBuilder.Clear();
            } while (!_userCommandList[0].Equals(_commands[7]));


        }

        //Приветствие в главном меню
        private void HelloMainMenu()
        {
            if (!_userRegistred)
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
                Console.WriteLine("Прерван воод, либо отправленно пустое поле");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void WriteUserNameAndText(string _text)
        {
            if (_userRegistred)
            {
                Console.WriteLine(_userManager.GetUserName() + _text);
            }
        }

        private bool WriteNotRegistred()
        {
            if (!_userRegistred)
            {
                Console.WriteLine("Вы не зарегистрировались :(");
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

            ValidateString( _userCommand );
            _userRegistred = _userManager.AddUserName(_userCommand);
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
                "\nКоманда /showVehicles - позволяет посмотреть, какие транспортные средства стоят у вас в Гараже" +
                "\nКоманда /removeVehicle - позволяет убрать транспорт из гаража" +
                "\nКоманда /exit - выход из программы" +
                "\nПосле каждого вывода на экран, необходимо нажать любую клавишу для продолжения");
            Console.ReadKey();
        }

        // Информация о программе, версия и дата создания
        private void CommandInfo()
        {
            WriteUserNameAndText("");
            Console.WriteLine($"Garage.Bot\nv0.4.0\nДата создания: {File.GetCreationTimeUtc(System.Reflection.Assembly.GetExecutingAssembly().Location)}");
            Console.ReadKey();
        }

        // Получает на вход текст, который был написан вместе с командой /echo и выводит его на экран
        private void CommandEcho(List<string> _echo)
        {
            _userCommandBuilder.Clear();
            if (!_echo.Any())
            {
                Console.WriteLine("Не задан текст для передачи");
                _userCommandList.Add(" ");
            } else 
            {
                WriteUserNameAndText(", вы ввели:");
                _userCommandBuilder.AppendJoin(" ", _echo);
                Console.WriteLine(_userCommandBuilder.ToString());
                _userCommandList.Insert(0,"");
            }
            Console.ReadKey();
        }

        // Добавляет транспорт в список транспорта пользователя
        private void CommandAddVehicle()
        {
            if (WriteNotRegistred())
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
            }
                
            Console.ReadKey();
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

        //Проверяет пуст ли гараж, если пуст выдаёт сообщение и завершает метод
        private bool GarageNotEmpty()
        {
            var _userVehicleList = _userManager.GetUserVehicleList();
            if (!_userVehicleList.Any())
            {
                Console.WriteLine($"{_userManager.GetUserName()}, ваш гараж пока пуст :(");
                return false;
            } else
            {
                return true;
            }
        }

        //Выводит названия + индексы транспортных средств юзвера
        private void WriteVehilces()
        {
            var _userVehicleList = _userManager.GetUserVehicleList();
            var _vehicleIndex = 0;
            Console.WriteLine("В вашем гараже:\n");
            _userVehicleList.ForEach(Vehicle => Console.WriteLine($"{_vehicleIndex++} {Vehicle.Name}"));
        }

        private void CommandShowAllVehicles()
        {
            if (WriteNotRegistred())
            {
                if (GarageNotEmpty())
                {
                    WriteVehilces();
                    Console.WriteLine();
                }
            }
            Console.ReadKey();
        }

        //Метод для удаления транспорта пользователя
        private void CommandRemoveVehicle()
        {
            if (WriteNotRegistred())
            {
                if (GarageNotEmpty())
                {
                    Console.WriteLine($"{_userManager.GetUserName()}, введите номер транспорта для удаления");
                    WriteVehilces();
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
            }

            Console.ReadKey();
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
