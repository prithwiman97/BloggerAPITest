using BloggerAPI.Models;
using BloggerAPI.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using BloggerAPI.Repositories;

namespace BloggerAPITestProject
{
    public class UsersTest
    {
        BloggerContext db = new BloggerContext();
        [SetUp]
        public void Setup()
        {
            var Users = new List<Users>
            {
                new Users{ Username="prithwi97",Firstname="Prithiwman",Lastname="Mazumdar" },
                new Users{ Username="soumyadip123",Firstname="Soumyadip",Lastname="Saha" },
                new Users{ Username="subham1234",Firstname="Subham",Lastname="Mitra" }
            };
            var Usersdata = Users.AsQueryable();
            var mockSet = new Mock<DbSet<Users>>();
            mockSet.As<IQueryable<Users>>().Setup(m => m.Provider).Returns(Usersdata.Provider);
            mockSet.As<IQueryable<Users>>().Setup(m => m.Expression).Returns(Usersdata.Expression);
            mockSet.As<IQueryable<Users>>().Setup(m => m.ElementType).Returns(Usersdata.ElementType);
            mockSet.As<IQueryable<Users>>().Setup(m => m.GetEnumerator()).Returns(Usersdata.GetEnumerator());
            var mockContext = new Mock<BloggerContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);
            db = mockContext.Object;
        }


        [Test]
        public void GetUsers()
        {
            var repo = new Mock<UsersRepository>(db);
            UsersController controller = new UsersController(repo.Object);
            var data = controller.Get();
            var result = data as ObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetByUsernamePositive()
        {
            var repo = new Mock<UsersRepository>(db);
            UsersController controller = new UsersController(repo.Object);
            var data = controller.Get("prithwi97");
            var result = data as ObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetByUsernameNegative()
        {
            var repo = new Mock<UsersRepository>(db);
            UsersController controller = new UsersController(repo.Object);
            var data = controller.Get("sujoy123");
            var result = data as ObjectResult;
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public void PostUser()
        {
            var repo = new Mock<UsersRepository>(db);
            UsersController controller = new UsersController(repo.Object);
            Users user = new Users { Username = "sujoy123", Firstname = "Sujoy", Lastname = "Basak" };
            var data = controller.Post(user);
            Assert.AreEqual("User Added Successfully", data);
        }

        [Test]
        public void PutUserPositive()
        {
            var repo = new Mock<UsersRepository>(db);
            UsersController controller = new UsersController(repo.Object);
            Users user = new Users { Firstname = "Prithwi", Lastname = "Mazumdar"};
            var data = controller.Put("prithwi97",user);
            Assert.AreEqual("User updated", data);
        }

        [Test]
        public void PutUserNegative()
        {
            var repo = new Mock<UsersRepository>(db);
            UsersController controller = new UsersController(repo.Object);
            Users user = new Users { Firstname = "Sujoy", Lastname = "Basak" };
            var data = controller.Put("sujoy123", user);
            Assert.AreEqual("User could not be updated", data);
        }

        [Test]
        public void DeleteUserPositive()
        {
            var repo = new Mock<UsersRepository>(db);
            UsersController controller = new UsersController(repo.Object);
            var data = controller.Delete("subham1234");
            Assert.AreEqual("User deleted successfully", data);
        }

        [Test]
        public void DeleteUserNegative()
        {
            var repo = new Mock<UsersRepository>(db);
            UsersController controller = new UsersController(repo.Object);
            var data = controller.Delete("sujoy123");
            Assert.AreEqual("User could not be deleted", data);
        }
    }
}