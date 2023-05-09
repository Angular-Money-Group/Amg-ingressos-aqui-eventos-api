using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Consts
{
    public static class MessageLogErrors
    {
        public const string saveEventMessage = "SaveEventAsync : Erro inesperado ao salvar um evento";
        public const string deleteEventMessage = "DeleteEventAsync : Erro inesperado ao deletar um evento";
        public const string FindByIdEventMessage = "FindByIdEventAsync : Erro inesperado ao buscar um evento";
        public const string findTicketByUser = "FindByIdEventAsync : Erro inesperado ao buscar os Ingressos ";
        public const string GetAllEventMessage = "GetAllEventsAsync : Erro inesperado ao buscar eventos";
        public const string UpdateTickets = "UpdateTickets : Não foi possivel alterar o ingresso ";
        public const string NotModificateTickets = "NotModificateTickets : O ticket não foi atualizado. ";
    }
}