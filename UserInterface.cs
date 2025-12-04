using System.Text;

namespace Garage.Bot
{
    // Класс пользователького интерфейса
    internal class UserInterface
    {
        private string? _userCommand;
        private User _user = new();
        private string[] _commands = { "/start", "/help", "/info", "/echo", "/exit" };
        private StringBuilder _userCommandBuilder = new();
        private List<string> _userCommandList = new List<string>();

        //Главное меню, от куда происходят переходы в подменю
        internal void MaimMenu()
        {

            do
            {
                _userCommandList.Clear();
                Console.Clear();

                if (_user.GetName() == null)
                {
                    Console.WriteLine($"Вас приветствует Garage.Bot!\nВведите команду и нажмите любую клавишу:\n{_userCommandBuilder.AppendJoin("\n", _commands)}");
                }
                else
                {
                    Console.WriteLine($"{_user.GetName()}, с возвращением в Garage.Bot :)\nВведите команду и нажмите любую клавишу:\n{_userCommandBuilder.AppendJoin("\n", _commands)}");
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
                            if (_user.GetName() == null)
                            {
                                CommandStart(ref _user);
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
            } while (!_userCommandList[0].Equals(_commands[4]));


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
                _user.SetName(_userCommand);
            }
        }

        // Описание работы рпограммы, пока не заполнено
        private void CommandHelp()
        {
            Console.WriteLine("Доступно пять команд:" +
                "\nКоманда /start - если пользователь ещё не вводил своё имя, то позволяет это сделать, если пользователь уже вводил своё имя, то возвращает его в главное меню без изменений" +
                "\nКоманда /help - данное меню с пояснениями :)" +
                "\nКоманда /info - выводит на экран дату создания программы и её версию" +
                "\nКоманда /echo - выводит на экран то, что было написано после самой команды, например \"/echo Hello\" выведет на экран \"Hello\"" +
                "\nКоманда /exit - выход из программы" +
                "\nПосле каждого вывода на экран, необходимо нажать любую клавишу для продолжения");
            Console.ReadKey();
        }

        // Информация о программе, версия и дата создания
        private void CommandInfo()
        {
            Console.WriteLine($"Garage.Bot\nv0.1.0\nДата создания: {File.GetCreationTimeUtc(System.Reflection.Assembly.GetExecutingAssembly().Location)}");
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
                Console.ReadKey();
            } else { 
                _userCommandBuilder.AppendJoin(" ", _echo);
            Console.WriteLine(_userCommandBuilder.ToString());
            Console.ReadKey();
                }
        }
    }
}
