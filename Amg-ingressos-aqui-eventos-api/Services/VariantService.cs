using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Utils;
using Amg_ingressos_aqui_eventos_api.Dto;
using System.Text.RegularExpressions;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class VariantService : IVariantService
    {
        private IVariantRepository _variantRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private ILotService _lotService;
        private MessageReturn _messageReturn;

        public VariantService(
            IVariantRepository variantRepository,
            IWebHostEnvironment webHostEnvironment,
            ILotService lotService
        )
        {
            _variantRepository = variantRepository;
            _lotService = lotService;
            _webHostEnvironment = webHostEnvironment;
            _messageReturn = new MessageReturn();
        }
        public async Task<MessageReturn> SaveAsync(Model.Variant variant)
        {
            try
            {
                ValidateModelSave(variant);
                if(variant.LocaleImage != string.Empty){
                    variant.LocaleImage = StoreImageAndGenerateLinkToAccess(variant.LocaleImage!);
                }
                variant.Status = Enum.StatusVariant.Active;
                _messageReturn.Data = await _variantRepository.Save<object>(variant);
                var IdentificateLot = 1;
                variant.Lot
                    .ToList()
                    .ForEach(async i =>
                    {
                        i.Identificate = IdentificateLot;
                        i.ReqDocs = variant.ReqDocs;
                        i.IdVariant = _messageReturn.Data.ToString();
                        i.Id = _lotService.SaveAsync(i).Result.Data.ToString();
                        IdentificateLot++;
                    });
            }
            catch (SaveVariantException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> EditAsync(List<VariantEditDto> variantEditDto)
        {
            try
            {
                foreach (var item in variantEditDto)
                {
                    var variantEdit = new Model.Variant()
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
                        Id= item.Id,
                        IdEvent = item.IdEvent
                    };
                    if(variantEdit.Id != null)
                        _messageReturn.Data = await _variantRepository.Edit<object>(variantEdit.Id, variantEdit);
                    else
                        _messageReturn.Data = await _variantRepository.Save<object>(variantEdit);
                }
            }
            catch (SaveLotException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteAsync(string id)
        {
            try
            {
                id.ValidateIdMongo();

                var variant = await _variantRepository.FindById<List<Model.Variant>>(id);

                variant[0].Lot
                    .ToList()
                    .ForEach(async i =>
                    {
                        _lotService.DeleteAsync(i.Id);
                    });

                _messageReturn.Data = await _variantRepository.Delete<object>(id);
            }
            catch (SaveLotException ex)
            {
                DeleteAsync(id);
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                DeleteAsync(id);
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> DeleteManyAsync(List<Model.Querys.Variant> Variant)
        {
            try
            {
                Variant.ForEach(async variant =>
                {
                    var LotsId = variant.Lot.Select(d => d.Id).ToList();

                    await _lotService.DeleteManyAsync(LotsId);
                });

                List<string> variantId = Variant.Select(v => v.Id).ToList();
                _messageReturn.Data = await _variantRepository.DeleteMany<object>(variantId);
            }
            catch (SaveLotException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        private void ValidateModelSave(Model.Variant variant)
        {
            if (variant.Name == "")
                throw new SaveVariantException("Nome é Obrigatório.");
            if (!variant.Lot.Any())
                throw new SaveVariantException("Lote é Obrigatório.");
            if (variant.HasPositions)
            {
                if (variant.LocaleImage == string.Empty)
                    throw new SaveVariantException("Imagem Variante é Obrigatório.");
                if (variant.Positions.PeoplePerPositions == 0)
                    throw new SaveVariantException("Pessoas por posição é Obrigatório.");
                if (variant.Positions.TotalPositions == 0)
                    throw new SaveVariantException("Total de posições é Obrigatório.");
            }
        }

        public async Task<MessageReturn> SaveManyAsync(List<Model.Variant> listVariant)
        {
            try
            {
                listVariant.ForEach(v =>
                {
                    ValidateModelSave(v);
                    if(v.LocaleImage != string.Empty){
                        v.LocaleImage = StoreImageAndGenerateLinkToAccess(v.LocaleImage!);
                    }

                    v.Status = Enum.StatusVariant.Active;
                });
                _messageReturn.Data = await _variantRepository.SaveMany<Model.Variant>(listVariant);

                listVariant.ForEach(async i =>
                {
                    var IdentificateLot = 1;
                    i.Lot.ForEach(l =>
                    {
                        l.Status = IdentificateLot == 1 ? Enum.StatusLot.Open : Enum.StatusLot.Wait;
                        l.Identificate = IdentificateLot;
                        l.ReqDocs = i.ReqDocs;
                        l.IdVariant = i.Id;
                        IdentificateLot++;
                        IdentificateLot++;
                    });
                    _lotService.SaveManyAsync(i.Lot);
                });
            }
            catch (SaveVariantException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }
    }
}
