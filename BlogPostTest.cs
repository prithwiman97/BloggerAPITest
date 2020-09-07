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
    class BlogPostTest
    {
        BloggerContext db = new BloggerContext();
        [SetUp]
        public void Setup()
        {
            var BlogPost = new List<BlogPost>
            {
                new BlogPost{ Id=1, Title="My Coding BlogPost", Content="An awesome coding BlogPost" },
                new BlogPost{ Id=2, Title="My Painting BlogPost", Content="An awesome painting BlogPost" },
                new BlogPost{ Id=3, Title="My Travelling BlogPost", Content="An awesome travel BlogPost" }
            };
            var BlogPostdata = BlogPost.AsQueryable();
            var mockSet = new Mock<DbSet<BlogPost>>();
            mockSet.As<IQueryable<BlogPost>>().Setup(m => m.Provider).Returns(BlogPostdata.Provider);
            mockSet.As<IQueryable<BlogPost>>().Setup(m => m.Expression).Returns(BlogPostdata.Expression);
            mockSet.As<IQueryable<BlogPost>>().Setup(m => m.ElementType).Returns(BlogPostdata.ElementType);
            mockSet.As<IQueryable<BlogPost>>().Setup(m => m.GetEnumerator()).Returns(BlogPostdata.GetEnumerator());
            var mockContext = new Mock<BloggerContext>();
            mockContext.Setup(c => c.BlogPosts).Returns(mockSet.Object);
            db = mockContext.Object;
        }


        [Test]
        public void GetBlogPost()
        {
            var repo = new Mock<BlogPostRepository>(db);
            BlogPostsController controller = new BlogPostsController(repo.Object);
            var data = controller.Get();
            var result = data as ObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetByBlogPostIdPositive()
        {
            var repo = new Mock<BlogPostRepository>(db);
            BlogPostsController controller = new BlogPostsController(repo.Object);
            var data = controller.Get(1);
            var result = data as ObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetByBlogPostIdNegative()
        {
            var repo = new Mock<BlogPostRepository>(db);
            BlogPostsController controller = new BlogPostsController(repo.Object);
            var data = controller.Get(4);
            var result = data as ObjectResult;
            Assert.AreEqual(400, result.StatusCode);
        }

        [Test]
        public void PostBlogPost()
        {
            var repo = new Mock<BlogPostRepository>(db);
            BlogPostsController controller = new BlogPostsController(repo.Object);
            BlogPost user = new BlogPost { Id = 5, Title = "My Food BlogPost", Content = "An awesome food BlogPost" };
            var data = controller.Post(user);
            Assert.AreEqual("Blog Post Added", data);
        }

        [Test]
        public void PutBlogPostPositive()
        {
            var repo = new Mock<BlogPostRepository>(db);
            BlogPostsController controller = new BlogPostsController(repo.Object);
            BlogPost user = new BlogPost { Title = "My Photography BlogPost", Content = "An awesome photography BlogPost" };
            var data = controller.Put(2, user);
            Assert.AreEqual("Post updated successfully", data);
        }

        [Test]
        public void PutBlogPostNegative()
        {
            var repo = new Mock<BlogPostRepository>(db);
            BlogPostsController controller = new BlogPostsController(repo.Object);
            BlogPost user = new BlogPost { Title = "My Food BlogPost", Content = "An awesome food BlogPost" };
            var data = controller.Put(8, user);
            Assert.AreEqual("Post could not be updated", data);
        }

        [Test]
        public void DeleteBlogPostPositive()
        {
            var repo = new Mock<BlogPostRepository>(db);
            BlogPostsController controller = new BlogPostsController(repo.Object);
            var data = controller.Delete(3);
            Assert.AreEqual("Post deleted successfully", data);
        }

        [Test]
        public void DeleteBlogPostNegative()
        {
            var repo = new Mock<BlogPostRepository>(db);
            BlogPostsController controller = new BlogPostsController(repo.Object);
            var data = controller.Delete(9);
            Assert.AreEqual("Post could not be deleted", data);
        }
    }
}
