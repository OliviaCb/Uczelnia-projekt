using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Projekt_Biblioteka
{
    class Program
    {

        static void Main(string[] args)
        {


            Library library = new Library("Biblioteka");
            DeserializeLibrary(library);
            Menu(library);

        }
        #region Glowne menu
        static void Menu(Library library)
        {
            library.addAdmin(addAdmin());

            int x = 0;
            //numberForMenu();
            while (x != 3)
            {
                Console.Clear();
                Console.WriteLine(" _________________________");
                Console.WriteLine("|   *** Biblioteka ***    |");
                Console.WriteLine("|                         |");
                Console.WriteLine("|   1. Biblioteka         |");
                Console.WriteLine("|   2. Administrator      |");
                Console.WriteLine("|   3. Wyjscie            |");
                Console.WriteLine("|_________________________|");
                switch (x = validateNumber(1, 3, "Podaj"))
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Zaloguj sie jako user: ");
                        string userLogin = validateString(3, 25, "Podaj login");
                        string userPassword = validateString(3, 25, "Podaj hasło");
                        if (library.userLogin(userLogin, userPassword))
                        {
                            User user = library.actualLoggedUser as User;
                            Console.WriteLine($"Zalogowano jako {user.Name}");
                            Console.ReadKey();
                            userMenu(library);
                        }
                        else
                        {
                            Console.WriteLine("Niezalogowano");
                            Console.ReadKey();
                        }
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Zaloguj sie jako administrator: ");
                        string login = validateString(3, 25, "Podaj login");
                        string passsword = validateString(3, 25, "Podaj hasło");

                        if (library.adminLogin(login, passsword))
                        {
                            MenuAdmin(library);
                        }
                        else Console.WriteLine("Nie masz odpowiednich uprawnien");
                        break;
                    case 3:
                        SerializeLibrary(library);
                        Console.WriteLine("Zapisano zminay");
                        Console.ReadLine();
                        break;
                }
            }

        }
        #endregion

        #region Admin Panel
        static void MenuAdmin(Library library)
        {
            //rzutoanie usera na admina
            Admin admin = library.actualLoggedUser as Admin;

            int x = 0;
            //numberForMenu();
            while (x != 9)
            {
                Console.Clear();
                Console.WriteLine(" ___________________________________ ");
                Console.WriteLine("|   *** Panel Administratora ***    |");
                Console.WriteLine("|                                   |");
                Console.WriteLine("|   1. Doda Klienta                 |");
                Console.WriteLine("|   2. Usuń Klienta                 |");
                Console.WriteLine("|   3. Edytuj Klienta               |");
                Console.WriteLine("|   4. Dodaj Książkę                |");
                Console.WriteLine("|   5. Uzupełnij ksiazki            |");
                Console.WriteLine("|   6. Usuń Książkę                 |");
                Console.WriteLine("|   7. Lista Książek                |");
                Console.WriteLine("|   8. Lista Klientów               |");
                Console.WriteLine("|   9. Cofnij                       |");
                Console.WriteLine("|___________________________________|");

                switch (x = validateNumber(1, 8, "Wybierz liczbe"))
                {
                    case 1: //dodaj klienta
                        library.addUser(admin.addUser());
                        Console.WriteLine("Pomyslnie dodano klienta");
                        Console.ReadKey();
                        Console.Clear();

                        break;
                    case 2: //usun klienta
                        removeUser(library);
                        break;
                    case 3: //modyfikuj klienta
                        editUser(library);
                        break;
                    case 4: // dodaj ksiazke
                        Console.Clear();
                        library.addBook(admin.addBook());
                        break;
                    case 5://uzupelnij zapasy
                        refillBooks(library);
                        break;
                    case 6: //usun ksiazke
                        removeBook(library);
                        Console.ReadKey();
                        break;
                    case 7:// listka ksiazek
                        library.displayBooks();
                        Console.ReadKey();
                        break;
                    case 8://lista klientow
                        library.displayUsers();
                        Console.ReadKey();
                        break;
                    case 9:
                        library.actualLoggedUser = null;
                        break;

                }
            }

        }
        #region Akcje na uzytkowniku
        public static void removeUser(Library library)
        {
            Console.Clear();
            library.displayUsers();
            int number = validateNumber(1, library.numberOfUsers(), "Ktorego uzytkownika chcesz usunac");
            library.removeUser(number);
        }

        public static void editUser(Library library)
        {
            library.displayUsers();
            int number = validateNumber(1, library.numberOfUsers(), "Ktorego uzytkownika edytujesz?");
            User newUser = library.getUser(number);
            newUser.SetLogin(editString(1, 10, $"Czy zmenic login: {newUser.Login} - klikniecie enter nic nie zmieni", newUser.Login));
            newUser.SetName(editString(1, 10, $"Czy zmenic wartosc {newUser.Name} - klikniecie enter nic nie zmieni", newUser.Name));
            newUser.SetPassword(editString(1, 10, $"Czy zmenic haslo: {newUser.Password} - klikniecie enter nic nie zmieni", newUser.Password));
            newUser.SetSurname(editString(1, 10, $"Czy zmenic wartosc {newUser.Surname} - klikniecie enter nic nie zmieni", newUser.Surname));
            newUser.SetAddress(editString(1, 10, $"Czy zmenic wartosc {newUser.Address} - klikniecie enter nic nie zmieni", newUser.Address));
            newUser.SetBirthDate(editString(1, 10, $"Czy zmenic wartosc {newUser.BirthDate} - klikniecie enter nic nie zmieni", newUser.BirthDate));

            library.replaceUser(newUser, number);
        }
        #endregion

        #region Akcje na ksiazkach
        private static void removeBook(Library library)
        {
            library.displayBooks();
            int number = validateNumber(1, library.getBookAmount(), "Ktora ksiazke chcesz usunac?");
            library.RemoveBook(number);
        }

        private static void refillBooks(Library library)
        {
            library.displayBooks();
            int number = validateNumber(1, library.getBookAmount(), "Którą pozycje chcesz uzupełnić?");
            int amount = validateNumber(1, 100, "Ile ksiazek chcesz dodac?");
            library.RefillBooks(amount, number);
            Console.ReadLine();
        }
        #endregion

        #region walidacje
        public static string editString(int min, int max, string msg, string editedString)
        {
            string input = "";
            while (input.Length < min || input.Length > max)
            {
                Console.WriteLine(msg);
                input = Console.ReadLine();
                if (input.Length == 0)
                {
                    Console.WriteLine("Nie zmineiono wartości");
                    break;
                }
                if (input.Length < min) Console.WriteLine("Zbyt krótki text");
                else if (input.Length > max) Console.WriteLine("Zbyt długi text");
                else
                {
                    Console.WriteLine("Pomyslnie zmieniono wartość");
                    editedString = input;
                    break;
                }
            }
            return editedString;
        }
        public static string validateString(int min, int max, string msg)
        {
            bool validated = false;
            string input = "";
            while (input.Length < min || input.Length > max)
            {
                if (validated == true) break;
                Console.WriteLine(msg);
                input = Console.ReadLine();
                if (input.Length < min) Console.WriteLine("Zbyt krótki text");
                else if (input.Length > max) Console.WriteLine("Zbyt długi text");
                else validated = true;
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

        private static Admin addAdmin()
        {
            Admin admin = new Admin("jan", "jan", "jan", "jan");
            return admin;
        }

        #endregion

        #region User panel
        static void userMenu(Library library)
        {
            User user = library.actualLoggedUser as User;
            //  library.addAdmin(addAdmin());

            int x = 0;
            //numberForMenu();
            while (x != 7)
            {
                Console.Clear();
                Console.WriteLine(" _________________________");
                Console.WriteLine("|   *** User ***          |");
                Console.WriteLine("|                         |");
                Console.WriteLine("|   1. Wypozycz ksiazke   |");
                Console.WriteLine("|   3. Oddaj ksiazke      |");
                Console.WriteLine("|   3. Co mam wypozyczone |");
                Console.WriteLine("|   4. Historia wypozyczen|");
                Console.WriteLine("|   5. Kategorie ksiazek  |\n" +
                                  "|          w bibliotece   |");
                Console.WriteLine("|   6. Wylosuj ksiazke    |");
                Console.WriteLine("|   7. Cofnij             |");
                Console.WriteLine("|_________________________|");
                switch (x = validateNumber(1, 6, "Co chcesz zrobic"))
                {
                    case 1: // wyppozycz
                        borrowABook(library, user);
                        Console.Clear();

                        break;
                    case 2: //oddaj ksiazke
                        giveBackBook(user, library);
                        Console.Clear();
                        break;
                    case 3:// co sypozyczne
                        user.displayBorrowedBooks();
                        Console.ReadKey();
                        break;
                    case 4: //historia wypozyczen
                        break;
                    case 5: //kategorie ksiazek
                        library.DisplayCategories();
                        Console.ReadKey();
                        break;
                    case 6:
                        DrawRandomBookFromCategory(library);
                        break;
                }
            }

        }
        private static void DrawRandomBookFromCategory(Library library)
        {
            library.DisplayCategories();
            int number = validateNumber(1, library.getBookAmount(), "Ktora kategorie losujesz?");
            library.DrawFromCategory(number);
            Console.ReadLine();
        }
        private static void borrowABook(Library library, User user)
        {
            library.displayBooks();
            int number = validateNumber(1, library.getBookAmount(), "Ktora ksiazke chcesz wypozyczyc?");
            user.borrowBook(library.borrowBook(number));
        }

        private static void giveBackBook(User user, Library library)
        {
            user.displayBorrowedBooks();
            int number = validateNumber(1, user.GetAmountOfBooks(), "Ktora ksiazke chcesz oddac?");
            library.GiveBackBook(user.GiveBackBook(number));
        }
        #endregion
        #region Serializacja
        public static void SerializeLibrary(Library library)
        {
            List<Book> booksList = library.GetListOfBooks();
            List<User> usersList = library.GetListOfUsers();
            List<Admin> adminsList = library.GetListOfAdmins();
            List<string> categoriesList = library.GetListOfCategories();

            Serialize<User>("users.dat", usersList);
            Serialize<Book>("books.dat", booksList);
            Serialize<Admin>("admins.dat", adminsList);
            Serialize<string>("categories.dat", categoriesList);
        }

        public static void DeserializeLibrary(Library library)
        {
            Deserialize<User>(library, "users.dat");
            Deserialize<Book>(library, "books.dat");
            Deserialize<Admin>(library, "admins.dat");
            Deserialize<string>(library, "categories.dat");
        }

        //typ generyczny definuje jaki typ listy bedzie uzywany
        public static void Deserialize<T>(Library library, string fileName)
        {
            if (File.Exists(fileName))
            {
                FileStream fs = new FileStream(fileName, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                List<T> list = formatter.Deserialize(fs) as List<T>;
                library.SetListOf<T>(list);
                fs.Close();
            }
            else
            {
                Console.WriteLine($"Plik {fileName} nie isnieje");
                Console.ReadLine();
            }

        }

        private static void Serialize<T>(string fileName, List<T> dataToSerialize)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, dataToSerialize);
            fs.Close();
        }

        #endregion

    }
}
