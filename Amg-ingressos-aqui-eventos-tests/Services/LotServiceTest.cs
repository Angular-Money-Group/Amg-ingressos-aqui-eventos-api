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
    public class LotServiceTest
    {
        private LotService _lotService;
        private Mock<ILotRepository> _lotRepositoryMock = new Mock<ILotRepository>();
        private Mock<ITicketService> _ticketServiceMock = new Mock<ITicketService>();

        [SetUp]
        public void SetUp()
        {
            _lotRepositoryMock = new Mock<ILotRepository>();
            _lotService = new LotService(_lotRepositoryMock.Object,_ticketServiceMock.Object);
        }

        [Test]
        public void Given_complet_lot_When_save_Then_return_Ok()
        {
            //Arrange
            var lotComplet = FactoryLot.SimpleLot();
            var messageReturn = "OK";
            _lotRepositoryMock.Setup(x => x.Save<object>(lotComplet)).Returns(Task.FromResult(messageReturn as object));
            _ticketServiceMock.Setup(x => x.SaveAsync(It.IsAny<Ticket>()))
                .Returns(Task.FromResult( new MessageReturn(){Data ="3b241101-e2bb-4255-8caf-4136c566a962"}));

            //Act
            var resultMethod = _lotService.SaveAsync(lotComplet);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Data);
        }

        [Test]
        public void Given_lot_without_Description_When_save_Then_return_message_miss_Description()
        {
            //Arrange
            var lotComplet = FactoryLot.SimpleLot();
            lotComplet.Identificador = 0;
            var expectedMessage = new MessageReturn() { Message = "Identificador é Obrigatório." };

            //Act
            var resultMethod = _lotService.SaveAsync(lotComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_lot_without_startDate_When_save_Then_return_message_miss_startDate()
        {
            //Arrange
            var lotComplet = FactoryLot.SimpleLot();
            lotComplet.StartDateSales = DateTime.MinValue;
            var expectedMessage = new MessageReturn() { Message = "Data Inicio de venda é Obrigatório." };

            //Act
            var resultMethod = _lotService.SaveAsync(lotComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_lot_without_endDate_When_save_Then_return_message_miss_endDate()
        {
            //Arrange
            var lotComplet = FactoryLot.SimpleLot();
            lotComplet.EndDateSales = DateTime.MinValue;
            var expectedMessage = new MessageReturn() { Message = "Data final de venda é Obrigatório." };

            //Act
            var resultMethod = _lotService.SaveAsync(lotComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_complet_event_When_save_Then_return_Internal_error()
        {
            //Arrange
            var lot = FactoryLot.SimpleLot();
            _lotRepositoryMock.Setup(x => x.Save<object>(lot)).
                Throws(new Exception("Erro ao conectar a base de dados"));
            _ticketServiceMock.Setup(x => x.SaveAsync(It.IsAny<Ticket>()))
                .Returns(Task.FromResult( new MessageReturn(){Data ="3b241101-e2bb-4255-8caf-4136c566a962"}));

            //Act
            var resultMethod = _lotService.SaveAsync(lot);

            //Assert
            Assert.IsNotEmpty(resultMethod.Exception.Message);
        }
    }
}