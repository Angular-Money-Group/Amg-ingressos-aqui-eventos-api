using Amg_ingressos_aqui_eventos_api.Controllers;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using Amg_ingressos_aqui_eventos_tests.FactoryServices;
using Amg_ingressos_aqui_eventos_api.Services;
using Amg_ingressos_aqui_eventos_api.Model;
using Microsoft.AspNetCore.Mvc;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Infra;

namespace Amg_ingressos_aqui_eventos_tests.Controllers
{
    [TestFixture]
    public class TicketsControllerTest
    {
        private TicketController _ticketController;
        private readonly Mock<IEventRepository> _eventRepositoryMock = new Mock<IEventRepository>();
        private readonly Mock<ITicketRepository> _ticketRepositoryMock = new Mock<ITicketRepository>();
        private readonly Mock<ITicketRowRepository> _ticketRowRepositoryMock = new Mock<ITicketRowRepository>();
        private readonly Mock<IEmailService> _emailRepositoryMock = new Mock<IEmailService>();
        private readonly Mock<ILotRepository> _lotRepositoryMock = new Mock<ILotRepository>();
        private readonly Mock<IVariantRepository> _variantRepositoryMock = new Mock<IVariantRepository>();
        private readonly Mock<ILogger<TicketService>> _loggerServiceMock = new Mock<ILogger<TicketService>>();
        private readonly Mock<ILogger<TicketController>> _loggerMock = new Mock<ILogger<TicketController>>();

        [SetUp]
        public void Setup()
        {
            _ticketController = new TicketController(
                _loggerMock.Object,
                new TicketService(
                    _ticketRepositoryMock.Object,
                    _ticketRowRepositoryMock.Object,
                    _variantRepositoryMock.Object,
                    _lotRepositoryMock.Object,
                    _emailRepositoryMock.Object,
                    _eventRepositoryMock.Object,
                    _loggerServiceMock.Object
                )
            );
        }

        [Test]
        public async Task Given_ValidUserID_When_GetUserTickets_Then_Return_Tickets_Async()
        {
            // Arrange
            var userID = "644178cb940d123bafb3a4ae";
            var messageReturn = FactoryTicket.ListSimpleTicket();
            _ticketRepositoryMock
                .Setup(x => x.GetTickets<Ticket>(It.IsAny<Ticket>()))
                .Returns(Task.FromResult(messageReturn as List<Ticket>)!);

            // Act
            var result = await _ticketController.GetByUser(userID);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(messageReturn, okResult?.Value);
        }

        [Test]
        public async Task Given_ValidLotID_When_GetTicketsRemeaning_Then_Return_List_Tickets_Async()
        {
            // Arrange
            var lotID = "6451b37d90737f442d2b357a";
            var messageReturn = FactoryTicket.ListSimpleTicketWithoutIdUser();
            _ticketRepositoryMock
                .Setup(x => x.GetTickets<Ticket>(It.IsAny<Ticket>()))
                .Returns(Task.FromResult(messageReturn as List<Ticket>)!);

            // Act
            var result = await _ticketController.GetRemainingByLot(lotID);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(messageReturn, okResult?.Value);
        }
    }
}