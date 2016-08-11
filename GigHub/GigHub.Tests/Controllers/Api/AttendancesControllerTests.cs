using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GigHub.Core.Repositories;
using Moq;
using GigHub.Core;
using GigHub.Controllers;
using GigHub.Controllers.Api;
using GigHub.Tests.Extensions;
using GigHub.Core.Models;
using GigHub.Core.Dtos;
using FluentAssertions;
using System.Web.Http.Results;

namespace GigHub.Tests.Controllers.Api
{
    [TestClass]
    public class AttendancesControllerTests
    {
        private Mock<IAttendanceRepository> _mockRepository;
        private string _userId;
        private AttendancesController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IAttendanceRepository>();

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.Attendances).Returns(_mockRepository.Object);

            _userId = "1";
            _controller = new AttendancesController(mockUnitOfWork.Object);
            _controller.MockCurrentUser(_userId, "user1@email.com");
        }

        [TestMethod]
        public void Attend_AttendanceAlreadyExist_ShouldReturnBadRequest()
        {
            var attendance = new Attendance();

            _mockRepository.Setup(r => r.GetAttendance(1, _userId)).ReturnsAsync(attendance);

            var dto = new AttendanceDto { GigId = 1 };

            var result = _controller.Attend(dto).Result;

            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public void Attend_ValidRequest_ShouldReturnOk()
        {
            var dto = new AttendanceDto { GigId = 1 };

            var result = _controller.Attend(dto).Result;

            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public void DeleteAttendance_GigNotExist_ShouldReturnNotFound()
        {
            var result = _controller.DeleteAttendance(1).Result;

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void DeleteAttendance_ValidRequest_ShouldReturnOk()
        {
            var attendance = new Attendance();

            _mockRepository.Setup(r => r.GetAttendance(1, _userId)).ReturnsAsync(attendance);

            var result = _controller.DeleteAttendance(1).Result;

            result.Should().BeOfType<OkNegotiatedContentResult<int>>();
        }

        [TestMethod]
        public void DeleteAttendance_ValidRequest_ShouldReturnIdOfDeleteAttendance()
        {
            var attendance = new Attendance();

            _mockRepository.Setup(r => r.GetAttendance(1, _userId)).ReturnsAsync(attendance);

            var result = _controller.DeleteAttendance(1).Result as OkNegotiatedContentResult<int>;

            result.Content.Should().Be(1);
        }
    }
}
