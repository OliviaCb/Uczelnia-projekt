using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_Biblioteka
{
    //oznacza mozliwosc serializacji klasy(b.formater)

    [Serializable]
    public class Library
    {
        private string name;
        private List<Book> bookList;
        public List<Admin> adminList;
        private List<User> userList;
        private List<string> categoriesOfBooks;

        public Person actualLoggedUser = null;

        public string Name { get => name; private set => name = value; }

        public Library(string name)
        {
            this.Name = name;
            this.bookList = new List<Book>();
            this.adminList = new List<Admin>();
            this.userList = new List<User>();
            this.categoriesOfBooks = new List<string>();
        }
        #region Akcje na userze
        public void addUser(User user)
        {
            this.userList.Add(user);
        }

        public int numberOfUsers()
        {
            return userList.Count;
        }

        public void removeUser(int id)
        {
            GiveBackBooks(id);
            this.userList.RemoveAt(id - 1);

        }

        public void GiveBackBooks(int id)
        {
            string category = this.bookList[id - 1].category;

            if (bookList.Find(x => x.category == category) == null)
            {
                this.categoriesOfBooks.Remove(category);
            }
            User user = this.userList[id - 1];
            List<Book> borrowedBooks = user.GetBookList();
            Book borrowedBook;
            for (int i = 0; i < borrowedBooks.Count; i++)
            {
                borrowedBook = borrowedBooks[i];
                if (this.bookList.Contains(borrowedBook))
                {
                    bookList.Find(book => book == borrowedBook).Borrowed--;
                    bookList.Find(book => book == borrowedBook).InStock++;
                }
            }
        }

        public void DisplayCategories()
        {
            if (categoriesOfBooks.Count == 0) Console.WriteLine("Brak ksiazek to i brak kategorii ;)");
            else
            {
                for (int i = 0; i < categoriesOfBooks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {categoriesOfBooks[i]}");
                }
            }

        }
        private void setCategoriesFromBooks()
        {
            if (bookList.Count > 0)
            {
                for (int i = 0; i < bookList.Count; i++)
                {
                    if (categoriesOfBooks.Count == 0)
                    {
                        categoriesOfBooks.Add(bookList[i].category);
                    }
                    else if (!categoriesOfBooks.Contains(bookList[i].category))
                    {
                        categoriesOfBooks.Add(bookList[i].category);
                    }
                }
            }
        }
        public void DrawFromCategory(int id)
        {
            string category = categoriesOfBooks[id - 1];
            List<Book> booksListByCategory = bookList.FindAll(x => x.category == category);

            Random rnd = new Random();
            int randomNumber = rnd.Next(1, booksListByCategory.Count);
            Console.WriteLine($"Wylosowana ksiazka z kategorii {category} to '{booksListByCategory[randomNumber].title}'\n" +
                $"Autora : {bookList[randomNumber].author}");

        }

        public void displayUsers()
        {
            for (int i = 0; i < userList.Count; i++)
            {
                if (userList.Count == 0) Console.WriteLine("Brak uzytkownikow");
                else
                {
                    Console.Write($"{i + 1}. ");
                    userList[i].PresentYourself();
                    Console.WriteLine();
                }
            }
        }
        public void SetName(string name)
        {
            this.name = name;
        }

        public User getUser(int id)
        {
            User user = userList[id - 1];
            return user;
        }
        public void replaceUser(User user, int id)
        {
            userList[id - 1] = user;
            Console.WriteLine("Pomyslnie podmieniono uzykownika");
        }
        #endregion

        #region Akcje na ksiazkach
        public void addBook(Book book)
        {
            this.bookList.Add(book);
            setCategoriesFromBooks();
        }

        public void GiveBackBook(Book book)
        {
            this.bookList.Find(x => x == book).InStock++;
            this.bookList.Find(x => x == book).Borrowed--;
            Console.WriteLine("Pomyslnie oddano ksiazke");
            Console.ReadLine();
        }

        public void RefillBooks(int amount, int id)
        {
            bookList[id - 1].InStock += amount;
            Console.WriteLine("Pomyslnie dodano zapasy");
        }

        public int getBookAmount()
        {
            return this.bookList.Count;
        }

        public void RemoveBook(int id)
        {
            this.bookList.RemoveAt(id - 1);
            Console.WriteLine("Pomyslnie usunieto ksiazke");
        }

        public void displayBooks()
        {
            for (int i = 0; i < bookList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {bookList[i].title}  autora : {bookList[i].author}\n" +
                    $"Stron: {bookList[i].nopages}, kategoria: {bookList[i].category} " +
                    $"W bibliotece: {bookList[i].InStock} , wypozyczonych: {bookList[i].Borrowed}");
            }
        }

        public Book borrowBook(int id)
        {
            if (reduceInStock(id))
            {
                return bookList[id - 1];
            }
            Console.ReadKey();
            return null;

        }
        private bool reduceInStock(int id)
        {
            if (bookList[id - 1].InStock - 1 < 0)
            {
                Console.WriteLine("Brak w magazynie");
                return false;
            }
            else
            {
                bookList[id - 1].InStock--;
                bookList[id - 1].Borrowed++;
                return true;
            }
        }
        #endregion

        #region Serializacja danych
        public List<Book> GetListOfBooks()
        {
            return this.bookList;
        }
        public List<string> GetListOfCategories()
        {
            return this.categoriesOfBooks;
        }
        public List<Admin> GetListOfAdmins()
        {
            return this.adminList;
        }
        public List<User> GetListOfUsers()
        {
            return this.userList;
        }
        public void SetListOf<T>(List<T> deserializedList)
        {
            if (deserializedList is List<Book>) this.bookList = deserializedList as List<Book>;
            else if (deserializedList is List<User>) this.userList = deserializedList as List<User>;
            else if (deserializedList is List<Admin>) this.adminList = deserializedList as List<Admin>;
            else this.categoriesOfBooks = deserializedList as List<string>;
        }

        #endregion
        public void addAdmin(Admin admin)
        {
            this.adminList.Add(admin);
        }


        #region Logowanie
        public bool adminLogin(string username, string password)
        {
            bool isLoggedin = false;
            for (int i = 0; i < adminList.Count; i++)
            {
                if (username != adminList[i].Login) Console.WriteLine("Nieprawidlowy login");
                else if (password != adminList[i].Password) Console.WriteLine("Nieoprawidlowe haslo");
                else
                {
                    Console.WriteLine("Zalogowano");
                    actualLoggedUser = adminList[i];
                    isLoggedin = true;
                    break;
                }
            }
            return isLoggedin;
        }

        public bool userLogin(string username, string password)
        {
            bool isLoggedin = false;
            for (int i = 0; i < userList.Count; i++)
            {
                if (username != userList[i].Login) Console.WriteLine("Nieprawidlowy login");
                else if (password != userList[i].Password) Console.WriteLine("Nieoprawidlowe haslo");
                else
                {
                    Console.WriteLine("Zalogowano");
                    actualLoggedUser = userList[i];
                    isLoggedin = true;
                    break;
                }
            }
            return isLoggedin;
        }
        #endregion
    }

}
