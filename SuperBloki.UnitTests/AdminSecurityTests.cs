using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Moq;
using SuperBloki.WebUI.Controllers;
using SuperBloki.WebUI.Infrastructure.Abstract;
using SuperBloki.WebUI.Models;


namespace SuperBloki.UnitTests
{
    
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {

            // Organizacja - tworzenie symulację uwierzytelniania
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "12345")).Returns(true);

            // Organizacja - tworzenie modelu widoku
            // z poprawnymi poświadczeniami
            LoginViewModel model = new LoginViewModel
            {
                UserName = "admin",
                Password = "12345"
            };

            // Organizacja - tworzenie kontrolera
            AccountController target = new AccountController(mock.Object);


            // Akcja - uwierzytelnianie
            ActionResult result = target.Login(model, "/MyURL");

            // Instrukcja
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Cannot_Login_With_Invalid_Credentials()
        {
            // Organizacja - tworzenie dostawcę symulowanego uwierzytelniania
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("badUser", "badPass")).Returns(false);

            // Organizacja - tworzenie modelu widoku
            // z nieprawidłowymi danymi logowania
            LoginViewModel model = new LoginViewModel
            {
                UserName = "badUser",
                Password = "badPass"
            };

            // Organizacja - tworzenie kontrolera
            AccountController target = new AccountController(mock.Object);


            // Akcja - uwierzytelnianie
            ActionResult result = target.Login(model, "/MyURL");

            // Instrukcja
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}

