using FluentAssertions;
using GigHub.Controllers.Api;
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http.Results;

namespace GigHub.Tests.Controllers.Api
{
    [TestClass]
    public class GigsControllerTests
    {
        private GigsController _controller;
        private Mock<IGigRepository> _mockRepository;
        private string _userId;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IGigRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.Gigs).Returns(_mockRepository.Object);

            _userId = "1";
            _controller = new GigsController(mockUnitOfWork.Object);
            _controller.MockCurrentUser(_userId, "user1@email.com");
        }

        [TestMethod]
        public void Cancel_GigCanceled_ShouldReturnNotFound()
        {
            var gig = new Gig();
            gig.Cancel();

            _mockRepository.Setup(r => r.GetGigWithAttendances(1)).ReturnsAsync(gig);

            var result = _controller.Cancel(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_NoGigWithGivenIdExists_ShouldReturnNotFound()
        {
            var result = _controller.Cancel(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_UserCancelingAnotherGig_ShouldReturnUnauthorized()
        {
            var gig = new Gig
            {
                ArtistId = _userId + "-"
            };

            _mockRepository.Setup(r => r.GetGigWithAttendances(1)).ReturnsAsync(gig);

            var result = _controller.Cancel(1).Result;

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [TestMethod]
        public void Cancel_ValidRequest_ShouldReturnOk()
        {
            var gig = new Gig
            {
                ArtistId = _userId
            };

            _mockRepository.Setup(r => r.GetGigWithAttendances(1)).ReturnsAsync(gig);

            var result = _controller.Cancel(1).Result;

            result.Should().BeOfType<OkResult>();
        }
    }
}
