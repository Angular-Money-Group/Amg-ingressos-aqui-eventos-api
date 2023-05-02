namespace Amg_ingressos_aqui_eventos_api.Exceptions
{
    public class SaveVariantException : Exception
    {

        public SaveVariantException()
        {
        }

        public SaveVariantException(string message)
            : base(message)
        {
        }

        public SaveVariantException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}