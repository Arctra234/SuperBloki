using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuperBloki.Domain.Entities;

namespace SuperBloki.WebUI.Models
{
    public class ConstructorsListViewModel
    {
        public IEnumerable<Constructor> Constructors { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentProducer { get; set; }
    }
}