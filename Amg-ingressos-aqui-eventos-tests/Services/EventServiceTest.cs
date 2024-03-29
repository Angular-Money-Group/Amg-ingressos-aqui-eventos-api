using Amg_ingressos_aqui_eventos_tests.FactoryServices;
using Amg_ingressos_aqui_eventos_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Amg_ingressos_aqui_eventos_api.Dto;

namespace Prime.UnitTests.Services
{
    public class EventServiceTest
    {
        private EventService _eventService;
        private Mock<IEventRepository> _eventRepositoryMock = new Mock<IEventRepository>();
        private readonly Mock<IVariantService> _variantServiceMock = new Mock<IVariantService>();
        private readonly Mock<ILogger<EventService>> _loggerMock = new Mock<ILogger<EventService>>();
        private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

        [SetUp]
        public void SetUp()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _eventService = new EventService(_eventRepositoryMock.Object,_variantServiceMock.Object, _webHostEnvironmentMock.Object,_loggerMock.Object);
        }

        [Test]
        public void Given_event_without_name_When_save_Then_return_message_miss_name()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.Name = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Nome é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_local_When_save_Then_return_message_miss_local()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.Local = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Local é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_type_When_save_Then_return_message_miss_type()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.Type = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Tipo é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_image_When_save_Then_return_message_miss_image()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.Image = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Imagem é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_description_When_save_Then_return_message_miss_description()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.Description = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Descrição é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_cep_When_save_Then_return_message_miss_cep()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.Address.Cep = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "CEP é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_address_When_save_Then_return_message_miss_address()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.Address = new Address(){};
            var expectedMessage = new MessageReturn() { Message = "Endereço é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_number_When_save_Then_return_message_miss_number()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.Address.Number = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Número Endereço é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_neighborhood_When_save_Then_return_message_miss_neighborhood()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.Address.Neighborhood = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Vizinhança é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_city_When_save_Then_return_message_miss_city()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.Address.City = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Cidade é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_state_When_save_Then_return_message_miss_state()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();

            eventComplet.Address.State = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Estado é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_startDate_When_save_Then_return_message_miss_startDate()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.StartDate = DateTime.MinValue;
            var expectedMessage = new MessageReturn() { Message = "Data Inicio é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_endDate_When_save_Then_return_message_miss_endDate()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.EndDate = DateTime.MinValue;
            var expectedMessage = new MessageReturn() { Message = "Data Fim é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        

        [Test]
        public void Given_event_without_variant_When_save_Then_return_message_miss_Variant()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.Variants = new List<VariantWithLotDto>();
            var expectedMessage = new MessageReturn() { Message = "Variante é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_id_event_When_findById_Then_return_objectEvent()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            var id = "3b241101-e2bb-4255-8caf-4136c566a962";
            _eventRepositoryMock.Setup(x => x.GetById<EventComplet>(id))
            .Returns(Task.FromResult(new EventComplet()));

            //Act
            var result = _eventService.GetByIdAsync(id);

            //Assert
            Assert.AreEqual(eventComplet, result.Result.Data);
        }

        [Test]
        public void Given_id_event_without_24_digits_When_findById_Then_return_objectEvent()
        {
            //Arrange
            var messageExpected = "Id é obrigatório e está menor que 24 digitos";

            //Act
            var result = _eventService.GetByIdAsync("3b241101-e2bb-4255-8caf");

            //Assert
            Assert.AreEqual(messageExpected, result.Result.Message);
        }

        [Test]
        public void Given_id_event_is_empty_When_findById_Then_return_message_id_is_necessary()
        {
            //Arrange
            var messageExpected = "Id é obrigatório";

            //Act
            var result = _eventService.GetByIdAsync("");

            //Assert
            Assert.AreEqual(messageExpected, result.Result.Message);
        }

        [Test]
        public void Given_id_event_When_Delete_Then_return_message_Ok()
        {
            //Arrange
            var messageReturn = "Evento deletado";
            var id = "3b241101-e2bb-4255-8caf-4136c566a962";
            _eventRepositoryMock.Setup(x => x.Delete(id)).Returns(Task.FromResult(true));

            //Act
            var result = _eventService.DeleteAsync(id);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Data);
        }

        [Test]
        public void Given_id_event_is_0_When_Delete_Then_return_message_id_is_necessary()
        {
            //Arrange
            var messageExpected = "Id é obrigatório";

            //Act
            var result = _eventService.DeleteAsync("");

            //Assert
            Assert.AreEqual(messageExpected, result.Result.Message);
        }

        [Test]
        public void Given_Events_When_GetAllEvents_Then_return_list_objects_events()
        {
            //Arrange
            var messageReturn = FactoryEvent.ListSimpleEventWithNames();
            Pagination pagination = new Pagination(){
                Page = 1,
                PageSize = 10
            };
            _eventRepositoryMock.Setup(x => x.GetByFilterComplet<EventComplet>(pagination,new Event())).Returns(Task.FromResult(new List<EventComplet>()));

            //Act
            var resultTask = _eventService.GetEventsAsync(new FilterOptions(), pagination);

            //Assert
            Assert.AreEqual(messageReturn, resultTask.Result.Data);
        }

        [Test]
        public void Given_complet_event_When_save_Then_return_Internal_error()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            _eventRepositoryMock.Setup(x => x.Save(eventComplet)).
                Throws(new Exception("Erro ao conectar a base de dados"));
            _variantServiceMock.Setup(x => x.SaveAsync(It.IsAny<VariantWithLotDto>()))
                .Returns(Task.FromResult( new MessageReturn(){Data ="3b241101-e2bb-4255-8caf-4136c566a962"}));

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.IsNotEmpty(resultMethod.Exception?.Message);
        }
    }
}