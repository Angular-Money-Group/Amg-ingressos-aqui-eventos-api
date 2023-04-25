using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using System.Text;
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
                if (variant.Name == "")
                    throw new SaveVariantException("Nome é Obrigatório.");

                variant.Lot.ForEach(async i => {
                    i.Id = _lotService.SaveAsync(i).Result.Data.ToString();
                });
                
                _messageReturn.Data = await _variantRepository.Save<object>(variant);
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
    }
}