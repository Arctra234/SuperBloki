using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SuperBloki.Domain.Abstract;
using SuperBloki.Domain.Entities;
using SuperBloki.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SuperBloki.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        //Test na zachowanie metody akcji Index () w kontrolerze Admin
        public void Index_Contains_All_Games()
        {
            // Organizacja - tworzenie symulowanego magazynu danych
            Mock<IConstructorRepository> mock = new Mock<IConstructorRepository>();
            mock.Setup(m => m.Constructors).Returns(new List<Constructor>
            {
                new Constructor { ConstructorID = 1, Name = "Klocki1"},
                new Constructor { ConstructorID = 2, Name = "Klocki2"},
                new Constructor { ConstructorID = 3, Name = "Klocki3"},
                new Constructor { ConstructorID = 4, Name = "Klocki4"},
                new Constructor { ConstructorID = 5, Name = "Klocki5"}
            });

            // Organizacja - tworzenie kontrolera
            AdminController controller = new AdminController(mock.Object);

            // Akt
            List<Constructor> result = ((IEnumerable<Constructor>)controller.Index().
                ViewData.Model).ToList();

            // Assert
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Klocki1", result[0].Name);
            Assert.AreEqual("Klocki2", result[1].Name);
            Assert.AreEqual("Klocki3", result[2].Name);
        }

        // Testowanie metody akcji Edit ()

        [TestMethod]
        public void Can_Edit_Game()
        {
            // Organizacja - tworzenie symulowanego magazynu danych
            Mock<IConstructorRepository> mock = new Mock<IConstructorRepository>();
            mock.Setup(m => m.Constructors).Returns(new List<Constructor>
            {
                new Constructor { ConstructorID = 1, Name = "Klocki1"},
                new Constructor { ConstructorID = 2, Name = "Klocki2"},
                new Constructor { ConstructorID = 3, Name = "Klocki3"},
                new Constructor { ConstructorID = 4, Name = "Klocki4"},
                new Constructor { ConstructorID = 5, Name = "Klocki4"}
            });

            // Organizacja - tworzenie kontrolera
            AdminController controller = new AdminController(mock.Object);

            // Akt
            Constructor constructor1 = controller.Edit(1).ViewData.Model as Constructor;
            Constructor constructor2 = controller.Edit(2).ViewData.Model as Constructor;
            Constructor constructor3 = controller.Edit(3).ViewData.Model as Constructor;

            // Assert
            Assert.AreEqual(1, constructor1.ConstructorID);
            Assert.AreEqual(2, constructor2.ConstructorID);
            Assert.AreEqual(3, constructor3.ConstructorID);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Game()
        {
            // Organizacja - tworzenie symulowanego magazynu danych
            Mock<IConstructorRepository> mock = new Mock<IConstructorRepository>();
            mock.Setup(m => m.Constructors).Returns(new List<Constructor>
            {
                new Constructor { ConstructorID = 1, Name = "Klocki1"},
                new Constructor { ConstructorID = 2, Name = "Klocki2"},
                new Constructor { ConstructorID = 3, Name = "Klocki3"},
                new Constructor { ConstructorID = 4, Name = "Klocki4"},
                new Constructor { ConstructorID = 5, Name = "Klocki5"}
            });

            // Organizacja - tworzenie kontrolera
            AdminController controller = new AdminController(mock.Object);

            // Akt
            Constructor result = controller.Edit(6).ViewData.Model as Constructor;

            // Assert
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Organizacja - tworzenie symulowanego magazynu danych
            Mock<IConstructorRepository> mock = new Mock<IConstructorRepository>();

            // Organizacja - tworzenie kontrolera
            AdminController controller = new AdminController(mock.Object);

            // Organizacja - tworzenie obiekta Constructor
            Constructor constructor = new Constructor { Name = "Test" };

            // Akcja - próba zapisania obiekta
            ActionResult result = controller.Edit(constructor);

            // Asercja - sprawdzenie, czy dostęp do magazynu jest wykonywany
            mock.Verify(m => m.SaveConstructor(constructor));

            // Asercja - sprawdzenie typu wyniku metody
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Organizacja - tworzenie symulowanego magazynu danych
            Mock<IConstructorRepository> mock = new Mock<IConstructorRepository>();

            // Organizacja - tworzenie kontrolera
            AdminController controller = new AdminController(mock.Object);

            // Organizacja - tworzenie obiekta Constructor
            Constructor constructor = new Constructor { Name = "Test" };

            // Organizacja - dodanie błądu do stanu modeli
            controller.ModelState.AddModelError("error", "error");

            // Akcja - próba zapisania przedmiotu
            ActionResult result = controller.Edit(constructor);

            // Asercja - sprawdzenie, czy repozytorium NIE jest dostępne 
            mock.Verify(m => m.SaveConstructor(It.IsAny<Constructor>()), Times.Never());

            // Asercja - sprawdzenie typu wyniku metody
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        //Testowanie jednostkowe: usuwanie elementów

        [TestMethod]
        public void Can_Delete_Valid_Games()
        {
            // Organizacja - tworzenie obiekta Constructor
            Constructor constructor = new Constructor { ConstructorID = 2, Name = "Klocki2" };

            // Organizacja - tworzenie symulowanego magazynu danych
            Mock<IConstructorRepository> mock = new Mock<IConstructorRepository>();
            mock.Setup(m => m.Constructors).Returns(new List<Constructor>
            {
                new Constructor { ConstructorID = 1, Name = "Klocki1"},
                new Constructor { ConstructorID = 2, Name = "Klocki2"},
                new Constructor { ConstructorID = 3, Name = "Klocki3"},
                new Constructor { ConstructorID = 4, Name = "Klocki4"},
                new Constructor { ConstructorID = 5, Name = "Klocki5"}
            });

            // Organizacja - tworzenie kontrolera
            AdminController controller = new AdminController(mock.Object);

            // Akcja - usunięcie klocków
            controller.Delete(constructor.ConstructorID);

            // Asercja - sprawdzenie, czy metoda usuwania znajduje się w repozytorium
            // wywołało poprawny obiekt Constructor
            mock.Verify(m => m.DeleteConstructor(constructor.ConstructorID));
        }


    }
}