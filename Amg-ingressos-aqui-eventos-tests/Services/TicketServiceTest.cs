using NUnit.Framework;
using Moq;
using System.Text.Json;
using Amg_ingressos_aqui_eventos_api.Services;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_tests.FactoryServices;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Prime.UnitTests.Services
{
    public class TicketServiceTest
    {
        private TicketService _ticketService;
        private Mock<ITicketRepository> _ticketRepositoryMock = new Mock<ITicketRepository>();

        [SetUp]
        public void SetUp()
        {
            _ticketRepositoryMock = new Mock<ITicketRepository>();
            _ticketService = new TicketService(_ticketRepositoryMock.Object);
        }

        [Test]
        public void Given_complet_ticket_When_save_Then_return_Ok()
        {
            //Arrange
            var ticketComplet = FactoryTicket.SimpleTicket();
            var messageReturn = "OK";
            _ticketRepositoryMock.Setup(x => x.Save<object>(ticketComplet))
                .Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _ticketService.SaveAsync(ticketComplet);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Data);
        }

        [Test]
        public void Given_ticket_without_Value_When_save_Then_return_message_miss_Value()
        {
            //Arrange
            var ticketComplet = FactoryTicket.SimpleTicket();
            ticketComplet.Value = 0;
            var expectedMessage = new MessageReturn() { Message = "Valor do Ingresso é Obrigatório." };

            //Act
            var resultMethod = _ticketService.SaveAsync(ticketComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_ticket_without_IdLote_When_save_Then_return_message_miss_IdLote()
        {
            //Arrange
            var ticketComplet = FactoryTicket.SimpleTicket();
            ticketComplet.IdLote = string.Empty;
            var expectedMessage = new MessageReturn() { 
                Message = "Id é obrigatório" };

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
            _ticketRepositoryMock.Setup(x => x.Save<object>(ticket)).
                Throws(new Exception("Erro ao conectar a base de dados"));

            //Act
            var resultMethod = _ticketService.SaveAsync(ticket);

            //Assert
            Assert.IsNotEmpty(resultMethod.Exception.Message);
        }
    }
}