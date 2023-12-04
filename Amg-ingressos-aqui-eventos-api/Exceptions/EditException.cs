namespace Amg_ingressos_aqui_eventos_api.Exceptions
{
    public class EditException : Exception
    {
        public EditException()
        {
        }

        public EditException(string message)
            : base(message)
        {
        }

        public EditException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}