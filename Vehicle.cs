using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage.Bot
{
    internal class Vehicle
    {
        private string _name;

        internal Vehicle(string _name)
        {
            this._name = _name;
        }

        internal string GetName()
        {
            return _name;
        }
    }
}
