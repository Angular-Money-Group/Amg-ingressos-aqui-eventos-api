namespace Amg_ingressos_aqui_eventos_api.Exceptions
{
    public class ReportException : Exception
    {
        public ReportException()
        {
        }

        public ReportException(string message)
            : base(message)
        {
        }

        public ReportException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}