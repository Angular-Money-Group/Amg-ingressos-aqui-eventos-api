using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_eventos_api.Services;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_tests.FactoryServices;
using Amg_ingressos_aqui_eventos_api.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Amg_ingressos_aqui_eventos_api.Dto;

namespace Prime.UnitTests.Services
{ 
    public class VariantServiceTest
    {
        private VariantService _variantService;
        private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
        private Mock<IVariantRepository> _variantRepositoryMock = new Mock<IVariantRepository>();
        private readonly Mock<ILotService> _lotServiceMock = new Mock<ILotService>();
        private readonly Mock<ILogger<VariantService>> _loggerMock = new Mock<ILogger<VariantService>>();
        private Mock<ILotRepository>  _lotRepositoryMock = new Mock<ILotRepository>();

    [SetUp]
        public void SetUp()
        {
            _variantRepositoryMock = new Mock<IVariantRepository>();
            _variantService = new VariantService(_variantRepositoryMock.Object,  _webHostEnvironmentMock.Object, _lotServiceMock.Object,_loggerMock.Object,_lotRepositoryMock.Object);
        }

        [Test]
        public void Given_complet_variant_When_save_Then_return_Ok()
        {
            //Arrange
            var variantComplet = FactoryVariant.SimpleVariant();
            var messageReturn = "OK";
            _variantRepositoryMock.Setup(x => x.Save(variantComplet)).Returns(Task.FromResult(new Variant()));
            _lotServiceMock.Setup(x => x.SaveAsync(It.IsAny<LotWithTicketDto>()))
                .Returns(Task.FromResult(new MessageReturn() { Data = "3b241101-e2bb-4255-8caf-4136c566a962" }));

            //Act
            var resultMethod = _variantService.SaveAsync(variantComplet);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Data);
        }

        [Test]
        public void Given_variant_without_name_When_save_Then_return_message_miss_name()
        {
            //Arrange
            var variantComplet = FactoryVariant.SimpleVariant();
            variantComplet.Name = string.Empty;
            var expectedMessage = new MessageReturn() { Message = "Nome é Obrigatório." };

            //Act
            var resultMethod = _variantService.SaveAsync(variantComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_variant_without_lot_When_save_Then_return_message_miss_Lot()
        {
            //Arrange
            var variantComplet = FactoryVariant.SimpleVariant();
            variantComplet.Lots = new List<LotWithTicketDto>();
            var expectedMessage = new MessageReturn() { Message = "Lote é Obrigatório." };

            //Act
            var resultMethod = _variantService.SaveAsync(variantComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_variant_with_position_without_Image_When_save_Then_return_message_miss_Image()
        {
            //Arrange
            var variantComplet = FactoryVariant.ListSimpleVariantWithPosition().First();
            variantComplet.LocaleImage = string.Empty;
            
            var expectedMessage = new MessageReturn() { Message = "Imagem Variante é Obrigatório." };

            //Act
            var resultMethod = _variantService.SaveAsync(variantComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_variant_with_position_without_PeoplePerPositions_When_save_Then_return_message_miss_PeoplePerPositions()
        {
            //Arrange
            var variantComplet = FactoryVariant.ListSimpleVariantWithPosition().First();
            variantComplet.Positions.PeoplePerPositions = 0;
            
            var expectedMessage = new MessageReturn() { Message = "Pessoas por posição é Obrigatório." };

            //Act
            var resultMethod = _variantService.SaveAsync(variantComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_variant_with_position_without_TotalPositions_When_save_Then_return_message_miss_TotalPositions()
        {
            //Arrange
            var variantComplet = FactoryVariant.ListSimpleVariantWithPosition().First();
            variantComplet.Positions.TotalPositions = 0;
            
            var expectedMessage = new MessageReturn() { Message = "Total de posições é Obrigatório." };

            //Act
            var resultMethod = _variantService.SaveAsync(variantComplet);

            //Assert
            Assert.AreEqual(expectedMessage.Message, resultMethod.Result.Message);
        }

        [Test]
        public void Given_complet_variant_When_save_Then_return_Internal_error()
        {
            //Arrange
            var variantComplet = FactoryVariant.SimpleVariant();
            _variantRepositoryMock.Setup(x => x.Save(variantComplet))
                .Throws(new Exception("erro ao conectar na base de dados"));
            _lotServiceMock.Setup(x => x.SaveAsync(It.IsAny<LotWithTicketDto>()))
                .Returns(Task.FromResult(new MessageReturn() { Data = "3b241101-e2bb-4255-8caf-4136c566a962" }));

            //Act
            var resultMethod = _variantService.SaveAsync(variantComplet);

            //Assert
            Assert.IsNotEmpty(resultMethod?.Exception?.Message);
        }
    }
}