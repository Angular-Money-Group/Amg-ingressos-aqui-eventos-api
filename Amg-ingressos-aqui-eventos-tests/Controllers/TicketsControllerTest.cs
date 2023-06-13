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
using Microsoft.AspNetCore.Hosting;

namespace Amg_ingressos_aqui_eventos_tests.Controllers
{
    [TestFixture]
    public class TicketsControllerTest
    {
        private TicketController _ticketController;
        private Mock<IEventRepository> _eventRepositoryMock = new Mock<IEventRepository>();
        private Mock<HttpClient> _clientMock = new Mock<HttpClient>();

        private Mock<ITicketRepository> _ticketRepositoryMock = new Mock<ITicketRepository>();
        private Mock<IVariantService> _variantServiceMock = new Mock<IVariantService>();
        private Mock<ILogger<TicketController>> _loggerMock = new Mock<ILogger<TicketController>>();
        private Mock<ILogger<TicketController>> _ticketMock = new Mock<ILogger<TicketController>>();
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

        [SetUp]
        public void Setup()
        {
            _ticketController = new TicketController(_loggerMock.Object,
            new EventService(_eventRepositoryMock.Object, _variantServiceMock.Object, _webHostEnvironmentMock.Object, _clientMock.Object),
            new TicketService(_ticketRepositoryMock.Object));
        }

        [Test]
        public async Task Given_ValidUserID_When_GetUserTickets_Then_Return_Tickets_Async()
        {
            // Arrange
            var userID = "644178cb940d123bafb3a4ae";
            var messageReturn = FactoryTicket.ListSimpleTicket();
            _ticketRepositoryMock.Setup(x => x.GetTickets<object>(It.IsAny<Ticket>()))
                .Returns(Task.FromResult(messageReturn as List<Ticket>)!);

            // Act
            var result = await _ticketController.GetTicketByUser(userID);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(messageReturn, okResult?.Value);
        }

        [Test]
        public async Task Given_ValidLotID_When_GetTicketsRemeaning_Then_Return_List_Tickets_Async()
        {
            // Arrange
            var lotID = "6451b37d90737f442d2b357a";
            var messageReturn = FactoryTicket.ListSimpleTicketWithoutIdUser();
            _ticketRepositoryMock.Setup(x => x.GetTickets<object>(It.IsAny<Ticket>()))
                .Returns(Task.FromResult(messageReturn as List<Ticket>)!);

            // Act
            var result = await _ticketController.GetTicketsRemainingByLot(lotID);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(messageReturn, okResult?.Value);
        }
    }
}