using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SuperBloki.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Proszę podać imię")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Wstaw pierwszy adres wysyłki")]
        [Display(Name = "Pierwszy adres")]
        public string Line1 { get; set; }

        [Display(Name = "Drugi adres")]
        public string Line2 { get; set; }

        [Display(Name = "Trzeci adres")]
        public string Line3 { get; set; }

        [Required(ErrorMessage = "Proszę podać miasto")]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required(ErrorMessage = "Proszę podać kraj")]
        [Display(Name = "Kraj")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }

    }
}
