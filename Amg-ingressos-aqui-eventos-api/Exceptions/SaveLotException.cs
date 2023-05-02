namespace Amg_ingressos_aqui_eventos_api.Exceptions
{
    public class SaveLotException : Exception
    {

        public SaveLotException()
        {
        }

        public SaveLotException(string message)
            : base(message)
        {
        }

        public SaveLotException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}