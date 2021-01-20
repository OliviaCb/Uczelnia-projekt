using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_Biblioteka
{
    [Serializable]
    public class Person : AppUser
    {
        
        public Person(string name, string surame, string login, string password) : base(name, surame, login, password)
        {
        }

        public override void PresentYourself()
        {
            Console.Write($"{this.Name} - {this.Surname}");
        }
    }
}
