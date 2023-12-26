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
                _messageReturn.Data = new EventCompletWithTransactionDto().ModelToDto(data[0]);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.GetById, this.GetType().Name, nameof(GetByIdAsync), "Evento"), id);
                throw;
            }
        }

        public async Task<MessageReturn> SaveAsync(EventCompletDto eventObject)
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
                Event modelEvent = new EventCompletDto().DtoToModel(eventObject);
                await _eventRepository.Save(modelEvent);

                eventObject.Variants
                    .ToList()
                    .ForEach(i =>
                    {
                        i.IdEvent = modelEvent.Id ?? string.Empty;
                        _ = _variantService.SaveAsync(i);

                        if (i.QuantityCourtesy > 0)
                        {
                            var RemainingCourtesy = new RemainingCourtesy
                            {
                                VariantId = i.Id,
                                VariantName = i.Name,
                                Quantity = i.QuantityCourtesy
                            };

                            modelEvent.Courtesy.RemainingCourtesy.Add(RemainingCourtesy);
                        }
                    });

                _messageReturn.Data = await _eventRepository.Edit(modelEvent.Id, modelEvent);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), "Evento"), eventObject);
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
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(SetHighlightEventAsync), "Eventos Destaque"), id);
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
                _logger.LogError(ex, string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteAsync), "Evento"), id);
                throw;
            }
        }

        public async Task<MessageReturn> GetEventsAsync(FilterOptions filters, Pagination paginationOptions)
        {
            try
            {
                Dictionary<string, object> dic = GenerateFilters(filters);
                var data = await _eventRepository.GetByFilter<EventComplet>(dic, paginationOptions);
                _messageReturn.Data = new EventCompletWithTransactionDto().ModelListToDtoList(data);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetEventsAsync), "Eventos"));
                throw;
            }
        }

        public async Task<MessageReturn> GetCardEventsHighligth()
        {
            try
            {
                var dic = GenerateFilters(new FilterOptions() { Highlights = true });
                var data = await _eventRepository.GetByFilter<EventComplet>(dic, null);
                _messageReturn.Data = new CardDto().ModelListToDtoList(data);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetCardEventsWeekly), "Eventos"));
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
                dic.Add("Status", (int)EnumStatusEvent.Active);
                var data = await _eventRepository.GetByFilter<EventComplet>(dic, null);
                data = data.Where(x => x.StartDate.Date <= endOfRange.Date).ToList();
                _messageReturn.Data = new CardDto().ModelListToDtoList(data);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetCardEventsWeekly), "Eventos"));
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
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetWithUserData), "Eventos"));
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

                if (eventEdit.Image != null && IsBase64String(eventEdit.Image))
                {
                    IsBase64Image(eventEdit.Image!);
                    eventEdit.Image = StoreImageAndGenerateLinkToAccess(eventEdit.Image!);
                }
                _messageReturn.Data = await _eventRepository.Edit(id, eventEdit);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Edit, this.GetType().Name, nameof(EditEventsAsync), "Evento"));
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
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetAllEventsWithTickets), "Evento"));
                throw;
            }
        }

        public async Task<MessageReturn> GetCardEvents(FilterOptions filters, Pagination paginationOptions)
        {
            try
            {
                Dictionary<string, object> dic = GenerateFilters(filters);
                var data = await _eventRepository.GetByFilter<EventComplet>(dic, paginationOptions);
                _messageReturn.Data = new CardDto().ModelListToDtoList(data);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetCardEvents), "Eventos"));
                throw;
            }
        }

        private void ValidateModelSave(EventCompletDto eventSave)
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
            if (!eventSave.Variants.Any())
                throw new SaveException("Variante é Obrigatório.");
        }

        private static bool IsBase64String(string base64)
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
                Convert.FromBase64String(base64Data);
            }
            catch (FormatException)
            {
                throw new SaveException("Essa imagem não está em base64");
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

            string linkImagem = "https://api.ingressosaqui.com/imagens/" + nomeArquivoImage;

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
                    dic.Add("Status", (int)EnumStatusEvent.Active);
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

            //filtro default
            if (dic.Count == 0)
            {
                DateTime startOfWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek);
                dic.Add("StartDate", startOfWeek);
                dic.Add("Status", (int)EnumStatusEvent.Active);
            }

            return dic;
        }
    }
}