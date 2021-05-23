using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SuperBloki.Domain.Abstract;
using SuperBloki.Domain.Entities;
using SuperBloki.WebUI.Controllers;
using SuperBloki.WebUI.Models;
using SuperBloki.WebUI.HtmlHelpers;

namespace SuperBloki.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        //Testowanie paginacji
        [TestMethod]
        public void Can_Paginate()
        {
            // Organizacja (arrange)
            Mock<IConstructorRepository> mock = new Mock<IConstructorRepository>();
            mock.Setup(m => m.Constructors).Returns(new List<Constructor>
            {
                new Constructor { ConstructorID = 1, Name = "Klocki1"},
                new Constructor { ConstructorID = 2, Name = "Klocki2"},
                new Constructor { ConstructorID = 3, Name = "Klocki3"},
                new Constructor { ConstructorID = 4, Name = "Klocki4"},
                new Constructor { ConstructorID = 5, Name = "Klocki5"}
            });
            ConstructorController controller = new ConstructorController(mock.Object);
            controller.pageSize = 3;

            // akt (act)
            ConstructorsListViewModel result = (ConstructorsListViewModel)controller.List(null, 2).Model;

            // Komunikat (assert)
            List<Constructor> games = result.Constructors.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "Klocki4");
            Assert.AreEqual(games[1].Name, "Klocki5");
        }

        //  Ten test weryfikuje dane wyjściowe metody pomocniczej przy użyciu wartości ciągu literału zawierającej podwójne cudzysłowy.
        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Organizacja - zdefiniowanie pomocnika HTML - to konieczność
            // aby zastosować metodę rozszerzenia
            HtmlHelper myHelper = null;

            // Organizacja - tworzenie obiektu PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 27,
                ItemsPerPage = 10
            };

            // Organizacja - konfigurowanie delegata za pomocą wyrażenia lambda
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Akt
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Komunikat
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Organizacja (arrange)
            Mock<IConstructorRepository> mock = new Mock<IConstructorRepository>();
            mock.Setup(m => m.Constructors).Returns(new List<Constructor>
            {
                new Constructor { ConstructorID = 1, Name = "Klocki1"},
                new Constructor { ConstructorID = 2, Name = "Klocki2"},
                new Constructor { ConstructorID = 3, Name = "Klocki3"},
                new Constructor { ConstructorID = 4, Name = "Klocki4"},
                new Constructor { ConstructorID = 5, Name = "Klocki5"}
            });
            ConstructorController controller = new ConstructorController(mock.Object);
            controller.pageSize = 3;

            // Akt
            ConstructorsListViewModel result
                = (ConstructorsListViewModel)controller.List(null, 2).Model;

            // Komunikat
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        // Test, aby sprawdzić funkcjonalność filtrowania według producentów
        [TestMethod]
        public void Can_Filter_Games()
        {
            // Organizacja (arrange)
            Mock<IConstructorRepository> mock = new Mock<IConstructorRepository>();
            mock.Setup(m => m.Constructors).Returns(new List<Constructor>
            {
                new Constructor { ConstructorID = 1, Name = "Klocki1", Producer="Prod1"},
                new Constructor { ConstructorID = 2, Name = "Klocki2", Producer="Prod2"},
                new Constructor { ConstructorID = 3, Name = "Klocki3", Producer="Prod1"},
                new Constructor { ConstructorID = 4, Name = "Klocki4", Producer="Prod2"},
                new Constructor { ConstructorID = 5, Name = "Klocki5", Producer="Prod3"}
            });
            ConstructorController controller = new ConstructorController(mock.Object);
            controller.pageSize = 3;

            // Akt
            List<Constructor> result = ((ConstructorsListViewModel)controller.List("Prod2", 1).Model)
                .Constructors.ToList();

            // Komunikat (assert)
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Klocki2" && result[0].Producer == "Prod2");
            Assert.IsTrue(result[1].Name == "Klocki4" && result[1].Producer == "Prod2");
        }

        // Test jednostkowy przeznaczony do testowania możliwości generowania listy producentów
        [TestMethod]
        public void Can_Create_Producers()
        {
            // Organizacja - tworzenie symulowanego repozytorium
            Mock<IConstructorRepository> mock = new Mock<IConstructorRepository>();
            mock.Setup(m => m.Constructors).Returns(new List<Constructor>
            {
                new Constructor { ConstructorID = 1, Name = "Klocki1", Producer="LEGO"},
                new Constructor { ConstructorID = 2, Name = "Klocki2", Producer="LEGO"},
                new Constructor { ConstructorID = 3, Name = "Klocki3", Producer="PLAYMOBIL"},
                new Constructor { ConstructorID = 4, Name = "Klocki4", Producer="BIG"},
            });

            // Organizacja - tworzenie kontrolera
                        NavController target = new NavController(mock.Object);

            // Akcja - uzyskanie zestawu kategorii
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Komunikat
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "PLAYMOBIL");
            Assert.AreEqual(results[1], "LEGO");
            Assert.AreEqual(results[2], "BIG");
        }

        //Sprawdzenie, czy metoda działania Menu () poprawnie dodała szczegóły dotyczące wybranego producenta 

        [TestMethod]
        public void Indicates_Selected_Producer()
        {
            // Organizacja - tworzenie symulowanego repozytorium
            Mock<IConstructorRepository> mock = new Mock<IConstructorRepository>();
            mock.Setup(m => m.Constructors).Returns(new Constructor[] 
            {
                new Constructor { ConstructorID = 1, Name = "Klocki1", Producer="LEGO"},
                new Constructor { ConstructorID = 2, Name = "Klocki2", Producer="PLAYMOBIL"}
            });

            // Organizacja - tworzenie kontrolera
            NavController target = new NavController(mock.Object);

            // Organizacja - określenie wybranego producenta
            string producerToSelect = "PLAYMOBIL";

            // Akt
            string result = target.Menu(producerToSelect).ViewBag.SelectedProducer;

            // Komunikat
            Assert.AreEqual(producerToSelect, result);
        }

        // Testowanie możliwości generowania prawidłowych liczników produktów dla różnych producentów

        public void Generate_Category_Specific_Game_Count()
        {
            // Organizacja (arrange)
            Mock<IConstructorRepository> mock = new Mock<IConstructorRepository>();
            mock.Setup(m => m.Constructors).Returns(new List<Constructor>
            {
                new Constructor { ConstructorID = 1, Name = "Klocki1", Producer="Prod1"},
                new Constructor { ConstructorID = 2, Name = "Klocki2", Producer="Prod2"},
                new Constructor { ConstructorID = 3, Name = "Klocki3", Producer="Prod1"},
                new Constructor { ConstructorID = 4, Name = "Klocki4", Producer="Prod2"},
                new Constructor { ConstructorID = 5, Name = "Klocki5", Producer="Prod3"}
            });
            ConstructorController controller = new ConstructorController(mock.Object);
            controller.pageSize = 3;

            // Akcja - testowanie liczników produktów dla różnych producentów
            int res1 = ((ConstructorsListViewModel)controller.List("Prod1").Model).PagingInfo.TotalItems;
            int res2 = ((ConstructorsListViewModel)controller.List("Prod2").Model).PagingInfo.TotalItems;
            int res3 = ((ConstructorsListViewModel)controller.List("Prod3").Model).PagingInfo.TotalItems;
            int resAll = ((ConstructorsListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Komunikat
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
