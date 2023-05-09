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
    public class EventControllerTest
    {
        private EventController _eventController;
        private Mock<IEventRepository> _eventRepositoryMock = new Mock<IEventRepository>();
        private Mock<IVariantService> _variantServiceMock = new Mock<IVariantService>();
        private Mock<ILogger<EventController>> _loggerMock = new Mock<ILogger<EventController>>();

        [SetUp]
        public void Setup()
        {
            _eventController = new EventController(_loggerMock.Object, 
            new EventService(_eventRepositoryMock.Object,_variantServiceMock.Object));
        }

        [Test]
        public async Task Given_Events_When_GetAllEvents_Then_return_list_objects_events_Async()
        {
            // Arrange
            var messageReturn = FactoryEvent.ListSimpleEvent();
            _eventRepositoryMock.Setup(x => x.GetAllEvents<object>()).Returns(Task.FromResult(messageReturn as List<Event>)!);

            // Act
            var result = (await _eventController.GetAllEventsAsync() as OkObjectResult);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_Events_When_GetAllEvents_and_internal_error_Then_return_status_code_500_Async()
        {
            // Arrange
            var messageReturn = FactoryEvent.ListSimpleEvent();
            var expectedMessage = MessageLogErrors.GetAllEventMessage;
            _eventRepositoryMock.Setup(x => x.GetAllEvents<object>()).Throws(new Exception("error conection database"));

            // Act
            var result = (await _eventController.GetAllEventsAsync() as ObjectResult);

            // Assert
            Assert.AreEqual(500, result!.StatusCode);
            Assert.AreEqual(expectedMessage, result.Value);
        }

        [Test]
        public async Task Given_events_When_GetAllEvents_not_foud_register_Then_return_message_empty_list_Async()
        {
            // Arrange
            var expectedMessage = "Lista vazia";
            _eventRepositoryMock.Setup(x => x.GetAllEvents<object>()).Throws(new GetAllEventException(expectedMessage));

            // Act
            var result = (await _eventController.GetAllEventsAsync() as NoContentResult);

            // Assert
            Assert.AreEqual(204, result!.StatusCode);
        }

        [Test]
        public async Task Given_Events_When_GetWeeklyEvents_Then_return_list_objects_events_Async()
        {
            // Arrange
            IPagination pagination = new IPagination();
            pagination.page = 1;
            pagination.pageSize = 10;

            var messageReturn = FactoryEvent.ListSimpleEvent();
            _eventRepositoryMock.Setup(x => x.GetWeeklyEvents<object>(pagination)).Returns(Task.FromResult(messageReturn as List<Event>)!);

            // Act
            var result = (await _eventController.GetWeeklyEventsAsync(pagination) as OkObjectResult);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

                [Test]
        public async Task Given_Events_When_GetWeeklyEvents_and_internal_error_Then_return_status_code_500_Async()
        {
            IPagination pagination = new IPagination();
            pagination.page = 1;
            // Arrange
            var messageReturn = FactoryEvent.ListSimpleEvent();
            var expectedMessage = MessageLogErrors.GetAllEventMessage;
            _eventRepositoryMock.Setup(x => x.GetWeeklyEvents<object>(pagination)).Throws(new Exception("error coxcnection database"));

            // Act
            var result = (await _eventController.GetWeeklyEventsAsync(pagination) as ObjectResult);

            // Assert
            Assert.AreEqual(500, result!.StatusCode);
            Assert.AreEqual(expectedMessage, result.Value);
        }

        [Test]
        public async Task Given_events_When_GetWeeklyEvents_not_foud_register_Then_return_message_empty_list_Async()
        {
            // Arrange
            IPagination pagination = new IPagination();
            pagination.page = 100;
            pagination.pageSize = 10;

            var expectedMessage = "Lista vazia";
            _eventRepositoryMock.Setup(x => x.GetWeeklyEvents<object>(pagination)).Throws(new GetAllEventException(expectedMessage));

            // Act
            var result = (await _eventController.GetWeeklyEventsAsync(pagination) as NoContentResult);

            // Assert
            Assert.AreEqual(204, result!.StatusCode);
        }

        [Test]
        public async Task Given_id_event_When_FindById_Then_return_event_Async()
        {
            // Arrange
            var messageReturn = FactoryEvent.SimpleEvent();
            var id = "3b241101-e2bb-4255-8caf-4136c566a962";
            _eventRepositoryMock.Setup(x => x.FindById<Event>(id)).
                Returns(Task.FromResult(messageReturn as object));

            // Act
            var result = (await _eventController.FindByIdEventAsync(id) as OkObjectResult);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_id_event_When_FindById_and_has_internal_error_Then_return_status_code_500_Async()
        {
            // Arrange
            var id = "3b241101-e2bb-4255-8caf-4136c566a962";
            var espectedReturn = MessageLogErrors.FindByIdEventMessage;
            _eventRepositoryMock.Setup(x => x.FindById<object>(id)).
                Throws(new Exception("error conection database"));

            // Act
            var result = (await _eventController.FindByIdEventAsync(id) as ObjectResult);

            // Assert
            Assert.AreEqual(500, result!.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_id_event_When_FindByIdEvent_and_not_found_data_return_msg_empty_Async()
        {
            // Arrange
            var expectedMessage = "Sem registros";
            var id = "3b241101-e2bb-4255-8caf-4136c566a962";
            _eventRepositoryMock.Setup(x => x.FindById<Event>(id)).
                Throws(new FindByIdEventException(expectedMessage));

            // Act
            var result = (await _eventController.FindByIdEventAsync(id) as NoContentResult);

            // Assert
            Assert.AreEqual(204, result!.StatusCode);
        }

        [Test]
        public async Task Given_id_event_When_Delete_Then_return_message_deleted_Async()
        {
            // Arrange
            var messageReturn = "Evento deletado";
            var id = "3b241101-e2bb-4255-8caf-4136c566a962";
            _eventRepositoryMock.Setup(x => x.Delete<object>(id)).Returns(Task.FromResult(messageReturn as object));

            // Act
            var result = (await _eventController.DeleteEventAsync(id) as OkObjectResult);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_id_event_When_Delete_and_has_internal_error_Then_return_status_code_500_Async()
        {
            // Arrange
            var id = "3b241101-e2bb-4255-8caf-4136c566a962";
            var espectedReturn = MessageLogErrors.deleteEventMessage;
            _eventRepositoryMock.Setup(x => x.Delete<object>(id)).Throws(new Exception("error conection database"));

            // Act
            var result = (await _eventController.DeleteEventAsync(id) as ObjectResult);

            // Assert
            Assert.AreEqual(500, result!.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_id_event_When_delete_and_not_found_data_return_msg_empty_Async()
        {
            // Arrange
            var expectedMessage = "Sem registros";
            var id = "3b241101-e2bb-4255-8caf-4136c566a962";
            _eventRepositoryMock.Setup(x => x.Delete<object>(id)).Throws(new DeleteEventException(expectedMessage));

            // Act
            var result = (await _eventController.DeleteEventAsync(id) as NoContentResult);

            // Assert
            Assert.AreEqual(204, result!.StatusCode);
        }

        [Test]
        public async Task Given_event_When_Save_Then_return_message_created_Async()
        {
            // Arrange
            var messageReturn = "Evento criado";
            var eventSave = FactoryEvent.SimpleEvent();
            _eventRepositoryMock.Setup(x => x.Save<object>(eventSave))
                .Returns(Task.FromResult(messageReturn as object));
            _variantServiceMock.Setup(x => x.SaveAsync(It.IsAny<Variant>()))
                .Returns(Task.FromResult( new MessageReturn(){Data ="3b241101-e2bb-4255-8caf-4136c566a962"}));

            // Act
            var result = (await _eventController.SaveEventAsync(eventSave) as OkObjectResult);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_event_When_Save_and_has_internal_error_Then_return_status_code_500_Async()
        {
            // Arrange
            var eventSave = FactoryEvent.SimpleEvent();
            var espectedReturn = MessageLogErrors.saveEventMessage;
            _eventRepositoryMock.Setup(x => x.Save<object>(eventSave)).Throws(new Exception("error conection database"));

            // Act
            var result = (await _eventController.SaveEventAsync(eventSave) as ObjectResult);

            // Assert
            Assert.AreEqual(500, result!.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_event_When_save_and_not_found_data_return_msg_empty_Async()
        {
            // Arrange
            var expectedMessage = "Sem registros";
            var id = "3b241101-e2bb-4255-8caf-4136c566a962";
            _eventRepositoryMock.Setup(x => x.Delete<object>(id)).Throws(new DeleteEventException(expectedMessage));

            // Act
            var result = (await _eventController.DeleteEventAsync(id) as NoContentResult);

            // Assert
            Assert.AreEqual(204, result!.StatusCode);
        }
    }
}