using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Utils;
using Amg_ingressos_aqui_eventos_api.Dto;
using System.Text.RegularExpressions;
using Amg_ingressos_aqui_eventos_api.Consts;
using System.Data;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class VariantService : IVariantService
    {
        private readonly IVariantRepository _variantRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILotService _lotService;
        private readonly ILogger<VariantService> _logger;
        private readonly MessageReturn _messageReturn;

        public VariantService(
            IVariantRepository variantRepository,
            IWebHostEnvironment webHostEnvironment,
            ILotService lotService,
            ILogger<VariantService> logger
        )
        {
            _variantRepository = variantRepository;
            _lotService = lotService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _messageReturn = new MessageReturn();
        }
        public async Task<MessageReturn> SaveAsync(VariantWithLotDto variant)
        {
            try
            {
                ValidateModelSave(variant);
                if (variant.LocaleImage != string.Empty)
                {
                    variant.LocaleImage = StoreImageAndGenerateLinkToAccess(variant.LocaleImage!);
                }
                variant.Status = Enum.EnumStatusVariant.Active;
                var idVariant = await _variantRepository.Save<object>(variant) ?? throw new RuleException("Id nao pode ser null");
                var IdentificateLot = 1;
                variant.Lots
                    .ToList()
                    .ForEach(i =>
                    {
                        i.Identificate = IdentificateLot;
                        i.ReqDocs = variant.ReqDocs;
                        i.IdVariant = idVariant.ToString() ?? string.Empty;
                        i.Id = _lotService.SaveAsync(i).Result.Data.ToString() ?? string.Empty;
                        IdentificateLot++;
                    });
            }
            catch (SaveException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), "Variante"));
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), "Variante"));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> EditAsync(List<VariantEditDto> listVariant)
        {
            try
            {
                foreach (var item in listVariant)
                {
                    var variantEdit = new Variant()
                    {
                        Description = item.Description,
                        HasPositions = item.HasPositions,
                        LocaleImage = item.LocaleImage,
                        Name = item.Name,
                        Positions = item.Positions,
                        QuantityCourtesy = item.QuantityCourtesy,
                        ReqDocs = item.ReqDocs,
                        SellTicketsBeforeStartAnother = item.SellTicketsBeforeStartAnother,
                        SellTicketsInAnotherBatch = item.SellTicketsInAnotherBatch,
                        Status = item.Status,
                        Id = item.Id,
                        IdEvent = item.IdEvent
                    };

                    if (variantEdit.Id != null)
                        _messageReturn.Data = await _variantRepository.Edit<object>(variantEdit.Id, variantEdit);
                    else
                        _messageReturn.Data = await _variantRepository.Save<object>(variantEdit);
                }
            }
            catch (EditException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Edit, this.GetType().Name, nameof(EditAsync), "Variante"));
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Edit, this.GetType().Name, nameof(EditAsync), "Variante"));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteAsync(string id)
        {
            try
            {
                id.ValidateIdMongo();
                await _lotService.DeleteByVariantAsync(id);
                _messageReturn.Data = await _variantRepository.Delete<object>(id);
            }
            catch (DeleteException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteAsync), "Variante"));
                _ = DeleteAsync(id);
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteAsync), "Variante"));
                _ = DeleteAsync(id);
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteManyAsync(List<VariantWithLotDto> listVariant)
        {
            try
            {
                listVariant.ForEach(async variant =>
                {
                    List<string> LotsId = variant.Lots.Select(d => d.Id).ToList();

                    await _lotService.DeleteManyAsync(LotsId);
                });

                List<string> variantId = listVariant.Select(v => v.Id).ToList();
                _messageReturn.Data = await _variantRepository.DeleteMany<object>(variantId);
            }
            catch (DeleteException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteManyAsync), "Variantes"));
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteManyAsync), "Variantes"));
                throw;
            }

            return _messageReturn;
        }

        private void ValidateModelSave(VariantWithLotDto variant)
        {
            if (variant.Name == "")
                throw new SaveException("Nome é Obrigatório.");
            if (!variant.Lots.Any())
                throw new SaveException("Lote é Obrigatório.");
            if (variant.HasPositions)
            {
                if (variant.LocaleImage == string.Empty)
                    throw new SaveException("Imagem Variante é Obrigatório.");
                if (variant.Positions.PeoplePerPositions == 0)
                    throw new SaveException("Pessoas por posição é Obrigatório.");
                if (variant.Positions.TotalPositions == 0)
                    throw new SaveException("Total de posições é Obrigatório.");
            }
        }

        public async Task<MessageReturn> SaveManyAsync(List<VariantWithLotDto> listVariant)
        {
            try
            {
                listVariant.ForEach(v =>
                {
                    ValidateModelSave(v);
                    if (!string.IsNullOrEmpty(v.LocaleImage))
                    {
                        v.LocaleImage = StoreImageAndGenerateLinkToAccess(v.LocaleImage!);
                    }
                    v.Status = Enum.EnumStatusVariant.Active;
                });

                var listVariantModel = new VariantWithLotDto().ListDtoToListModel(listVariant);
                _messageReturn.Data = await _variantRepository.SaveMany<Variant>(listVariantModel);

                listVariant.ForEach(i =>
                {
                    var IdentificateLot = 1;
                    i.Lots.ForEach(l =>
                    {
                        l.Status = IdentificateLot == 1 ? Enum.EnumStatusLot.Open : Enum.EnumStatusLot.Wait;
                        l.Identificate = IdentificateLot;
                        l.ReqDocs = i.ReqDocs;
                        l.IdVariant = i.Id;
                        IdentificateLot++;
                        IdentificateLot++;
                    });
                    _ = _lotService.SaveManyAsync(i.Lots);
                });
            }
            catch (SaveException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveManyAsync), "Variantes"));
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveManyAsync), "Variantes"));
                throw;
            }

            return _messageReturn;
        }

        private string StoreImageAndGenerateLinkToAccess(string image)
        {
            try
            {
                image = Regex.Replace(image, @"data:image/.*?;base64,", "");

                byte[] imageBytes = Convert.FromBase64String(image);

                var nomeArquivoImage = $"{Guid.NewGuid()}.jpg";
                var directoryPathImage = Path.Combine(
                    _webHostEnvironment.ContentRootPath,
                    "images"
                );

                Directory.CreateDirectory(directoryPathImage);

                var filePathImage = Path.Combine(directoryPathImage, nomeArquivoImage);

                string linkImagem = "https://api.ingressosaqui.com/imagens/" + nomeArquivoImage;

                using (var stream = new FileStream(filePathImage, FileMode.Create))
                {
                    stream.Write(imageBytes, 0, imageBytes.Length);
                }
                return linkImagem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("{0}:{1} - Erro ao gerar link imagem variante.", this.GetType().Name, nameof(StoreImageAndGenerateLinkToAccess)), image);
                throw;
            }
        }
    }
}