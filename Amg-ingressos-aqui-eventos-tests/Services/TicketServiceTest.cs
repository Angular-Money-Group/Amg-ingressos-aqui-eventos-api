using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_eventos_api.Services;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_tests.FactoryServices;
using Amg_ingressos_aqui_eventos_api.Model;
using Microsoft.Extensions.Logging;

namespace Prime.UnitTests.Services
{
    public class TicketServiceTest
    {
        private TicketService _ticketService;
        private Mock<ITicketRepository> _ticketRepositoryMock = new Mock<ITicketRepository>();
        private Mock<ITicketRowRepository> _ticketRowRepositoryMock =
            new Mock<ITicketRowRepository>();
        private readonly Mock<IVariantRepository> _variantRepositoryMock = new Mock<IVariantRepository>();
        private readonly Mock<IEventRepository> _eventRepositoryMock = new Mock<IEventRepository>();
        private readonly Mock<IEmailService> _emailRepositoryMock = new Mock<IEmailService>();
        private readonly Mock<ILotRepository> _lotRepositoryMock = new Mock<ILotRepository>();
        private readonly Mock<ILogger<TicketService>> _loggerMock = new Mock<ILogger<TicketService>>();

        [SetUp]
        public void SetUp()
        {
            _ticketRepositoryMock = new Mock<ITicketRepository>();
            _ticketRowRepositoryMock = new Mock<ITicketRowRepository>();
            _ticketService = new TicketService(
                _ticketRepositoryMock.Object,
                _ticketRowRepositoryMock.Object,
                _variantRepositoryMock.Object,
                _lotRepositoryMock.Object,
                _emailRepositoryMock.Object,
                _eventRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Test]
        public void Given_complet_ticket_When_save_Then_return_Ok()
        {
            //Arrange
            var ticketComplet = FactoryTicket.SimpleTicket();
            _ticketRepositoryMock
                .Setup(x => x.Save(ticketComplet))
                .Returns(Task.FromResult(new Ticket()));

            //Act
            var resultMethod = _ticketService.SaveAsync(ticketComplet);

            //Assert
            Assert.AreEqual(ticketComplet, resultMethod.Result.Data);
        }

        [Test]
        public void Given_ticket_without_Value_When_save_Then_return_message_miss_Value()
        {
            //Arrange
            var ticketComplet = FactoryTicket.SimpleTicket();
            ticketComplet.Value = 0;
            var expectedMessage = new MessageReturn()
            {
                Message = "Valor do Ingresso é Obrigatório."
            };

            //Act
            var resultMethod = _ticketService.SaveAsync(ticketComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_ticket_without_IdLot_When_save_Then_return_message_miss_IdLot()
        {
            //Arrange
            var ticketComplet = FactoryTicket.SimpleTicket();
            ticketComplet.IdLot = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Id é obrigatório" };

            //Act
            var resultMethod = _ticketService.SaveAsync(ticketComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_complet_event_When_save_Then_return_Internal_error()
        {
            //Arrange
            var ticket = FactoryTicket.SimpleTicket();
            _ticketRepositoryMock
                .Setup(x => x.Save(ticket))
                .Throws(new Exception("Erro ao conectar a base de dados"));

            //Act
            var resultMethod = _ticketService.SaveAsync(ticket);

            //Assert
            Assert.IsNotEmpty(resultMethod?.Exception?.Message);
        }
    }
}