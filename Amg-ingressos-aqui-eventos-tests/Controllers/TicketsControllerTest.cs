using Amg_ingressos_aqui_eventos_api.Controllers;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using Amg_ingressos_aqui_eventos_tests.FactoryServices;
using Amg_ingressos_aqui_eventos_api.Services;
using Amg_ingressos_aqui_eventos_api.Model;
using Microsoft.AspNetCore.Mvc;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;

namespace Amg_ingressos_aqui_eventos_tests.Controllers
{
    public class TicketsControllerTest
    {
        private TicketController _ticketController;
        private Mock<IEventRepository> _eventRepositoryMock = new Mock<IEventRepository>();
        private Mock<ITicketRepository> _ticketRepositoryMock = new Mock<ITicketRepository>();
        private Mock<IVariantService> _variantServiceMock = new Mock<IVariantService>();
        private Mock<ILogger<TicketController>> _loggerMock = new Mock<ILogger<TicketController>>();
        private Mock<ILogger<TicketController>> _ticketMock = new Mock<ILogger<TicketController>>();

        [SetUp]
        public void Setup()
        {
            _ticketController = new TicketController(_loggerMock.Object, 
            new EventService(_eventRepositoryMock.Object,_variantServiceMock.Object), 
            new TicketService(_ticketRepositoryMock.Object));
        }

    [Test]
    public async Task Given_ValidUserID_When_GetUserTickets_Then_Return_Tickets_Async()
    {
        // Arrange
        var userID = "644178cb940d123bafb3a4ae";
        var messageReturn = FactoryTicket.ListSimpleTicket();
        _ticketRepositoryMock.Setup(x => x.GetUserTickets<object>(userID)).Returns(Task.FromResult(messageReturn as List<Ticket>));

        // Act
        var result = await _ticketController.GetTicketByUser(userID);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.AreEqual(messageReturn, okResult.Value);
    }

    [Test]
    public async Task Given_NoTicketsForUser_When_GetUserTickets_Then_Return_NoContent_Async()
    {
        // Arrange
        var userID = "34334";
        var expectedMessage = "Sem registros";
        _ticketRepositoryMock.Setup(x => x.GetUserTickets<List<Ticket>>(userID)).ReturnsAsync(new List<Ticket>());

        // Act
        var result = await _ticketController.GetTicketByUser(userID);

        // Assert
        Assert.IsInstanceOf<NoContentResult>(result);
        var noContentResult = result as NoContentResult;
        Assert.AreEqual(204, noContentResult.StatusCode);
    }

    [Test]
    public async Task Given_InvalidUserID_When_GetUserTickets_Then_Return_BadRequest_Async()
    {
        // Arrange
        var userID = "";
        var expectedMessage = "ID de usuário inválido";
        _ticketRepositoryMock.Setup(x => x.GetUserTickets<List<Ticket>>(userID)).ThrowsAsync(new ArgumentException(expectedMessage));

        // Act
        var result = await _ticketController.GetTicketByUser(userID);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var badRequestResult = result as BadRequestObjectResult;
        Assert.AreEqual(expectedMessage, badRequestResult.Value);
    }

    [Test]
    public async Task Given_InternalServerError_When_GetUserTickets_Then_Return_InternalServerError_Async()
    {
        // Arrange
        var userID = "644178cb940d123bafb3a4ae";
        var expectedMessage = "Erro interno do servidor";
        _ticketRepositoryMock.Setup(x => x.GetUserTickets<List<Ticket>>(userID)).ThrowsAsync(new Exception(expectedMessage));

        // Act
        var result = await _ticketController.GetTicketByUser(userID);

        // Assert
        Assert.IsInstanceOf<StatusCodeResult>(result);
        var statusCodeResult = result as StatusCodeResult;
        Assert.AreEqual(500, statusCodeResult.StatusCode);
    }

        
    }
}