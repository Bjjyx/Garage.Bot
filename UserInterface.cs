using System.Text;

namespace Garage.Bot
{
    // Класс пользователького интерфейса
    internal class UserInterface
    {
        private string? _userCommand;
        private User? _user;
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

                if (_user == null)
                {
                    Console.WriteLine($"Вас приветствует Garage.Bot!\nВведите команду и нажмите любую клавишу:\n{_userCommandBuilder.AppendJoin("\n", _commands)}");
                }
                else
                {
                    Console.WriteLine($"{_user.Name}, с возвращением в Garage.Bot :)\nВведите команду и нажмите любую клавишу:\n{_userCommandBuilder.AppendJoin("\n", _commands)}");
                }

                _userCommand = Console.ReadLine();

                if (_userCommand == null)
                {
                    Console.WriteLine("Вы отменили ввод");
                    Console.ReadKey();
                    _userCommandList.Add("");
                }
                else
                {
                    _userCommandList.AddRange(_userCommand.Trim().Split());

                    switch (_userCommandList[0].ToLowerInvariant())
                    {
                        case "/start":
                            Console.Clear();
                            if (_user == null)
                            {
                                CommandStart(ref _user);
                            }
                            break;
                        case "/help":
                            Console.Clear();
                            CommandHelp(ref _user);
                            break;
                        case "/info":
                            Console.Clear();
                            CommandInfo(ref _user);
                            break;
                        case "/echo":
                            Console.Clear();
                            _userCommandList.Remove("/echo");
                            CommandEcho(_userCommandList, ref _user);
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

        // Комманда старт, для нового пользователя предлагает ввести имя, для пользователя с именем делает ничего
        private void CommandStart(ref User _user)
        {
            Console.WriteLine("Введите ваше имя:");
            _userCommand = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(_userCommand))
            {
                Console.WriteLine("Прерван воод, либо отправленно пустое поле");
                Console.ReadKey();
            } else
            {
                _user = new();
                _user.Name = _userCommand;
            }
        }

        // Описание работы рпограммы, пока не заполнено
        private void CommandHelp(ref User _user)
        {
            if (!string.IsNullOrEmpty(_user.Name))
            {
                Console.WriteLine(_user.Name);
            }
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
        private void CommandInfo(ref User _user)
        {
            if (!string.IsNullOrEmpty(_user.Name))
            {
                Console.WriteLine(_user.Name);
            }
            Console.WriteLine($"Garage.Bot\nv0.3.0\nДата создания: {File.GetCreationTimeUtc(System.Reflection.Assembly.GetExecutingAssembly().Location)}");
            Console.ReadKey();
        }

        // Получает на вход текст, который был написан вместе с командой /echo и выводит его на экран
        private void CommandEcho(List<string> _echo, ref User _user)
        {
            _userCommandBuilder.Clear();
            if (!_echo.Any())
            {
                Console.WriteLine("Не задан текст для передачи");
                _userCommandList.Add(" ");
                Console.ReadKey();
            } else 
            { 
                if (!string.IsNullOrEmpty(_user.Name))
                {
                    Console.WriteLine($"{_user.Name}, вы ввели:");
                }
                _userCommandBuilder.AppendJoin(" ", _echo);
                Console.WriteLine(_userCommandBuilder.ToString());
                Console.ReadKey();
                _userCommandList.Insert(0,"");
            }
        }

        // Добавляет транспорт в список транспорта пользователя
        private void CommandAddVehicle()
        {
            if (_user == null)
            {
                Console.WriteLine("Вы не зарегистрировались :(");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Введите название транспортного средства:");
                _userCommand = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(_userCommand))
                {
                    Console.WriteLine("Ошибка ввода, название не может быть пустым");
                    Console.ReadKey();
                }
                else
                {
                    _user.AddVehicle(_userCommand);
                    Console.WriteLine("Транспорт успешно поставлен в Garage :)");
                    Console.ReadKey();
                }
            }
        }

        private void CommandShowAllVehicles()
        {
            if (_user == null)
            {
                Console.WriteLine("Вы не зарегистрированы");
                Console.ReadKey();
            }
            else
            {
                var _userVehicleList = _user.GetVehicleList();
                if (!_userVehicleList.Any())
                {
                    Console.WriteLine($"{_user.Name}, ваш гараж пока пуст :(");
                    Console.ReadKey();
                }
                else
                {
                   // Console.WriteLine("В вашем гараже:\n" + _userCommandBuilder.AppendJoin("\n", _userVehicleList));
                    Console.WriteLine("В вашем гараже:\n");
                    _userVehicleList.ForEach(Vehicle => Console.WriteLine(Vehicle.GetName()));
                    Console.ReadKey();
                }
            }
        }

        //Метод для удаления транспорта пользователя
        //!!! Переработать при рефакторинге !!!
        private void CommandRemoveVehicle()
        {
            if (_user == null)
            {
                Console.WriteLine("Вы не зарегистрированы");
                Console.ReadKey();
            }
            else
            {
                var _userVehicleList = _user.GetVehicleList();
                if (!_userVehicleList.Any())
                {
                    Console.WriteLine($"{_user.Name}, ваш гараж пока пуст :(");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Выберите что нужно убрать из гаража:\n");
                    _userVehicleList.ForEach(Vehicle => Console.WriteLine(Vehicle.GetName()));

                    int _userVehicleNumber = -1;
                    string _userVehicleName = "";
                    _userCommand = Console.ReadLine();

                    _userVehicleList.ForEach(Vehicle => {
                        if (string.Equals(Vehicle.GetName(), _userCommand, StringComparison.OrdinalIgnoreCase))
                        {
                            _userVehicleNumber = _userVehicleList.IndexOf(Vehicle);
                            _userVehicleName = Vehicle.GetName();
                        }
                    });

                    if (_userVehicleNumber == -1)
                    {
                        Console.WriteLine("Транспорт не найден");
                        Console.ReadKey();
                    } else
                    {
                        _user.RemoveVehicle(_userVehicleNumber);
                        Console.WriteLine(($"{_user.Name}, транспорт {_userVehicleName} был удалён"));
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
