using BloggerAPI.Models;
using BloggerAPI.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            Users user = new Users { Username = "sujoy123", Firstname = "Sujoy", Lastname = "Basak" };
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);
            db = mockContext.Object;
        }

        [Test]
        public void PostUserPositive()
        {
            UsersController controller = new UsersController(db);
            Users user = new Users { Username = "sujoy123", Firstname = "Sujoy", Lastname = "Basak" };
            var data = controller.Post(user);
            var result = data as ObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void PostUserNegative()
        {
            UsersController controller = new UsersController(db);
            Users user = new Users { Username = "sujoy123", Firstname = "Sujoy", Lastname = "Basak" };
            var data = controller.Post(user);
            var result = data as ObjectResult;
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public void GetUsers()
        {
            UsersController controller = new UsersController(db);
            var data = controller.Get();
            var result = data as ObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetByUsername()
        {
            UsersController controller = new UsersController(db);
            var data = controller.Get("sujoy123");
            var result = data as ObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}