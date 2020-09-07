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
    class BlogTest
    {
        BloggerContext db = new BloggerContext();
        [SetUp]
        public void Setup()
        {
            var Blog = new List<Blog>
            {
                new Blog{ Id=1, Title="My Coding Blog", Description="An awesome coding blog" },
                new Blog{ Id=2, Title="My Painting Blog", Description="An awesome painting blog" },
                new Blog{ Id=3, Title="My Travelling Blog", Description="An awesome travel blog" }
            };
            var Blogdata = Blog.AsQueryable();
            var mockSet = new Mock<DbSet<Blog>>();
            mockSet.As<IQueryable<Blog>>().Setup(m => m.Provider).Returns(Blogdata.Provider);
            mockSet.As<IQueryable<Blog>>().Setup(m => m.Expression).Returns(Blogdata.Expression);
            mockSet.As<IQueryable<Blog>>().Setup(m => m.ElementType).Returns(Blogdata.ElementType);
            mockSet.As<IQueryable<Blog>>().Setup(m => m.GetEnumerator()).Returns(Blogdata.GetEnumerator());
            var mockContext = new Mock<BloggerContext>();
            mockContext.Setup(c => c.Blogs).Returns(mockSet.Object);
            db = mockContext.Object;
        }


        [Test]
        public void GetBlog()
        {
            var repo = new Mock<BlogRepository>(db);
            BlogsController controller = new BlogsController(repo.Object);
            var data = controller.Get();
            var result = data as ObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetByBlogIdPositive()
        {
            var repo = new Mock<BlogRepository>(db);
            BlogsController controller = new BlogsController(repo.Object);
            var data = controller.Get(1);
            var result = data as ObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetByBlogIdNegative()
        {
            var repo = new Mock<BlogRepository>(db);
            BlogsController controller = new BlogsController(repo.Object);
            var data = controller.Get(4);
            var result = data as ObjectResult;
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public void PostBlog()
        {
            var repo = new Mock<BlogRepository>(db);
            BlogsController controller = new BlogsController(repo.Object);
            Blog user = new Blog { Id = 5, Title = "My Food Blog", Description = "An awesome food blog" };
            var data = controller.Post(user);
            Assert.AreEqual("Blog published successfully", data);
        }

        [Test]
        public void PutBlogPositive()
        {
            var repo = new Mock<BlogRepository>(db);
            BlogsController controller = new BlogsController(repo.Object);
            Blog user = new Blog { Title = "My Photography Blog", Description = "An awesome photography blog" };
            var data = controller.Put(2, user);
            Assert.AreEqual("Blog Updated", data);
        }

        [Test]
        public void PutBlogNegative()
        {
            var repo = new Mock<BlogRepository>(db);
            BlogsController controller = new BlogsController(repo.Object);
            Blog user = new Blog { Title = "My Food Blog", Description = "An awesome food blog" };
            var data = controller.Put(8, user);
            Assert.AreEqual("Blog could not be updated", data);
        }

        [Test]
        public void DeleteBlogPositive()
        {
            var repo = new Mock<BlogRepository>(db);
            BlogsController controller = new BlogsController(repo.Object);
            var data = controller.Delete(3);
            Assert.AreEqual("Blog deleted", data);
        }

        [Test]
        public void DeleteBlogNegative()
        {
            var repo = new Mock<BlogRepository>(db);
            BlogsController controller = new BlogsController(repo.Object);
            var data = controller.Delete(9);
            Assert.AreEqual("Blog could not be deleted", data);
        }
    }
}
