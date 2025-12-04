namespace Garage.Bot
{
    internal class User
    {
        private string? name;

        // Метод заполнения имени
        internal void SetName(string name) {  this.name = name; }
        // Метод для возврата имени пользователя
        internal string GetName() => name;
    }
}
