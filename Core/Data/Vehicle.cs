namespace Garage.Bot.Core.Data
{
    internal class Vehicle
    {
        internal string Name {  get; set; }
        internal Guid Id { get; }
        internal GarageUser User { get; }
        internal DateTime CreatedAt { get; }
        internal VehicleState State { get; set; }
        internal DateTime StateChangedAt { get; set; }

        internal Vehicle(GarageUser User, string Name)
        {
            this.Name = Name;
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            this.User = User;
            State = VehicleState.Active;
        }
    }
}
