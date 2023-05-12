using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Utils;
using System.Text.RegularExpressions;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class EventService : IEventService
    {
        private IEventRepository _eventRepository;
        private IVariantService _variantService;
        private MessageReturn _messageReturn;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EventService(IEventRepository eventRepository, IVariantService variantService, IWebHostEnvironment webHostEnvironment)
        {
            _eventRepository = eventRepository;
            _variantService = variantService;
            _webHostEnvironment = webHostEnvironment;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> FindByIdAsync(string id)
        {
            try
            {
                id.ValidateIdMongo();

                _messageReturn.Data = await _eventRepository.FindById<Event>(id);

                return _messageReturn;
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _messageReturn;
        }
        public async Task<MessageReturn> FindEventByDescriptionAsync(string description)
        {
            try
            {
                _messageReturn.Data = await _eventRepository.FindByDescription<Event>(description);

                return _messageReturn;
            }
            catch (FindByDescriptionException ex)
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
                ValidateModelSave(eventSave);
                IsBase64Image(eventSave.Image!);

                byte[] imageBytes = Convert.FromBase64String(eventSave.Image!);

                eventSave.Image = eventSave.Image!.Replace("data:image/jpeg;base64,", "");

                var nomeArquivo = $"{Guid.NewGuid()}.jpg";
                var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "images", nomeArquivo);
                string linkImagem = "http://api.ingressosaqui.com:3002/imagens/" + nomeArquivo;


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    stream.Write(imageBytes, 0, imageBytes.Length);
                }

                eventSave.Image = linkImagem;
                _messageReturn.Data = await _eventRepository.Save<object>(eventSave);

                eventSave.Variant.ToList().ForEach(i =>
                {
                    i.IdEvent = _messageReturn.Data.ToString() ?? string.Empty;
                    i.Id = _variantService.SaveAsync(i).Result.Data.ToString();
                });
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
                id.ValidateIdMongo();

                _messageReturn.Data = (string)await _eventRepository.Delete<object>(id);
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> GetEventsAsync(bool highlights, bool weekly, Pagination paginationOptions)
        {
            try
            {
                if (highlights)
                {
                    _messageReturn.Data = await _eventRepository.GetHighlightedEvents<List<Event>>(paginationOptions);
                }
                else if (weekly)
                {
                    _messageReturn.Data = await _eventRepository.GetWeeklyEvents<List<Event>>(paginationOptions);
                }
                else
                {
                    _messageReturn.Data = await _eventRepository.GetAllEvents<List<Event>>();
                }
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

        private void ValidateModelSave(Event eventSave)
        {
            if (eventSave.Name == "")
                throw new SaveEventException("Nome é Obrigatório.");
            if (eventSave.Local == "")
                throw new SaveEventException("Local é Obrigatório.");
            if (eventSave.Type == "")
                throw new SaveEventException("Tipo é Obrigatório.");
            if (eventSave.Image == "")
                throw new SaveEventException("Imagem é Obrigatório.");
            if (eventSave.Description == "")
                throw new SaveEventException("Descrição é Obrigatório.");
            if (eventSave.Address == null)
                throw new SaveEventException("Endereço é Obrigatório.");
            if (eventSave.Address.Cep == "")
                throw new SaveEventException("CEP é Obrigatório.");
            if (eventSave.Address.Number == string.Empty)
                throw new SaveEventException("Número Endereço é Obrigatório.");
            if (eventSave.Address.Neighborhood == "")
                throw new SaveEventException("Vizinhança é Obrigatório.");
            if (eventSave.Address.City == "")
                throw new SaveEventException("Cidade é Obrigatório.");
            if (eventSave.Address.State == "")
                throw new SaveEventException("Estado é Obrigatório.");
            if (eventSave.StartDate == DateTime.MinValue)
                throw new SaveEventException("Data Inicio é Obrigatório.");
            if (eventSave.EndDate == DateTime.MinValue)
                throw new SaveEventException("Data Fim é Obrigatório.");
            if (!eventSave.Variant.Any())
                throw new SaveEventException("Variante é Obrigatório.");
        }

        public void IsBase64Image(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                throw new SaveEventException("Imagem é obrigatório");

            var base64Data = Regex.Match(base64String, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

            try
            {
                byte[] imageData = Convert.FromBase64String(base64Data);
            }
            catch (FormatException)
            {
                throw new SaveEventException("Essa imagem não está em base64");
            }
        }
    }
}