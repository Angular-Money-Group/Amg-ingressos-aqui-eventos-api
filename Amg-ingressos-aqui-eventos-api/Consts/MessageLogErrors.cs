using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Consts
{
    public static class MessageLogErrors
    {
        public const string SaveMessage = "{0} : Erro inesperado ao salvar um(a) {1}";
        public const string DeleteMessage = "{0} : Erro inesperado ao deletar um(a) {1}";
        public const string saveEventMessage = "SaveEventAsync : Erro inesperado ao salvar um evento";
        public const string highlightEventmessage = "HighlightEventAsync : Erro inesperado ao destacar um evento";
        public const string deleteEventMessage = "DeleteEventAsync : Erro inesperado ao deletar um evento";
        public const string EditEventMessage = "EditEventAsync : Erro inesperado ao Editar um evento";
        public const string FindByIdEventMessage = "FindByIdEventAsync : Erro inesperado ao buscar um evento";
        public const string findTicketByUser = "FindByIdEventAsync : Erro inesperado ao buscar os Ingressos ";
        public const string GetAllEventMessage = "GetAllEventsAsync : Erro inesperado ao buscar eventos";
        public const string UpdateTickets = "UpdateTickets : Não foi possivel alterar o ingresso ";
        public const string NotModificateTickets = "NotModificateTickets : O ticket não foi atualizado. ";
        public const string saveVariantMessage = "SaveVariantAsync : Erro inesperado ao salvar uma variante";
        public const string deleteVariantMessage = "DeleteVariantAsync : Erro inesperado ao deletar uma variante";
        public const string entranceTicket = "entranceAsync: Erro inesperado ao ler o ticket";
        public const string objectInvalid = "Objeto inválido ou nulo";
    }
}