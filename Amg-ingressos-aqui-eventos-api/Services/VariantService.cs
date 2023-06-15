using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Utils;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class VariantService : IVariantService
    {
        private IVariantRepository _variantRepository;
        private ILotService _lotService;
        private MessageReturn _messageReturn;

        public VariantService(IVariantRepository variantRepository, ILotService lotService)
        {
            _variantRepository = variantRepository;
            _lotService = lotService;
            _messageReturn = new MessageReturn();
        }
        public async Task<MessageReturn> SaveAsync(Variant variant)
        {
            try
            {
                ValidateModelSave(variant);
                variant.Status = Enum.StatusVariant.Active;
                _messageReturn.Data = await _variantRepository.Save<object>(variant);
                var IdentificateLot = 1;
                variant.Lot.ToList().ForEach(async i =>
                {
                    i.Identificate = IdentificateLot;
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

        public async Task<MessageReturn> EditAsync(string id, Variant varinatEdit)
        {
            try
            {
                _messageReturn.Data = await _variantRepository.Edit<object>(id, varinatEdit);
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

                var variant = await _variantRepository.FindById<List<Variant>>(id);

                variant[0].Lot.ToList().ForEach(async i =>
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

        private void ValidateModelSave(Variant variant)
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
    }
}