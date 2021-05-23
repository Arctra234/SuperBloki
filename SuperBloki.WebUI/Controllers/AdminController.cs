using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SuperBloki.Domain.Abstract;
using SuperBloki.Domain.Entities;

namespace SuperBloki.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IConstructorRepository repository;

        public AdminController(IConstructorRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Constructors);
        }

        public ViewResult Edit(int constructorID)
        {
            Constructor constructor = repository.Constructors
                .FirstOrDefault(g => g.ConstructorID == constructorID);
            return View(constructor);
        }

        [HttpPost]
        public ActionResult Edit(Constructor constructor, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    constructor.ImageMimeType = image.ContentType;
                    constructor.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(constructor.ImageData, 0, image.ContentLength);
                }
                repository.SaveConstructor(constructor);
                TempData["message"] = string.Format("Zmiany w klockah  \"{0}\" zostały zapisane", constructor.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(constructor);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Constructor());
        }

        [HttpPost]
        public ActionResult Delete(int constructorID)
        {
            Constructor deletedConstructor = repository.DeleteConstructor(constructorID);
            if (deletedConstructor != null)
            {
                TempData["message"] = string.Format("Klocki \"{0}\" zostały usunięte",
                    deletedConstructor.Name);
            }
            return RedirectToAction("Index");
        }
    }
}