using System.Text;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class EventService : IEventService
    {
        private IEventRepository _eventRepository;
        private MessageReturn _messageReturn;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> FindByIdAsync(string id)
        {
            try
            {
                _messageReturn.Message = ValidateIdMongo(id);

                if (_messageReturn.Message.Any())
                    throw new FindByIdEventException(_messageReturn.Message);
                
                _messageReturn.Data = await _eventRepository.FindById<Event>(id);

                return _messageReturn;
            }
            catch (FindByIdEventException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> SaveAsync(Event eventSave)
        {
            try
            {
                _messageReturn.Message = ValidateModelSave(eventSave);

                if (_messageReturn.Message.Any())
                    throw new SaveEventException(_messageReturn.Message);

                _messageReturn.Data = await _eventRepository.Save<object>(eventSave);
            }
            catch (SaveEventException ex)
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
                _messageReturn.Message = ValidateIdMongo(id);

                if (_messageReturn.Message.Any())
                    throw new DeleteEventException(_messageReturn.Message);

                _messageReturn.Data = (string)await _eventRepository.Delete<object>(id);
            }
            catch (DeleteEventException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> GetAllEventsAsync()
        {
            try
            {
                _messageReturn.Data = await _eventRepository.GetAllEvents<object>();
            }
            catch (GetAllEventException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _messageReturn;
        }

        private string ValidateModelSave(Event eventSave)
        {
            StringBuilder mensage = new StringBuilder();

            if (eventSave.Name == "")
                mensage.Append("Nome é Obrigatório.");
            else if (eventSave.Local == "")
                mensage.Append("Local é Obrigatório.");
            else if (eventSave.Type == "")
                mensage.Append("Tipo é Obrigatório.");
            else if (eventSave.Image == "")
                mensage.Append("Imagem é Obrigatório.");
            else if (eventSave.Description == "")
                mensage.Append("Descrição é Obrigatório.");
            else if (eventSave.Cep == "")
                mensage.Append("CEP é Obrigatório.");
            else if (eventSave.Address == "")
                mensage.Append("Endereço é Obrigatório.");
            else if (eventSave.Number == 0)
                mensage.Append("Número Endereço é Obrigatório.");
            else if (eventSave.Neighborhood == "")
                mensage.Append("Vizinhança é Obrigatório.");
            else if (eventSave.Complement == "")
                mensage.Append("Complemento é Obrigatório.");
            else if (eventSave.ReferencePoint == "")
                mensage.Append("Ponto de referência é Obrigatório.");
            else if (eventSave.City == "")
                mensage.Append("Cidade é Obrigatório.");
            else if (eventSave.State == "")
                mensage.Append("Estado é Obrigatório.");
            else if (eventSave.Day == "")
                mensage.Append("Dia é Obrigatório.");
            else if (eventSave.Lot == "")
                mensage.Append("Lote é Obrigatório.");
            else if (eventSave.VipArea == "")
                mensage.Append("Área Vip é Obrigatório.");

            return mensage.ToString();
        }

        private string ValidateIdMongo(string id)
        {
            if (string.IsNullOrEmpty(id))
                return "Id é obrigatório";
            else if (id.Length < 24)
                return "Id é obrigatório e está menor que 24 digitos";
            else
                return string.Empty;
        }
    }
}