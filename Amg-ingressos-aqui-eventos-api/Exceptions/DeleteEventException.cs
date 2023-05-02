namespace Amg_ingressos_aqui_eventos_api.Exceptions
{
    public class DeleteEventException : Exception
    {
        public DeleteEventException()
        {
        }

        public DeleteEventException(string message)
            : base(message)
        {
        }

        public DeleteEventException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}