using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperBloki.WebUI.Models
{
    public class PagingInfo
    {
        // Ilość towaru
        public int TotalItems { get; set; }

        // Liczba produktów na stronie
        public int ItemsPerPage { get; set; }

        // Numer bieżącej strony
        public int CurrentPage { get; set; }

        // Całkowita liczba stron
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }
}