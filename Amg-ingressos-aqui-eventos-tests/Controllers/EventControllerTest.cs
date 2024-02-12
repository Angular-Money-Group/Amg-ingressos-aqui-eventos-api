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
using Microsoft.AspNetCore.Hosting;
using Amg_ingressos_aqui_eventos_api.Dto;

namespace Amg_ingressos_aqui_eventos_tests.Controllers
{
    public class EventControllerTest
    {
        private EventController _eventController;
        private readonly Mock<IEventRepository> _eventRepositoryMock = new Mock<IEventRepository>();
        private readonly Mock<IVariantService> _variantServiceMock = new Mock<IVariantService>();
        private readonly Mock<ILogger<EventController>> _loggerMock = new Mock<ILogger<EventController>>();
        private readonly Mock<ILogger<EventService>> _loggerServiceMock = new Mock<ILogger<EventService>>();
        private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

        [SetUp]
        public void Setup()
        {
            _eventController = 
            new EventController(_loggerMock.Object,
                new EventService(
                    _eventRepositoryMock.Object,
                    _variantServiceMock.Object, 
                    _webHostEnvironmentMock.Object,
                    _loggerServiceMock.Object)
            );
        }

        [Test]
        public async Task Given_Events_When_GetAllEvents_Then_return_list_objects_events_Async()
        {
            // Arrange
            Pagination pagination = new Pagination(){
                Page = 1,
                PageSize = 10
            };

            var messageReturn = FactoryEvent.ListSimpleEventWithNames();
            _eventRepositoryMock.Setup(x => x.GetByFilterComplet<EventComplet>(pagination,new Event())).Returns(Task.FromResult(new List<EventComplet>()));

            // Act
            var result = (OkObjectResult)await _eventController.GetEventsAsync(new FilterOptions(), pagination);

            // Assert
            Assert.AreEqual(messageReturn, result.Value);
        }

        [Test]
        public async Task Given_Events_When_HighlightEventAsync_Then_return_edited_event_Async()
        {

            var messageReturn = FactoryEvent.SimpleEvent();

            var id = "6462946db12323abeff9253f";

            _eventRepositoryMock.Setup(x => x.SetHighlightEvent(id)).Returns(Task.FromResult(true));

            // Act
            var result = (OkObjectResult)await _eventController.SetHighlightEventAsync(id);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_Events_When_SetHighlightEvent_Then_Return_400_When_Max_Highlighted_Flag_Toggled()
        {
            var expectedMessage = "Maximo de Eventos destacados atingido";

            var id = "6462946db12323abeff9253f";
            _eventRepositoryMock.Setup(x => x.SetHighlightEvent(id)).ThrowsAsync(new GetException("Maximo de Eventos destacados atingido"));

            // Act
            var result = (ObjectResult)await _eventController.SetHighlightEventAsync(id);

            // Assert
            Assert.AreEqual(400, result?.StatusCode);
            Assert.AreEqual(expectedMessage, result?.Value);
        }

        [Test]
        public async Task Given_Events_When_HighlightEventAsync_Then_return_return_status_code_500_Async()
        {

            var expectedMessage = MessageLogErrors.EditController;

            var id = "6462946db12323abeff9253f";
            _eventRepositoryMock.Setup(x => x.SetHighlightEvent(id)).ThrowsAsync(new Exception("error conection database"));

            // Act
            var result = (ObjectResult)await _eventController.SetHighlightEventAsync(id);

            // Assert
            Assert.AreEqual(500, result?.StatusCode);
            Assert.AreEqual(expectedMessage, result?.Value);
        }

        [Test]
        public async Task Given_Events_When_FindEventsByName_Then_return_list_objects_events_Async()
        {
            // Arrange
            var name = "CBLOL";
            var dic = new Dictionary<string,object>(){{"Name",name}};
            var messageReturn = FactoryEvent.ListSimpleEvent();
            _eventRepositoryMock.Setup(x => x.GetByFilter<Event>(dic, null)).Returns(Task.FromResult((messageReturn as List<Event>, 10000000000))!);

            // Act
            var result = (OkObjectResult)await _eventController.GetByNameAsync(name);

            // Assert
            Assert.AreEqual(messageReturn, result!.Value);
        }

        [Test]
        public async Task Given_Events_When_FindEventsByName_and_internal_error_Then_return_status_code_500_Async()
        {
            var name = "descricao";
            // Arrange
            var expectedMessage = MessageLogErrors.GetController;
            var dic = new Dictionary<string,object>(){{"Name",name}};
            _eventRepositoryMock.Setup(x => x.GetByFilter<Event>(dic,null)).Throws(new Exception("error coxcnection database"));

            // Act
            var result = (NoContentResult)await _eventController.GetByNameAsync(name);

            // Assert
            Assert.AreEqual(500, result!.StatusCode);
            Assert.AreEqual(expectedMessage, result);
        }

        [Test]
        public async Task Given_events_When_FindEventsByName_not_foud_register_Then_return_message_empty_list_Async()
        {
            // Arrange
            var name = "3b241101-e2bb-4255-8caf-4136c566a962";

            var expectedMessage = "Lista vazia";
            var dic = new Dictionary<string,object>(){{"Name",name}};
            _eventRepositoryMock.Setup(x => x.GetByFilter<Event>(dic,null)).Throws(new GetException(expectedMessage));

            // Act
            var result = (NoContentResult)await _eventController.GetByNameAsync(name);

            // Assert
            Assert.AreEqual(204, result?.StatusCode);
        }

        [Test]
        public async Task Given_id_event_When_FindById_Then_return_event_Async()
        {
            // Arrange
            var messageReturn = FactoryEvent.SimpleEvent();
            var id = "3b241101-e2bb-4255-8caf-4136c566a962";
            _eventRepositoryMock.Setup(x => x.GetById<EventComplet>(id)).
                Returns(Task.FromResult(new EventComplet()));

            // Act
            var result = (OkObjectResult)await _eventController.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_id_event_When_FindById_and_has_internal_error_Then_return_status_code_500_Async()
        {
            // Arrange
            var id = "3b241101-e2bb-4255-8caf-4136c566a962";
            var espectedReturn = MessageLogErrors.GetController;
            _eventRepositoryMock.Setup(x => x.GetById<object>(id)).
                Throws(new Exception("error conection database"));

            // Act
            var result = (ObjectResult)await _eventController.GetByIdAsync(id);

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
            _eventRepositoryMock.Setup(x => x.GetById<object>(id)).
                Throws(new GetException(expectedMessage));

            // Act
            var result = (NotFoundObjectResult)await _eventController.GetByIdAsync(id);

            // Assert
            Assert.AreEqual(404, result?.StatusCode);
        }

        [Test]
        public async Task Given_id_event_When_Delete_Then_return_message_deleted_Async()
        {
            // Arrange
            var messageReturn = "Evento deletado";
            var id = "3b241101-e2bb-4255-8caf-4136c566a962";
            _eventRepositoryMock.Setup(x => x.Delete(id)).Returns(Task.FromResult(true));

            // Act
            var result = (OkObjectResult)await _eventController.DeleteEventAsync(id);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_id_event_When_Delete_and_has_internal_error_Then_return_status_code_500_Async()
        {
            // Arrange
            var id = "3b241101-e2bb-4255-8caf-4136c566a962";
            var espectedReturn = MessageLogErrors.DeleteController;
            _eventRepositoryMock.Setup(x => x.Delete(id)).Throws(new Exception("error conection database"));

            // Act
            var result = (ObjectResult)await _eventController.DeleteEventAsync(id);

            // Assert
            Assert.AreEqual(500, result!.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_event_When_Save_Then_return_message_created_Async()
        {
            // Arrange
            object messageReturn = string.Empty;
            var eventSave = FactoryEvent.SimpleEvent();
            _eventRepositoryMock.Setup(x => x.Save(eventSave))
                .Returns(Task.FromResult(new Event()));
            _variantServiceMock.Setup(x => x.SaveAsync(It.IsAny<VariantWithLotDto>()))
                .Returns(Task.FromResult(new MessageReturn() { Data = "3b241101-e2bb-4255-8caf-4136c566a962" }));

            // Act
            var result = (OkObjectResult)await _eventController.SaveEventAsync(eventSave);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_event_When_Save_and_has_internal_error_Then_return_status_code_500_Async()
        {
            // Arrange
            var eventSave = FactoryEvent.SimpleEvent();
            var espectedReturn = MessageLogErrors.SaveController;
            _eventRepositoryMock.Setup(x => x.Save(eventSave)).Throws(new Exception("error conection database"));

            // Act
            var result = (ObjectResult)await _eventController.SaveEventAsync(eventSave);

            // Assert
            Assert.AreEqual(500, result!.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }
    }
}