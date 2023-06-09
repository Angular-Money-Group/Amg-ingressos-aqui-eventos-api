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
    public class VariantServiceTest
    {
        private VariantService _variantService;
        private Mock<IVariantRepository> _variantRepositoryMock = new Mock<IVariantRepository>();
        private Mock<ILotService> _lotServiceMock = new Mock<ILotService>();

        [SetUp]
        public void SetUp()
        {
            _variantRepositoryMock = new Mock<IVariantRepository>();
            _variantService = new VariantService(_variantRepositoryMock.Object, _lotServiceMock.Object);
        }

        [Test]
        public void Given_complet_variant_When_save_Then_return_Ok()
        {
            //Arrange
            var variantComplet = FactoryVariant.SimpleVariant();
            var messageReturn = "OK";
            _variantRepositoryMock.Setup(x => x.Save<object>(variantComplet)).Returns(Task.FromResult(messageReturn as object));
            _lotServiceMock.Setup(x => x.SaveAsync(It.IsAny<Lot>()))
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
        public void Given_variant_without_lot_When_save_Then_return_message_miss_Variant()
        {
            //Arrange
            var variantComplet = FactoryVariant.SimpleVariant();
            variantComplet.Lot = new List<Lot>();
            var expectedMessage = new MessageReturn() { Message = "Lote é Obrigatório." };

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
            _variantRepositoryMock.Setup(x => x.Save<object>(variantComplet))
                .Throws(new Exception("erro ao conectar na base de dados"));
            _lotServiceMock.Setup(x => x.SaveAsync(It.IsAny<Lot>()))
                .Returns(Task.FromResult(new MessageReturn() { Data = "3b241101-e2bb-4255-8caf-4136c566a962" }));

            //Act
            var resultMethod = _variantService.SaveAsync(variantComplet);

            //Assert
            Assert.IsNotEmpty(resultMethod.Exception.Message);
        }
    }
}