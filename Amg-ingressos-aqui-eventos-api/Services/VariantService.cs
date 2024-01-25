using Amg_ingressos_aqui_eventos_api.Model;
using Amg_ingressos_aqui_eventos_api.Services.Interfaces;
using Amg_ingressos_aqui_eventos_api.Repository.Interfaces;
using Amg_ingressos_aqui_eventos_api.Exceptions;
using Amg_ingressos_aqui_eventos_api.Utils;
using Amg_ingressos_aqui_eventos_api.Dto;
using System.Text.RegularExpressions;
using Amg_ingressos_aqui_eventos_api.Consts;
using System.Data;
using Amg_ingressos_aqui_eventos_api.Repository;

namespace Amg_ingressos_aqui_eventos_api.Services
{
    public class VariantService : IVariantService
    {
        private readonly IVariantRepository _variantRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILotService _lotService;
        private readonly ILogger<VariantService> _logger;
        private readonly MessageReturn _messageReturn;
        private readonly ILotRepository _lotRepository;
        private readonly ITicketService _ticketService;

        public VariantService(
            IVariantRepository variantRepository,
            IWebHostEnvironment webHostEnvironment,
            ILotService lotService,
            ILogger<VariantService> logger,
            ILotRepository lotRepository,
            ITicketService ticketService
        )
        {
            _variantRepository = variantRepository;
            _lotService = lotService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _messageReturn = new MessageReturn();
            _lotRepository = lotRepository;
            _ticketService = ticketService;
        }
        public async Task<MessageReturn> SaveAsync(VariantWithLotDto variant)
        {
            try
            {
                ValidateModelSave(variant);
                if (variant.LocaleImage != string.Empty)
                {
                    variant.LocaleImage = StoreImageAndGenerateLinkToAccess(variant.LocaleImage!);
                }
                variant.Status = Enum.StatusVariant.Active;
                Variant modelVariant = new VariantWithLotDto().DtoToModel(variant);
                var variantDatabase = await _variantRepository.Save(modelVariant) ?? throw new RuleException("Id nao pode ser null");
                var IdentificateLot = 1;
                variant.Lots
                    .ToList()
                    .ForEach(i =>
                    {
                        i.Identificate = IdentificateLot;
                        i.ReqDocs = variant.ReqDocs;
                        i.IdVariant = variantDatabase.Id ?? string.Empty;
                        i.Status = IdentificateLot == 1 ? Enum.StatusLot.Open : Enum.StatusLot.Wait;
                        var lot = (Lot)_lotService.SaveAsync(i).Result.Data;
                        i.Id = lot.Id;
                        IdentificateLot++;
                    });

                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> EditAsync(List<VariantEditDto> listVariant)
        {
            try
            {
                foreach (var item in listVariant)
                {
                    var variantEdit = new Variant()
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
                        Id = item.Id,
                        IdEvent = item.IdEvent
                    };

                    if (variantEdit.Id != null)
                        _messageReturn.Data = await _variantRepository.Edit(variantEdit.Id, variantEdit);
                    else
                        _messageReturn.Data = await _variantRepository.Save(variantEdit);
                }
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Edit, this.GetType().Name, nameof(EditAsync), ex));
                throw;
            }
        }

        public async Task<MessageReturn> DeleteAsync(string id)
        {
            try
            {
                id.ValidateIdMongo();
                await _lotService.DeleteByVariantAsync(id);
                _messageReturn.Data = await _variantRepository.Delete(id);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteAsync), ex));
                _ = DeleteAsync(id);
                throw;
            }
        }

        public async Task<MessageReturn> DeleteManyAsync(List<VariantWithLotDto> listVariant)
        {
            try
            {
                listVariant.ForEach(async variant =>
                {
                    List<string> LotsId = variant.Lots.Select(d => d.Id).ToList();

                    await _lotService.DeleteManyAsync(LotsId);
                });

                List<string> variantId = listVariant.Select(v => v.Id).ToList();
                _messageReturn.Data = await _variantRepository.DeleteMany(variantId);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Delete, this.GetType().Name, nameof(DeleteManyAsync), ex));
                throw;
            }
        }

        private void ValidateModelSave(VariantWithLotDto variant)
        {
            if (variant.Name == "")
                throw new SaveException("Nome é Obrigatório.");
            if (!variant.Lots.Any())
                throw new SaveException("Lote é Obrigatório.");
            if (variant.HasPositions)
            {
                if (variant.LocaleImage == string.Empty)
                    throw new SaveException("Imagem Variante é Obrigatório.");
                if (variant.Positions.PeoplePerPositions == 0)
                    throw new SaveException("Pessoas por posição é Obrigatório.");
                if (variant.Positions.TotalPositions == 0)
                    throw new SaveException("Total de posições é Obrigatório.");
            }
        }

        public async Task<MessageReturn> SaveManyAsync(List<VariantWithLotDto> listVariant)
        {
            try
            {
                listVariant.ForEach(v =>
                {
                    ValidateModelSave(v);
                    if (!string.IsNullOrEmpty(v.LocaleImage))
                    {
                        v.LocaleImage = StoreImageAndGenerateLinkToAccess(v.LocaleImage!);
                    }
                    v.Status = Enum.StatusVariant.Active;
                });

                var listVariantModel = new VariantWithLotDto().ListDtoToListModel(listVariant);
                var listVariants = await _variantRepository.SaveMany(listVariantModel);

                listVariant.ForEach(i =>
                {
                    var IdentificateLot = 1;
                    var idVariant = listVariants?.Find(v => v.Name == i.Name)?.Id ?? string.Empty;
                    i.Lots.ForEach(l =>
                    {
                        l.IdVariant = idVariant;
                        l.Status = IdentificateLot == 1 ? Enum.StatusLot.Open : Enum.StatusLot.Wait;
                        l.Identificate = IdentificateLot;
                        l.ReqDocs = i.ReqDocs;
                        IdentificateLot++;
                    });
                    _lotService.SaveManyAsync(i.Lots);
                });

                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Save, this.GetType().Name, nameof(SaveManyAsync), ex));
                throw;
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
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0}:{1} - Erro ao gerar link imagem variante.", this.GetType().Name, nameof(StoreImageAndGenerateLinkToAccess)), ex);
                throw;
            }
        }

        public async Task<MessageReturn> ManagerVariantLotsAsync(string idLote, DateTime dateManagerLots)
        {
            try
            {
                //Consultar os lotes que encerram na data de gestão de lotes
                var lots = await _lotRepository.GetLotByEndDateSales<Lot>(dateManagerLots);

                if (lots != null && lots.Count() > 0)
                {
                    List<string> lista = new List<string>();
                    //Percorrer os lotes e executar a gestao
                    foreach (Lot lot in lots)
                    {
                        //Consultar o numero de ingressos(tickets) não vendidos do lote
                        int qtdTicketsNaoUsados = await _ticketService.GetCountTicketsNoUser(lot.Id);

                        if (qtdTicketsNaoUsados > 0)
                        {
                            //Testa se os ingressos restantes do lote, devem ser migrados o proximo lote
                            //consultar pelo IdVariant, os dados de variação do lote
                            //1 - verifica se o lote precisa ser migrado
                            //2 - Consultar se existe mais um lote para o evento
                            //Permitir que os ingressos restantes sejam vendidos no proximo lote
                            var variant = await _variantRepository.GetById(lot.IdVariant);
                            if (variant != null && variant.SellTicketsInAnotherBatch != null && variant.SellTicketsInAnotherBatch.Value)
                            {
                                //Numero de identificacao do proximo lote
                                int identificate = lot.Identificate + 1;

                                //Consulta o proximo lote desta variação
                                var novoLote = await _lotRepository.GetByFilter(new Dictionary<string, string> {
                                    { "IdVariant",lot.IdVariant } ,
                                    { "Identificate", identificate.ToString()  }
                                });

                                //Verifica se tem mais lote - Precisa ter dados ou tem erro de integradade de dados
                                if (novoLote != null && novoLote.Count() > 0)
                                {
                                    //Consulta os tickets não vendidos do lote
                                    var tickets = await _ticketService.GetRemainingByLot(lot.Id);
                                    if (tickets != null && tickets.Data != null)
                                    {
                                        //Finalizar o lote que terminou a validade (atualizar o status)
                                        var oldLot = new Dictionary<string, string>()
                                        {
                                            {"Status", Convert.ToInt32(Enum.StatusLot.Finished).ToString() },
                                            {"TotalTickets",(lot.TotalTickets - qtdTicketsNaoUsados).ToString() }
                                        };

                                        _lotRepository.EditCombine(lot.Id, oldLot).GetAwaiter().GetResult();

                                        //Inicializa o novo o lote (atualizar o status)
                                        var newLot = new Dictionary<string, string>()
                                        {
                                            {"Status", Convert.ToInt32(Enum.StatusLot.Open).ToString() },
                                            {"TotalTickets",(novoLote.FirstOrDefault().TotalTickets + qtdTicketsNaoUsados).ToString() }
                                        };

                                        _lotRepository.EditCombine(novoLote.FirstOrDefault().Id, newLot).GetAwaiter().GetResult();

                                        //Move os ingressos não utilizados para o novo lote
                                        List<Ticket> listaTicket = new List<Ticket>();
                                        listaTicket = (List<Ticket>)tickets.Data;

                                        foreach (Ticket item in listaTicket)
                                        {
                                            //Monta objeto para atualizar o lote e o valor do ticket, com os dados do novo lote
                                            var retTicket = await _ticketService.EditAsync(item.Id, new Ticket()
                                            {
                                                IdColab = item.IdColab,
                                                IdLot = novoLote.FirstOrDefault().Id, // id do novo lote, que os ingressos vao ser atualizados
                                                IdUser = item.IdUser,
                                                IsSold = item.IsSold,
                                                Position = item.Position,
                                                QrCode = item.QrCode,
                                                ReqDocs = item.ReqDocs,
                                                Status = item.Status,
                                                TicketCortesia = item.TicketCortesia,
                                                Value = novoLote.FirstOrDefault().ValueTotal
                                            });
                                        }

                                        lista.Add($"Lote: {lot.Id} - Finalizado");
                                    }
                                }
                            }
                            //Vender todo o lote antes de iniciar outro
                            else if (variant != null && variant.SellTicketsBeforeStartAnother != null && !variant.SellTicketsBeforeStartAnother.Value)
                            {
                                //Não existe mais lote - Finalizar a variação (atualizar o status)
                                //var resultVariant = await _variantRepository.ChangeStatusVariant(lot.IdVariant, (int)Enum.EnumStatusVariant.Finished);

                                //Finalizar o lote que terminou a validade ou não tem mais tickets (ingressos) para serem vendidos (atualizar o status)
                                _lotRepository.ChangeStatusLot(lot.Id, (int)Enum.StatusLot.Finished).GetAwaiter().GetResult();
                                lista.Add($"Lote: {lot.Id} - Finalizado");

                                //Numero de identificacao do proximo lote
                                int identificate = lot.Identificate + 1;

                                //Consulta o proximo lote desta variação
                                var novoLote = await _lotRepository.GetByFilter(new Dictionary<string, string> {
                                    { "IdVariant",lot.IdVariant } ,
                                    { "Identificate", identificate.ToString()  }
                                });

                                //Verifica se tem mais lote - Precisa ter dados ou tem erro de integradade de dados
                                if (novoLote != null && novoLote.Count() > 0)
                                {
                                    //Inicia o novo lote da variação
                                    _lotRepository.ChangeStatusLot(novoLote.FirstOrDefault().Id, (int)Enum.StatusLot.Open).GetAwaiter().GetResult();
                                }
                            }
                            
                        }
                        else
                        {
                            //Não existe mais lote - Finalizar a variação (atualizar o status)
                            //var resultVariant = await _variantRepository.ChangeStatusVariant(lot.IdVariant, (int)Enum.EnumStatusVariant.Finished);

                            //Finalizar o lote que terminou a validade ou não tem mais tickets (ingressos) para serem vendidos (atualizar o status)
                            _lotRepository.ChangeStatusLot(lot.Id, (int)Enum.StatusLot.Finished).GetAwaiter().GetResult();

                            lista.Add($"Lote: {lot.Id} - Finalizado");
                            //Verificar se todos os ingressos do lote for vendido
                        }

                    }
                    _messageReturn.Data = Newtonsoft.Json.JsonConvert.SerializeObject(lista);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(ManagerVariantLotsAsync), "dateManagerLots"), dateManagerLots);
                throw;
            }
            return _messageReturn;
        }
    }
}