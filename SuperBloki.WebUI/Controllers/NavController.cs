using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperBloki.Domain.Abstract;
using Microsoft.CSharp;

namespace SuperBloki.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IConstructorRepository repository;

        public NavController(IConstructorRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string producer = null)
        {
            ViewBag.SelectedProducer = producer;

            IEnumerable<string> producers = repository.Constructors
                .Select(constructor => constructor.Producer)
                .Distinct()
                .OrderBy(x => x);
            return PartialView("FlexMenu", producers);
        }

    }
}