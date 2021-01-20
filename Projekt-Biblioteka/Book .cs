using System;
using System.Collections.Generic;
using System.Text;

namespace Projekt_Biblioteka
{
    //oznacza mozliwosc serializacji klasy(b.formater)
    [Serializable]
    public class Book
    {
        public long isbn { get; set; }
        public string category { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public int nopages { get; set; }
        public int InStock { get; set; }
        public int Borrowed { get; set; }

        public Book(long isbn, string category, string title, string author, int nopages, int inStock)
        {
            this.isbn = isbn;
            this.category = category;
            this.title = title;
            this.author = author;
            this.nopages = nopages;
            this.InStock = inStock;
            this.Borrowed = 0;
        }

    }
}