using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using SuperBloki.Domain.Abstract;
using SuperBloki.Domain.Entities;
using SuperBloki.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace SuperBloki.UnitTests
{
    [TestClass]
    public class ImageTests
    {

        //Testowanie jednostkowe: wyodrębnianie obrazów

        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Organizacja - tworzenie obiekta Constructor z danymi obrazu
            Constructor constructor = new Constructor
            {
                ConstructorID = 2,
                Name = "Klocki2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };
            // Organizacja - tworzenie symulowanego repozytorium
            Mock<IConstructorRepository> mock = new Mock<IConstructorRepository>();
            mock.Setup(m => m.Constructors).Returns(new List<Constructor> {
                new Constructor {ConstructorID = 1, Name = "Klocki1"},
                constructor,
                new Constructor {ConstructorID = 3, Name = "Klocki3"}
            }.AsQueryable());

            // Organizacja - tworzenie kontrolera
            ConstructorController controller = new ConstructorController(mock.Object);

            // Action - wywołanie metodę akcji GetImage ()
            ActionResult result = controller.GetImage(2);

            // Wyniki
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(constructor.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Organizacja - tworzenie symulowanego repozytorium
            Mock<IConstructorRepository> mock = new Mock<IConstructorRepository>();
            mock.Setup(m => m.Constructors).Returns(new List<Constructor> {
                new Constructor {ConstructorID = 1, Name = "Klocki1"},
                new Constructor {ConstructorID = 2, Name = "Klocki2"}
            }.AsQueryable());

            // Organizacja - tworzenie kontrolera
            ConstructorController controller = new ConstructorController(mock.Object);

            // Action - wywołanie metodę akcji GetImage ()
            ActionResult result = controller.GetImage(10);

            // Wyniki
            Assert.IsNull(result);
        }
    }
}
