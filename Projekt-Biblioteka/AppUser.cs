using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_Biblioteka
{
    [Serializable]
    public abstract class AppUser
    {
        private string login;
        private string password;
        private string name;
        private string surname;


        public string Login { get => login; private set => login = value; }
        public string Password { get => password; private set => password = value; }
        public string Name { get => name; private set => name = value; }
        public string Surname { get => surname; private set => surname = value; }

        public AppUser(string name, string surame, string login, string password)
        {
            Name = name;
            Surname = surame;
            Login = login;
            Password = password;
        }
        public abstract void PresentYourself();
        public void SetLogin(string login)
        {
            this.login = login;
        }
        public void SetPassword(string password)
        {
            this.password = password;
        }
        public void SetName(string name)
        {
            this.name = name;
        }
        public void SetSurname(string surname)
        {
            this.surname = surname;
        }
    }
}
