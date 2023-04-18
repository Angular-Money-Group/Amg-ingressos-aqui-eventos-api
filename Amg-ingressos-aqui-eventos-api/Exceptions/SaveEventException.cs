namespace Amg_ingressos_aqui_eventos_api.Exceptions
{
    public class SaveEventException : Exception
    {

        public SaveEventException()
        {
        }

        public SaveEventException(string message)
            : base(message)
        {
        }

        public SaveEventException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}