using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SuperBloki.Domain.Entities
{
    public class Constructor
    {
        [HiddenInput(DisplayValue = false)]
        public int ConstructorID { get; set; }

        [Display(Name = "Numer klocków(od producenta)")]
        [Required(ErrorMessage = "Wpisz numer klocków")]
        public int ConstructorNumber { get; set; }

        [Display(Name = "Nazwa")]
        [Required(ErrorMessage = "Wpisz nazwę klocków")]
        public string Name { get; set; }

        [Display(Name = "Seria")]
        [Required(ErrorMessage = "Wpisz numer serię")]
        public string Series { get; set; }

        [Display(Name = "Producent")]
        [Required(ErrorMessage = "Wpisz producenta")]
        public string Producer { get; set; }

        [Display(Name = "Ilość elementów")]
        [Required(ErrorMessage = "Wpisz ilość elementów")]
        public int ElementsAmount { get; set; }

        [Display(Name = "Cena (zł)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Wprowadź dodatnią wartość ceny")]
        public decimal Priсe { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}
