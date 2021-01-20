using System;
using System.Collections.Generic;

namespace Projekt_Biblioteka
{
    [Serializable]
    public class User : Person
    {
        private string address;
        private string birthDate;
        private List<Book> borrowedBooks;
        

        public string Address { get => address; private set => address = value; }
        public string BirthDate { get => birthDate; private set => birthDate = value; }

        public User(string name, string surame, string login, string password, string address, string birthDate) : base(name, surame, login, password)
        {
            this.Address = address;
            this.BirthDate = birthDate;
            this.borrowedBooks = new List<Book>();
            
        }
        
        #region Setery
        public void SetAddress(string address)
        {
            this.address = address;
        }
        public void SetBirthDate(string birthDate)
        {
            this.birthDate = birthDate;
        }
        #endregion

        #region Akcje na ksiazkach
        public void borrowBook(Book book)
        {
            if (borrowedBooks.Contains(book))
            {
                Console.WriteLine("Masz juz ta ksiazke");
                Console.ReadKey();
            }
            else if (book != null)
            {
                this.borrowedBooks.Add(book);
            }
        }
        public Book GiveBackBook(int id)
        {
            
            Book bookToReturn = borrowedBooks[id - 1];
            this.borrowedBooks.RemoveAt(id - 1);
            
            return bookToReturn;
        }
        public void displayBorrowedBooks()
        {
            if (borrowedBooks.Count == 0) Console.WriteLine("Brak książek do wyświetlenia");
            else
            {
                for (int i = 0; i < borrowedBooks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}.Tytul: {borrowedBooks[i].title} | Autor: {borrowedBooks[i].author} | Kategoria: {borrowedBooks[i].category} |");
                }
            }
        }

        
        
        #endregion
       

        public List<Book> GetBookList()
        {
            return this.borrowedBooks;
        }

        public int GetAmountOfBooks()
        {
            return this.borrowedBooks.Count;
        }

        

        public override void PresentYourself()
        {
            Console.Write($"{this.Name} - {this.Surname} - {this.Address} - {this.BirthDate}");
        }

    }
}
