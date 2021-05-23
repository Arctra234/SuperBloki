using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperBloki.Domain.Abstract;
using SuperBloki.Domain.Entities;
using SuperBloki.WebUI.Models;

namespace SuperBloki.WebUI.Controllers
{
    public class ConstructorController : Controller
    {
        private IConstructorRepository repository;
        //ilość towaru na strone
        public int pageSize = 4;
        public ConstructorController(IConstructorRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string producer, int page = 1)
        {
            ConstructorsListViewModel model = new ConstructorsListViewModel
            {
                Constructors = repository.Constructors
                    .Where(p => producer == null || p.Producer == producer)
                    .OrderBy(constructor => constructor.ConstructorID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                        CurrentPage = page,
                        ItemsPerPage = pageSize,
                        TotalItems = producer == null ?
                repository.Constructors.Count() :
                repository.Constructors.Where(constructor => constructor.Producer == producer).Count()
                },
                CurrentProducer = producer
            };
            return View(model);
        }

        public FileContentResult GetImage(int constructorID)
        {
            Constructor constructor = repository.Constructors
                .FirstOrDefault(g => g.ConstructorID == constructorID);

            if (constructor != null)
            {
                return File(constructor.ImageData, constructor.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

    }
}