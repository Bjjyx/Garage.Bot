using System.Text;

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
                _userCommandList.Clear();
                Console.Clear();

                HelloMainMenu();

                _userCommand = Console.ReadLine();

                //В данном if строка с проверкой, что _userCommand не null добавлен исключительно, чтобы не было ворнинга в строке 31)
                if (IsInputCorrect(_userCommand) && _userCommand != null)
                {
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

            if (IsInputCorrect(_userCommand))
            {
                _userRegistred = _userManager.AddUserName(_userCommand);
            }
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
            Console.WriteLine($"Garage.Bot\nv0.3.1\nДата создания: {File.GetCreationTimeUtc(System.Reflection.Assembly.GetExecutingAssembly().Location)}");
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

                    if (IsInputCorrect(_userCommand))
                    {
                        int index;
                        bool _isInteger = int.TryParse(_userCommand, out index);

                        if (_isInteger)
                        {
                            if (_userManager.DeleteUserVehicle(index))
                            {
                                Console.WriteLine("Транспорт убран из Garage");
                            }
                            else
                            {
                                Console.WriteLine("Транспорт не был найден");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ошибка ввода, необходимо ввести целое число");
                        }
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
