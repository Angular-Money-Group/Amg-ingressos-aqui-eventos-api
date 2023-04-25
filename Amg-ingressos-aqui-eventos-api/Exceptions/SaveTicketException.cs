namespace Amg_ingressos_aqui_eventos_api.Exceptions
{
    public class SaveTicketException : Exception
    {

        public SaveTicketException()
        {
        }

        public SaveTicketException(string message)
            : base(message)
        {
        }

        public SaveTicketException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}