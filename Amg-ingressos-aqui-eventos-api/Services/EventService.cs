using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Dto;
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
        private readonly IEventRepository _eventRepository;
        private readonly IVariantService _variantService;
        private readonly MessageReturn _messageReturn;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<EventService> _logger;
        
        public EventService(
            IEventRepository eventRepository,
            IVariantService variantService,
            IWebHostEnvironment webHostEnvironment,
            ILogger<EventService> logger
        )
        {
            _eventRepository = eventRepository;
            _variantService = variantService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> GetByIdAsync(string id)
        {
            try
            {
                id.ValidateIdMongo();
                _messageReturn.Data = await _eventRepository.GetById<Event>(id);

                return _messageReturn;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetByIdAsync), "Evento"), id);
                _messageReturn.Message = ex.Message;
            }
            catch (GetException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetByIdAsync), "Evento"), id);
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetByIdAsync), "Evento"), id);
                _messageReturn.Message = ex.Message;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> GetByNameAsync(string name)
        {
            try
            {
                _messageReturn.Data = await _eventRepository.GetByName<List<Event>>(name);
                return _messageReturn;
            }
            catch (GetException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetByNameAsync), "Evento"), name);
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetByNameAsync), "Evento"), name);
                _messageReturn.Message = ex.Message;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> SaveAsync(Event eventObject)
        {
            try
            {
                ValidateModelSave(eventObject);
                IsBase64Image(eventObject.Image!);
                eventObject.Image = StoreImageAndGenerateLinkToAccess(eventObject.Image!);
                eventObject.Status = Enum.EnumStatusEvent.Active;
                eventObject.Courtesy = new Courtesy()
                {
                    RemainingCourtesy = new List<RemainingCourtesy>(),
                    CourtesyHistory = new List<CourtesyHistory>()
                };
                _messageReturn.Data = await _eventRepository.Save<object>(eventObject);

                var variantId = "";

                eventObject.Variant
                    .ToList()
                    .ForEach(i =>
                    {
                        i.IdEvent = _messageReturn.Data.ToString() ?? string.Empty;
                        variantId = _variantService.SaveAsync(i).Result.Data.ToString();

                        if (i.QuantityCourtesy > 0)
                        {
                            var RemainingCourtesy = new RemainingCourtesy
                            {
                                VariantId = i.Id,
                                VariantName = i.Name,
                                Quantity = i.QuantityCourtesy
                            };

                            eventObject.Courtesy.RemainingCourtesy.Add(RemainingCourtesy);
                        }
                    });

                _messageReturn.Data = await _eventRepository.Edit<object>(eventObject.Id, eventObject);
            }
            catch (SaveException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), "Evento"), eventObject);
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), "Evento"), eventObject);
                throw;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> SetHighlightEventAsync(string id)
        {
            try
            {
                _messageReturn.Data = await _eventRepository.SetHighlightEvent<Event>(id);
                return _messageReturn;
            }
            catch (GetException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(SetHighlightEventAsync), "Eventos Destaque"), id);
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(SetHighlightEventAsync), "Eventos Destaque"), id);
                throw;
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
                _logger.LogError(ex, string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteAsync), "Evento"), id);
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteAsync), "Evento"), id);
                throw;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> GetEventsAsync(
            bool highlights,
            bool weekly,
            Pagination paginationOptions
        )
        {
            try
            {
                if (highlights)
                {
                    _messageReturn.Data = await _eventRepository.GetHighlightedEvents<List<Event>>(
                        paginationOptions
                    );
                }
                else if (weekly)
                {
                    _messageReturn.Data = await _eventRepository.GetWeeklyEvents<List<Event>>(
                        paginationOptions
                    );
                }
                else
                {
                    var allEvents = await _eventRepository.GetAllEvents<List<GetEventsWithNames>>(
                        paginationOptions
                    );

                    _messageReturn.Data = allEvents;
                }
            }
            catch (GetException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetByNameAsync), "Eventos"));
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetByNameAsync), "Eventos"));
                throw;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> GetWithUserData()
        {
            try
            {
                _messageReturn.Data = await _eventRepository.GetWithUserData<List<GetEventsWithNames>>();
            }
            catch (GetException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetWithUserData), "Eventos"));
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetWithUserData), "Eventos"));
                throw;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> GetByOrganizerAsync(
            string idOrganizer,
            Pagination paginationOptions,
            FilterOptions? filter
        )
        {
            try
            {
                _messageReturn.Data = await _eventRepository.GetByProducer<List<Event>>(
                    idOrganizer,
                    paginationOptions,
                    filter
                );
            }
            catch (GetException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetByNameAsync), "Eventos"));
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetByNameAsync), "Eventos"));
                throw;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> EditEventsAsync(string id, EventEditDto eventDto)
        {
            try
            {
                var eventEdit = new Event()
                {
                    Address = eventDto.Address,
                    Id = eventDto.Id,
                    Description = eventDto.Description,
                    StartDate = eventDto.StartDate,
                    EndDate = eventDto.EndDate,
                    Highlighted = eventDto.Highlighted,
                    IdMeansReceipt = eventDto.IdMeansReceipt,
                    Image = eventDto.Image,
                    Local = eventDto.Local,
                    Name = eventDto.Name,
                    Status = eventDto.Status,
                    Type = eventDto.Type
                };

                if (eventEdit.Image != null && IsBase64String(eventEdit.Image))
                {
                    IsBase64Image(eventEdit.Image!);
                    eventEdit.Image = StoreImageAndGenerateLinkToAccess(eventEdit.Image!);
                }

                _messageReturn.Data = await _eventRepository.Edit<Event>(id, eventEdit);
            }
            catch (EditException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Edit, this.GetType().Name, nameof(EditEventsAsync), "Evento"));
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Edit, this.GetType().Name, nameof(EditEventsAsync), "Evento"));
                throw;
            }
            return _messageReturn;
        }

        public async Task<MessageReturn> GetAllEventsWithTickets(string idEvent)
        {
            try
            {
                idEvent.ValidateIdMongo();
                _messageReturn.Data = await _eventRepository.GetAllEventsWithTickets(idEvent, string.Empty);
                return _messageReturn;
            }
            catch (IdMongoException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetAllEventsWithTickets), "Evento"));
                _messageReturn.Message = ex.Message;
            }
            catch (GetException ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetAllEventsWithTickets), "Evento"));
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetAllEventsWithTickets), "Evento"));
                throw;
            }
            return _messageReturn;
        }

        private void ValidateModelSave(Event eventSave)
        {
            if (eventSave.Name == "")
                throw new SaveException("Nome é Obrigatório.");
            if (eventSave.Local == "")
                throw new SaveException("Local é Obrigatório.");
            if (eventSave.Type == "")
                throw new SaveException("Tipo é Obrigatório.");
            if (eventSave.Image == "")
                throw new SaveException("Imagem é Obrigatório.");
            if (eventSave.Description == "")
                throw new SaveException("Descrição é Obrigatório.");
            if (eventSave.Address == null)
                throw new SaveException("Endereço é Obrigatório.");
            if (eventSave.Address.Cep == "")
                throw new SaveException("CEP é Obrigatório.");
            if (eventSave.Address.Number == string.Empty)
                throw new SaveException("Número Endereço é Obrigatório.");
            if (eventSave.Address.Neighborhood == "")
                throw new SaveException("Vizinhança é Obrigatório.");
            if (eventSave.Address.City == "")
                throw new SaveException("Cidade é Obrigatório.");
            if (eventSave.Address.State == "")
                throw new SaveException("Estado é Obrigatório.");
            if (eventSave.StartDate == DateTime.MinValue)
                throw new SaveException("Data Inicio é Obrigatório.");
            if (eventSave.EndDate == DateTime.MinValue)
                throw new SaveException("Data Fim é Obrigatório.");
            if (!eventSave.Variant.Any())
                throw new SaveException("Variante é Obrigatório.");
        }

        public static bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }

        private void IsBase64Image(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                throw new SaveException("Imagem é obrigatório");

            var base64Data = Regex
                .Match(base64String, @"data:image/(?<type>.+?),(?<data>.+)")
                .Groups["data"].Value;

            try
            {
                byte[] imageData = Convert.FromBase64String(base64Data);
            }
            catch (FormatException)
            {
                throw new SaveException("Essa imagem não está em base64");
            }
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
            catch
            {
                throw;
            }
        }
    }
}