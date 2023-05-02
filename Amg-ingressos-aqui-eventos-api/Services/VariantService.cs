using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Exceptions;

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
                variant.Status = Enum.StatusVariant.Active;
                _messageReturn.Data = await _variantRepository.Save<object>(variant);
                var IdentificadorLote = 0;
                variant.Lot.ToList().ForEach(async i =>
                {
                    i.Identificador = (IdentificadorLote++);
                    i.IdVariant = _messageReturn.Data.ToString();
                    i.Id = _lotService.SaveAsync(i).Result.Data.ToString();
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
        private void ValidateModelSave(Variant variant)
        {
            if (variant.Name == "")
                throw new SaveVariantException("Nome é Obrigatório.");
            if (!variant.Lot.Any())
                throw new SaveVariantException("Lote é Obrigatório.");
            if (variant.HasPositions)
            {
                if(variant.LocaleImage== string.Empty)
                    throw new SaveVariantException("Imagem Variante é Obrigatório.");
                if (variant.Positions.PeoplePerPositions == 0)
                    throw new SaveVariantException("Pessoas por posição é Obrigatório.");
                if (variant.Positions.TotalPositions == 0)
                    throw new SaveVariantException("Total de posições é Obrigatório.");
            }
        }
    }
}