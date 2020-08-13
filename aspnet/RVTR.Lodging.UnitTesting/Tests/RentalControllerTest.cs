using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RVTR.Lodging.DataContext;
using RVTR.Lodging.DataContext.Repositories;
using RVTR.Lodging.ObjectModel.Models;
using RVTR.Lodging.WebApi.Controllers;
using Xunit;

namespace RVTR.Lodging.UnitTesting.Tests
{
  public class RentalControllerTest
  {
    private static readonly SqliteConnection _connection = new SqliteConnection("Data Source=:memory:");
    private static readonly DbContextOptions<LodgingContext> _options = new DbContextOptionsBuilder<LodgingContext>().UseSqlite(_connection).Options;
    private readonly RentalController _controller;
    private readonly ILogger<RentalController> _logger;
    private readonly UnitOfWork _unitOfWork;

    public RentalControllerTest()
    {
      var contextMock = new Mock<LodgingContext>(_options);
      var loggerMock = new Mock<ILogger<RentalController>>();
      var repositoryMock = new Mock<Repository<RentalModel>>(new LodgingContext(_options));
      var unitOfWorkMock = new Mock<UnitOfWork>(contextMock.Object);

      repositoryMock.Setup(m => m.DeleteAsync(0)).Throws(new Exception());
      repositoryMock.Setup(m => m.DeleteAsync(1)).Returns(Task.FromResult(1));
      repositoryMock.Setup(m => m.InsertAsync(It.IsAny<RentalModel>())).Returns(Task.FromResult<RentalModel>(null));
      repositoryMock.Setup(m => m.SelectAsync()).Returns(Task.FromResult<IEnumerable<RentalModel>>(null));
      repositoryMock.Setup(m => m.SelectAsync(0)).Throws(new Exception());
      repositoryMock.Setup(m => m.SelectAsync(1)).Returns(Task.FromResult<RentalModel>(null));
      repositoryMock.Setup(m => m.Update(It.IsAny<RentalModel>()));
      unitOfWorkMock.Setup(m => m.Rental).Returns(repositoryMock.Object);

      _logger = loggerMock.Object;
      _unitOfWork = unitOfWorkMock.Object;
      _controller = new RentalController(_logger, _unitOfWork);
    }

    [Fact]
    public async void Test_Controller_Delete()
    {
      var resultFail = await _controller.Delete(0);
      var resultPass = await _controller.Delete(1);

      Assert.NotNull(resultFail);
      Assert.NotNull(resultPass);
    }

    [Fact]
    public async void Test_Controller_Get()
    {
      var resultMany = await _controller.Get();
      var resultFail = await _controller.GetAllRentalsById(0);
      var resultOne = await _controller.GetAllRentalsById(1);

      Assert.NotNull(resultMany);
      Assert.NotNull(resultFail);
      Assert.NotNull(resultOne);
    }

    [Fact]
    public async void Test_Controller_Post()
    {
      var resultPass = await _controller.Post(new RentalModel());

      Assert.NotNull(resultPass);
    }

    [Fact]
    public async void Test_Controller_Put()
    {
      var resultPass = await _controller.Put(new RentalModel());

      Assert.NotNull(resultPass);
    }
  }
}
