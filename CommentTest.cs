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
    class CommentTest
    {
        BloggerContext db = new BloggerContext();
        [SetUp]
        public void Setup()
        {
            var Comment = new List<Comment>
            {
                new Comment{ Id=1, Content="An awesome coding Comment" },
                new Comment{ Id=2, Content="An awesome painting Comment" },
                new Comment{ Id=3, Content="An awesome travel Comment" }
            };
            var Commentdata = Comment.AsQueryable();
            var mockSet = new Mock<DbSet<Comment>>();
            mockSet.As<IQueryable<Comment>>().Setup(m => m.Provider).Returns(Commentdata.Provider);
            mockSet.As<IQueryable<Comment>>().Setup(m => m.Expression).Returns(Commentdata.Expression);
            mockSet.As<IQueryable<Comment>>().Setup(m => m.ElementType).Returns(Commentdata.ElementType);
            mockSet.As<IQueryable<Comment>>().Setup(m => m.GetEnumerator()).Returns(Commentdata.GetEnumerator());
            var mockContext = new Mock<BloggerContext>();
            mockContext.Setup(c => c.Comments).Returns(mockSet.Object);
            db = mockContext.Object;
        }


        [Test]
        public void GetComment()
        {
            var repo = new Mock<CommentRepository>(db);
            CommentsController controller = new CommentsController(repo.Object);
            var data = controller.Get();
            var result = data as ObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void GetByBlogPostId()
        {
            var repo = new Mock<CommentRepository>(db);
            CommentsController controller = new CommentsController(repo.Object);
            var data = controller.Get(1);
            var result = data as ObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public void PostComment()
        {
            var repo = new Mock<CommentRepository>(db);
            CommentsController controller = new CommentsController(repo.Object);
            Comment user = new Comment { Id = 5, Content = "An awesome food Comment" };
            var data = controller.Post(user);
            Assert.AreEqual("Comment added successfully", data);
        }

        [Test]
        public void PutCommentPositive()
        {
            var repo = new Mock<CommentRepository>(db);
            CommentsController controller = new CommentsController(repo.Object);
            Comment user = new Comment { Content = "An awesome photography Comment" };
            var data = controller.Put(2, user);
            Assert.AreEqual("Comment Updated", data);
        }

        [Test]
        public void PutCommentNegative()
        {
            var repo = new Mock<CommentRepository>(db);
            CommentsController controller = new CommentsController(repo.Object);
            Comment user = new Comment { Content = "An awesome food Comment" };
            var data = controller.Put(8, user);
            Assert.AreEqual("Comment could not be updated", data);
        }

        [Test]
        public void DeleteCommentPositive()
        {
            var repo = new Mock<CommentRepository>(db);
            CommentsController controller = new CommentsController(repo.Object);
            var data = controller.Delete(3);
            Assert.AreEqual("Comment deleted", data);
        }

        [Test]
        public void DeleteCommentNegative()
        {
            var repo = new Mock<CommentRepository>(db);
            CommentsController controller = new CommentsController(repo.Object);
            var data = controller.Delete(9);
            Assert.AreEqual("Comment could not be deleted", data);
        }
    }
}
