namespace Amg_ingressos_aqui_eventos_api.Consts
{
    public static class MessageLogErrors
    {
        public const string SaveMessage = "{0} : Erro inesperado ao salvar um(a) {1}";
        public const string DeleteMessage = "{0} : Erro inesperado ao deletar um(a) {1}";
        public const string GetById = "{0}:{1} - erro ao buscar {2} por Id.";
        public const string Get = "{0}:{1} - erro ao buscar {2}.";
        public const string Save = "{0}:{1} - erro ao salvar {2}.";
        public const string Delete = "{0}:{1} - erro ao apagar {2}.";
        public const string Edit = "{0}:{1} - erro ao editar {2}.";
        public const string Report = "{0}:{1} - erro ao gerar relatório {2}.";
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