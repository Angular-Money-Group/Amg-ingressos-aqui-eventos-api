using Amg_ingressos_aqui_eventos_tests.FactoryServices;
using Amg_ingressos_aqui_eventos_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Model;

namespace Prime.UnitTests.Services
{
    public class EventServiceTest
    {
        private EventService _eventService;
        private Mock<IEventRepository> _eventServiceMock = new Mock<IEventRepository>();

        [SetUp]
        public void SetUp()
        {
            _eventServiceMock = new Mock<IEventRepository>();
            _eventService = new EventService(_eventServiceMock.Object);
        }

        [Test]
        public void Given_complet_event_When_save_Then_return_Ok()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            var messageReturn = "OK";
            _eventServiceMock.Setup(x => x.Save<object>(eventComplet)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Data);
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
            eventComplet.Cep = string.Empty;
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
            eventComplet.Address = string.Empty;
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
            eventComplet.Number = 0;
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
            eventComplet.Neighborhood = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Vizinhança é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_complement_When_save_Then_return_message_miss_complement()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.Complement = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Complemento é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_referencePoint_When_save_Then_return_message_miss_referencePoint()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.ReferencePoint = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Ponto de referência é Obrigatório." };

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
            eventComplet.City = string.Empty;
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
            eventComplet.State = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Estado é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_days_When_save_Then_return_message_miss_days()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.Day = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Dia é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_lote_When_save_Then_return_message_miss_lote()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.Lot = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Lote é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_event_without_VipArea_When_save_Then_return_message_miss_VipArea()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            eventComplet.VipArea = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Área Vip é Obrigatório." };

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_complet_event_When_save_has_error_null_in_repository_Then_return_message_error_internal()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            MessageReturn? messageReturn = null;
            _eventServiceMock.Setup(x => x.Save<MessageReturn>(eventComplet)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _eventService.SaveAsync(eventComplet);

            //Assert
            Assert.AreEqual("", resultMethod.Result.Message);
        }

        [Test]
        public void Given_id_event_When_findById_Then_return_objectEvent()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            var id = "3b241101-e2bb-4255-8caf-4136c566a962";
            _eventServiceMock.Setup(x => x.FindById<Event>(id)).Returns(Task.FromResult(eventComplet as object));

            //Act
            var result = _eventService.FindByIdAsync(id);

            //Assert
            Assert.AreEqual(eventComplet, result.Result.Data);
        }

        [Test]
        public void Given_id_event_without_24_digits_When_findById_Then_return_objectEvent()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            var messageExpected = "Id é obrigatório e está menor que 24 digitos";

            //Act
            var result = _eventService.FindByIdAsync("3b241101-e2bb-4255-8caf");

            //Assert
            Assert.AreEqual(messageExpected, result.Result.Message);
        }

        [Test]
        public void Given_id_event_is_empty_When_findById_Then_return_message_id_is_necessary()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
            var messageExpected = "Id é obrigatório";

            //Act
            var result = _eventService.FindByIdAsync("");

            //Assert
            Assert.AreEqual(messageExpected, result.Result.Message);
        }

        [Test]
        public void Given_id_event_When_Delete_Then_return_message_Ok()
        {
            //Arrange
            var messageReturn = "Evento deletado";
            var id = "3b241101-e2bb-4255-8caf-4136c566a962";
            _eventServiceMock.Setup(x => x.Delete<object>(id)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var result = _eventService.DeleteAsync(id);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Data);
        }

        [Test]
        public void Given_id_event_is_0_When_Delete_Then_return_message_id_is_necessary()
        {
            //Arrange
            var eventComplet = FactoryEvent.SimpleEvent();
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
            var messageReturn = FactoryEvent.ListSimpleEvent();
            _eventServiceMock.Setup(x => x.GetAllEvents<object>()).Returns(Task.FromResult(messageReturn as IEnumerable<object>));

            //Act
            var resultTask = _eventService.GetAllEventsAsync();

            //Assert
            Assert.AreEqual(messageReturn, resultTask.Result.Data);
        }
    }
}