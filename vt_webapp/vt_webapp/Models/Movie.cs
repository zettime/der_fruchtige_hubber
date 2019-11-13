using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// using System.Data.Entity;

namespace vt_webapp.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
    }

    // Zuletzt bearbeitete Seite( Noch nicht fertig):
    //  -> https://docs.microsoft.com/de-de/aspnet/mvc/overview/getting-started/introduction/adding-a-model
}