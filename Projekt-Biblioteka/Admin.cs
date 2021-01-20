using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_Biblioteka
{
    [Serializable]
    public class Admin : Person
    {

        public Admin(string name, string surame, string login, string password) : base(name, surame, login, password)
        {
        }
        
        public User addUser()
        {
            string name = validateString(3, 9, "Imię");
            string surname = validateString(3, 9, "Nazwisko");
            string login = validateString(3, 9, "Podaj login");
            string password = validateString(3, 9, "Podaj hasło");
            string address = validateString(1, 100, "Adres");
            string birthDate = validateString(1, 8, "Data urodzenia");

            User user = new User(name, surname, login, password, address, birthDate);
            return user;
        }

        public Book addBook()
        {
            int isbn = validateNumber(1, 13, "Podaj isbn <numer>");
            string category = validateString(1, 10, "Podaj kategorie");
            string author = validateString(1, 10, "Podaj autora");
            int pages = validateNumber(1, 10, "Podaj ilosc stron <numer>");
            string tittle = validateString(1, 10, "Podaj tytul");
            int inStock = validateNumber(1, 100, "Ile sztuk w magazynie");

            Book book = new Book(isbn, category, tittle, author, pages,inStock);
            return book;
        }

        #region Walidacje
        private string validateString(int min, int max, string message)
        {
            string input = "";
            while (input.Length < min || input.Length > max)
            {
                Console.WriteLine(message);
                input = Console.ReadLine();
            }
            return input;
        }
        public static int validateNumber(int min, int max, string msg)
        {
            bool validated = false;
            int number = 0;
            while (number < min || number > max)
            {
                if (validated == true) break;
                Console.WriteLine(msg);
                string input = Console.ReadLine();
                int.TryParse(input, out number);
                if (number < min) Console.WriteLine($"Liczba spoza zakresu, dozwolonyu zakres to{min} - {max}");
                else validated = true;
            }
            return number;
        }

        #endregion
    }
}
