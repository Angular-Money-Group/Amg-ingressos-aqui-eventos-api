namespace Amg_ingressos_aqui_eventos_api.Model.Querys
{
    public class GetTicketDataUser : Ticket
    {
        public GetTicketDataUser()
        {
            User = new List<User>();
            Lot = new List<Lot>();
        }

        public List<User> User { get; set; }
        public List<Lot> Lot { get; set; }
    }
}