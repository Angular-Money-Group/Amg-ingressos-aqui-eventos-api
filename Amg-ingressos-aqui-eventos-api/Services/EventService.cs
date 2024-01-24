using Amg_ingressos_aqui_eventos_api.Consts;
using Amg_ingressos_aqui_eventos_api.Dto;
using Amg_ingressos_aqui_eventos_api.Enum;
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
                var dic = new Dictionary<string, object>() { { "_id", id } };
                var data = await _eventRepository.GetByFilter<EventComplet>(dic, null);

                _messageReturn.Data = new EventCompletWithTransactionDto().ModelToDto(data.Item1[0]);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetByIdAsync), "Evento"), ex);
                throw;
            }
        }

        public async Task<MessageReturn> SaveAsync(EventCompletDto eventObject)
        {
            try
            {
                Event modelEvent = new EventCompletDto().DtoToModel(eventObject);
                modelEvent.ValidateModelSave();
                if (!eventObject.Variants.Any())
                    throw new SaveException("Variante é Obrigatório.");

                modelEvent.Image = StoreImageAndGenerateLinkToAccess(eventObject.Image!);
                modelEvent.Status = StatusEvent.Active;
                await _eventRepository.Save(modelEvent);

                //processa variantes
                eventObject.Variants.ForEach(i =>
                    {
                        i.IdEvent = modelEvent.Id ?? string.Empty;
                    });

                _ = await _variantService.SaveManyAsync(eventObject.Variants);

                eventObject.Variants.ForEach(v =>
                {
                    if (v.QuantityCourtesy > 0)
                    {
                        var RemainingCourtesy = new RemainingCourtesy
                        {
                            VariantId = v.Id,
                            VariantName = v.Name,
                            Quantity = v.QuantityCourtesy
                        };
                        modelEvent.Courtesy.RemainingCourtesy.Add(RemainingCourtesy);
                    }
                });

                await _eventRepository.Edit(modelEvent.Id, modelEvent);
                _messageReturn.Data = modelEvent;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), "Evento"), ex);
                throw;
            }
        }

        public async Task<MessageReturn> SetHighlightEventAsync(string id)
        {
            try
            {
                _messageReturn.Data = await _eventRepository.SetHighlightEvent(id);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(SetHighlightEventAsync), "Eventos Destaque"), ex);
                throw;
            }
        }

        public async Task<MessageReturn> DeleteAsync(string id)
        {
            try
            {
                id.ValidateIdMongo();
                var result = await _eventRepository.Delete(id);
                if (result)
                    _messageReturn.Data = "evento deletado";
                else
                    _messageReturn.Message = "evento não deletado";
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteAsync), "Evento"), ex);
                throw;
            }
        }

        public async Task<MessageReturn> GetEventsAsync(FilterOptions filters, Pagination paginationOptions)
        {
            try
            {
                Dictionary<string, object> dic = GenerateFilters(filters);
                var data = await _eventRepository.GetByFilter<EventComplet>(dic, paginationOptions);

                _messageReturn.Data = new EventCompletWithTransactionDto().ModelListToDtoList(data.Item1);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetEventsAsync), "Eventos"),ex);
                throw;
            }
        }

        public async Task<MessageReturn> GetEventsForGridAsync(FilterOptions filters, Pagination paginationOptions)
        {
            try
            {
                Dictionary<string, object> dic = GenerateFilters(filters);
                var data = await _eventRepository.GetByFilter<EventComplet>(dic, paginationOptions);

                _messageReturn.Data = new { data = new GridEventDto().ModelListToDtoList(data.Item1), TotalCount = data.count };
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetEventsAsync), "Eventos"),ex);
                throw;
            }
        }

        public async Task<MessageReturn> GetCardEventsHighligth()
        {
            try
            {
                var dic = GenerateFilters(new FilterOptions() { Highlights = true });
                var data = await _eventRepository.GetByFilter<EventComplet>(dic, null);
                _messageReturn.Data = new CardDto().ModelListToDtoList(data.Item1);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetCardEventsWeekly), "Eventos"),ex);
                throw;
            }
        }

        public async Task<MessageReturn> GetCardEventsWeekly()
        {
            try
            {
                var dic = new Dictionary<string, object>();
                DateTime startOfWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek);
                DateTime startOfRange = startOfWeek.AddDays(-1); // domingo
                DateTime endOfRange = startOfWeek.AddDays(7); // sábado

                dic.Add("StartDate", startOfRange.ToUniversalTime());
                dic.Add("Status", (int)StatusEvent.Active);
                var data = await _eventRepository.GetByFilter<EventComplet>(dic, null);
                data.Item1 = data.Item1.Where(x => x.StartDate.Date <= endOfRange.Date).ToList();
                _messageReturn.Data = new CardDto().ModelListToDtoList(data.Item1);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetCardEventsWeekly), "Eventos"),ex);
                throw;
            }
        }

        public async Task<MessageReturn> GetWithUserData()
        {
            try
            {
                var data = await _eventRepository
                    .GetByFilterComplet<EventComplet>(new Pagination() { Page = 0, PageSize = 10 }, new Event());
                _messageReturn.Data = new EventCompletWithTransactionDto().ModelListToDtoList(data);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetWithUserData), "Eventos"), ex);
                throw;
            }
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

                if (eventEdit.Image != null && eventEdit.Image.IsBase64String())
                {
                    eventEdit.Image = StoreImageAndGenerateLinkToAccess(eventEdit.Image);
                }
                _messageReturn.Data = await _eventRepository.Edit(id, eventEdit);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Edit, this.GetType().Name, nameof(EditEventsAsync), "Evento"), ex);
                throw;
            }
        }

        public async Task<MessageReturn> GetAllEventsWithTickets(string idEvent)
        {
            try
            {
                idEvent.ValidateIdMongo();
                var data = await _eventRepository.GetFilterWithTickets<EventComplet>(idEvent, string.Empty);
                _messageReturn.Data = new EventCompletWithTransactionDto().ModelListToDtoList(data);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetAllEventsWithTickets), "Evento"), ex);
                throw;
            }
        }

        public async Task<MessageReturn> GetCardEvents(FilterOptions filters, Pagination paginationOptions)
        {
            try
            {
                Dictionary<string, object> dic = GenerateFilters(filters);
                var data = await _eventRepository.GetByFilter<EventComplet>(dic, paginationOptions);
                _messageReturn.Data = new CardDto().ModelListToDtoList(data.Item1).OrderBy(c=> c.Year).ThenBy(c=> c.Month).ThenBy(c=> c.Day);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError( string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetCardEvents), "Eventos"), ex);
                throw;
            }
        }

        private string StoreImageAndGenerateLinkToAccess(string image)
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

            string linkImagem = Settings.HostImg + nomeArquivoImage;

            using (var stream = new FileStream(filePathImage, FileMode.Create))
            {
                stream.Write(imageBytes, 0, imageBytes.Length);
            }
            return linkImagem;
        }

        private static Dictionary<string, object> GenerateFilters(FilterOptions filters)
        {
            var dic = new Dictionary<string, object>();
            if (filters.Highlights != null)
            {
                dic.Add("Highlighted", filters.Highlights);
                if (filters.Highlights == true)
                {
                    DateTime startOfWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek);
                    dic.Add("StartDate", startOfWeek);
                    dic.Add("Status", (int)StatusEvent.Active);
                }
            }
            if (filters.Type != null && !string.IsNullOrEmpty(filters.Type))
                dic.Add("Type", filters.Type);
            if (filters.Name != null && !string.IsNullOrEmpty(filters.Name))
                dic.Add("Name", filters.Name);
            if (filters.IdOrganizer != null && !string.IsNullOrEmpty(filters.IdOrganizer))
                dic.Add("IdOrganizer", filters.IdOrganizer);
            if (filters.StartDate != null && filters.StartDate != DateTime.MinValue)
                dic.Add("StartDate", filters.StartDate);
            if (filters.EndDate != null && filters.EndDate != DateTime.MinValue)
                dic.Add("StartDate", filters.EndDate);
            if (!string.IsNullOrEmpty(filters.Status))
                dic.Add("Status", filters.Status ?? throw new RuleException("status não pode ser vazio."));

            return dic;
        }
    }
}