using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HotelBookingAPI.Configuration;
using HotelBookingAPI.Controllers;
using Moq;
using System.Threading.Tasks;
using Xunit;
using HotelBookingAPI.Repository;
using HotelBookingUnitTest.Fixture;
using System.Collections.Generic;
using HotelBookingAPI.Data;
using HotelBookingAPI.Models;

namespace HotelBookingUnitTest.System.Controllers
{
    public class TestBookingController
    {
        [Fact]
        public async Task GetBookings_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange
            var logger = Mock.Of<ILogger<BookingController>>();
            //mapperConfiguration
            var myProfile = new AutoMapperConfiguration();
            var configuration = new MapperConfiguration(mapper => mapper.AddProfile(myProfile));
            var mapper = new Mapper(configuration);
            var mockRepository = new Mock<IBookingRepository>();
            mockRepository.Setup(repository => repository.GetAll(null, null, null)).ReturnsAsync(BookingModelFixture.GetBookings());
            var contrBook = new BookingController(logger, mockRepository.Object, mapper);            
            // Act
            var result = (OkObjectResult)(await contrBook.GetBookings());

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetBookings_OnSuccess_InvokeBookingRepositoryExactlyOnce()
        {
            // Arrange
            var logger = Mock.Of<ILogger<BookingController>>();
            //mapperConfiguration
            var myProfile = new AutoMapperConfiguration();
            var configuration = new MapperConfiguration(mapper => mapper.AddProfile(myProfile));
            var mapper = new Mapper(configuration);
            var mockRepositoy = new Mock<IBookingRepository>();
            mockRepositoy.Setup(repository => repository.GetAll(null, null, null)).ReturnsAsync(BookingModelFixture.GetBookings());
            var contrBook = new BookingController(logger, mockRepositoy.Object, mapper);
            // Act
            _ = (OkObjectResult)await contrBook.GetBookings();

            // Assert
            mockRepositoy.Verify(repository => repository.GetAll(null, null, null), Times.Once);
        }

        [Fact]
        public async Task GetBookings_OnSuccess_ReturnsListOfBookingDTO()
        {
            // Arrange
            var logger = Mock.Of<ILogger<BookingController>>();
            //mapperConfiguration
            var myProfile = new AutoMapperConfiguration();
            var configuration = new MapperConfiguration(mapper => mapper.AddProfile(myProfile));
            var mapper = new Mapper(configuration);
            var mockRepository = new Mock<IBookingRepository>();
            mockRepository.Setup(repository => repository.GetAll(null, null, null)).ReturnsAsync(BookingModelFixture.GetBookings());
            var contrBook = new BookingController(logger, mockRepository.Object, mapper);
            // Act
            var result = (OkObjectResult)(await contrBook.GetBookings());

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.Value.Should().BeOfType<List<BookingDTO>>();
        }

        [Fact]
        public async Task GetBookings_OnNoBookingFound_Returns404()
        {
            // Arrange
            var logger = Mock.Of<ILogger<BookingController>>();
            //mapperConfiguration
            var myProfile = new AutoMapperConfiguration();
            var configuration = new MapperConfiguration(mapper => mapper.AddProfile(myProfile));
            var mapper = new Mapper(configuration);
            var mockRepository = new Mock<IBookingRepository>();
            mockRepository.Setup(repository => repository.GetAll(null, null, null)).ReturnsAsync(new List<BookingDTO>());
            var contrBook = new BookingController(logger, mockRepository.Object, mapper);
            // Act
            var result = await contrBook.GetBookings();


            // Assert
            result.Should().BeOfType<NotFoundResult>();
            var objectResult = (NotFoundResult)result;
            objectResult.StatusCode.Should().Be(404);
        }
    }
}
