using Amg_ingressos_aqui_eventos_api.Exceptions;

namespace Amg_ingressos_aqui_eventos_api.Utils
{
    public static class ExtensionMethods
    {
        public static void ValidateIdMongo(this string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new IdMongoException("Id é obrigatório");
            else if (id.Length < 24)
                throw new IdMongoException("Id é obrigatório e está menor que 24 digitos");
        }
        public static bool IsBase64String(this string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }
    }
}