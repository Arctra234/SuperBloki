using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web.Mvc;
using Moq;
using SuperBloki.Domain.Entities;
using SuperBloki.Domain.Abstract;
using SuperBloki.WebUI.Controllers;
using SuperBloki.WebUI.Models;


namespace SuperBloki.UnitTests
{
    
    [TestClass]
    public class CartTests
    {
        // Przetestowanie dodawanie pozycji do koszyka
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Organizacja - utworzenie wiele testowych klocków
            Constructor constructor1 = new Constructor { ConstructorID = 1, Name = "Klocki1" };
            Constructor constructor2 = new Constructor { ConstructorID = 2, Name = "Klocki2" };

            // Organizacja - tworzenie koszyka
            Cart cart = new Cart();


            // Akcja
            cart.AddItem(constructor1, 1);
            cart.AddItem(constructor2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Instrukcja
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Constructor, constructor1);
            Assert.AreEqual(results[1].Constructor, constructor2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Organizacja - utworzenie wiele testowych klocków
            Constructor constructor1 = new Constructor { ConstructorID = 1, Name = "Klocki1" };
            Constructor constructor2 = new Constructor { ConstructorID = 2, Name = "Klocki2" };

            // Organizacja - tworzenie koszyka
            Cart cart = new Cart();

            // Akcja
            cart.AddItem(constructor1, 1);
            cart.AddItem(constructor2, 1);
            cart.AddItem(constructor1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Constructor.ConstructorID).ToList();

            // Instrukcja
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);   // 6 kopii dodanych do koszyka
            Assert.AreEqual(results[1].Quantity, 1);
        }

        // Sprawdzenie, czy użytkownicy mają możliwość zmiany zdania i usunięcia pozycji z koszyka
        [TestMethod]
        public void Can_Remove_Line()
        {
            // Organizacja - utworzenie wiele testowych klocków
            Constructor constructor1 = new Constructor { ConstructorID = 1, Name = "Klocki1" };
            Constructor constructor2 = new Constructor { ConstructorID = 2, Name = "Klocki2" };
            Constructor constructor3 = new Constructor { ConstructorID = 3, Name = "Klocki3" };

            // Organizacja - tworzenie koszyka
            Cart cart = new Cart();

            // ORGANIZACJA - dodanie kilku klocków do koszyka
            cart.AddItem(constructor1, 1);
            cart.AddItem(constructor2, 4);
            cart.AddItem(constructor3, 2);
            cart.AddItem(constructor2, 1);

            // Akcja
            cart.RemoveLine(constructor2);

            // Instrukcja
            Assert.AreEqual(cart.Lines.Where(c => c.Constructor == constructor2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        // Testowane zachowanie dotyczy możliwości obliczenia całkowitego kosztu pozycji w koszyku
        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Organizacja - utworzenie wiele testowych klocków
            Constructor constructor1 = new Constructor { ConstructorID = 1, Name = "Klocki1", Priсe = 100 };
            Constructor constructor2 = new Constructor { ConstructorID = 2, Name = "Klocki2", Priсe = 55 };

            // Organizacja - tworzenie koszyka
            Cart cart = new Cart();

            // Akcja
            cart.AddItem(constructor1, 1);
            cart.AddItem(constructor2, 1);
            cart.AddItem(constructor1, 5);
            decimal result = cart.ComputeTotalValue();

            // Instrukcja
            Assert.AreEqual(result, 655);
        }

        //Test sprawdzający opróżnianie kosza od przedmiotów

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Organizacja - utworzenie wiele testowych klocków
            Constructor constructor1 = new Constructor { ConstructorID = 1, Name = "Klocki1", Priсe = 100 };
            Constructor constructor2 = new Constructor { ConstructorID = 2, Name = "Klocki2", Priсe = 55 };

            // Organizacja - tworzenie koszyka
            Cart cart = new Cart();

            // Akcja
            cart.AddItem(constructor1, 1);
            cart.AddItem(constructor2, 1);
            cart.AddItem(constructor1, 5);
            cart.Clear();

            // Instrukcja
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        // Sprawdzanie dodania do koszyka   
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Organizacja - tworzenie symulowanego repozytoriuma
            Mock<IConstructorRepository> mock = new Mock<IConstructorRepository>();
            mock.Setup(m => m.Constructors).Returns(new List<Constructor> {
                new Constructor {ConstructorID = 1, Name = "Klocki1", Producer = "Prod1"},
            }.AsQueryable());

            // Organizacja - tworzenie koszyka
            Cart cart = new Cart();

            // Organizacja - tworzenie kontrolera
            CartController controller = new CartController(mock.Object, null);

            // Akcja - dodanie klocków do koszyka
            controller.AddToCart(cart, 1, null);

            // Instrukcja
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Constructor.ConstructorID, 1);
        }


        // Po dodaniu klocków do koszyka powinno nastąpić przekierowanie do strony koszyka
        [TestMethod]
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
        {
            // Organizacja - tworzenie symulowanego repozytoriuma
            Mock<IConstructorRepository> mock = new Mock<IConstructorRepository>();
            mock.Setup(m => m.Constructors).Returns(new List<Constructor> {
                new Constructor {ConstructorID = 1, Name = "Klocki1", Producer = "Prod1"},
            }.AsQueryable());

            // Organizacja - tworzenie koszyka
            Cart cart = new Cart();


            // Organizacja - tworzenie kontrolera
            CartController controller = new CartController(mock.Object, null);

            // Akcja - dodanie klocków do koszyka
            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            // Instrukcja
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        // Sprawdzanie adres URL
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Organizacja - tworzenie koszyka
            Cart cart = new Cart();

            // Organizacja - tworzenie kontrolera
            CartController target = new CartController(null, null);

            // Action - wywolanie metody akcji Index ()
            CartIndexViewModel result
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Instrukcja
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }


        //Ten test pozwala sprawdzić brak możliwości przejścia na płatność, gdy koszyk jest pusty
        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Organizacja - tworzenie symulowanego procesora zamówień
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Organizacja - tworzenie koszyka
            Cart cart = new Cart();

            // Organizacja - tworzenie szczególów wysyłki
            ShippingDetails shippingDetails = new ShippingDetails();

            // Organizacja - tworzenie kontrolera
            CartController controller = new CartController(null, mock.Object);

            // Action
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Asercja - sprawdzenie, czy zamówienie nie zostało przekazane do handlera 
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());


            // Asercja - sprawdzenie, czy metoda zwróciła standardowy widok
            Assert.AreEqual("", result.ViewName);

            // Asercja - sprawdzenie, czy do widoku został przekazany niewłaściwy model
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        //Taki samy test, ale wprowadziłem błąd do modelu widoku, który emuluje problem zgłoszony przez spinacz modelu
        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Organizacja - tworzenie symulowanego procesora zamówień
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Organizacja - tworzenie koszyka
            Cart cart = new Cart();
            cart.AddItem(new Constructor(), 1);

            // Organizacja - tworzenie kontrolera
            CartController controller = new CartController(null, mock.Object);

            // Organizacja - dodanie blęda do modelu
            controller.ModelState.AddModelError("error", "error");

            // Akcja - próba przejścia do płatności
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Asercja - sprawdzenie, czy zlecenie nie jest przekazywane do procesora
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Asercja - sprawdzenie, czy metoda zwróciła standardowy widok
            Assert.AreEqual("", result.ViewName);

            // Asercja - sprawdzenie, czy do widoku został przekazany niewłaściwy model
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }


        //Test pracy przy przetwarzaniu normalnych zamówień jest wykonywany poprawnie
        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Organizacja - tworzenie symulowanego procesora zamówień
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Organizacja - tworzenie koszyka z elementem
            Cart cart = new Cart();
            cart.AddItem(new Constructor(), 1);

            // Organizacja - tworzenie kontrolera
            CartController controller = new CartController(null, mock.Object);

            // Akcja - próba przejścia do płatności
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Asercja - sprawdzenie, czy zamówienie zostało przekazane do programu obsługi
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());

            // Asercja - sprawdzenie, czy metoda zwraca widok 
            Assert.AreEqual("Completed", result.ViewName);

            // Asercja - sprawdzenie, czy do widoku przekazano prawidłowy model
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }

    }
}
