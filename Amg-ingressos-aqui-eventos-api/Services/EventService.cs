using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Model.Querys;
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

        private HttpClient _client;


        public EventService(IEventRepository eventRepository, IVariantService variantService, IWebHostEnvironment webHostEnvironment, HttpClient client)
        {
            _eventRepository = eventRepository;
            _variantService = variantService;
            _webHostEnvironment = webHostEnvironment;
            _client = client;
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
        public async Task<MessageReturn> FindEventByNameAsync(string name)
        {
            try
            {
                _messageReturn.Data = await _eventRepository.FindByName<List<Event>>(name);

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
                eventSave.Image = StoreImageAndGenerateLinkToAccess(eventSave.Image!);

                eventSave.Status = Enum.StatusEvent.Active;
            
                _messageReturn.Data = await _eventRepository.Save<object>(eventSave);
                if(_messageReturn.Data == null)
                    throw new SaveEventException("");

                eventSave.Variant.ToList().ForEach(i =>
                {
                    i.IdEvent = _messageReturn.Data.ToString();
                    //i.Id = _variantService.SaveAsync(i).Result.Data.ToString();
                });

                _variantService.SaveManyAsync(eventSave.Variant).Result.Data.ToString();


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
        public async Task<MessageReturn> HighlightEventAsync(string id)
        {
            try
            {
                _messageReturn.Data = await _eventRepository.SetHighlightEvent<Event>(id);

                return _messageReturn;
            }
            catch (FindByDescriptionException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (MaxHighlightedEvents ex)
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
                    var allEvents = await _eventRepository.GetAllEvents<List<GetEventsWithNames>>(paginationOptions);

                    _messageReturn.Data = allEvents;
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

        public async Task<MessageReturn> FindByOrganizerAsync(string idOrganizer, Pagination paginationOptions)
        {
            try
            {
                _messageReturn.Data = await _eventRepository.FindByProducer<List<Event>>(idOrganizer, paginationOptions);
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

        public async Task<MessageReturn> EditEventsAsync(string id, Event eventEdit)
        {
            try
            {
                if(eventEdit.Image != null){
                    IsBase64Image(eventEdit.Image!);
                    eventEdit.Image = StoreImageAndGenerateLinkToAccess(eventEdit.Image!);
                };


                _messageReturn.Data = await _eventRepository.Edit<Event>(id, eventEdit);

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

        private void IsBase64Image(string base64String)
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

        private string StoreImageAndGenerateLinkToAccess(string image)
        {
            try
            {
                image = Regex.Replace(image, @"data:image/.*?;base64,", "");

                byte[] imageBytes = Convert.FromBase64String(image);

                var nomeArquivoImage = $"{Guid.NewGuid()}.jpg";
                var directoryPathImage = Path.Combine(_webHostEnvironment.ContentRootPath, "images");

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