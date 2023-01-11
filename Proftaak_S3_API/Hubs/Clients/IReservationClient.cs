using Proftaak_S3_API.Models;
using System.Threading.Tasks;

namespace Proftaak_S3_API.Hubs.Clients
{
    public interface IReservationClient
    {
        Task ReceiveReservation(List<Reservations> reservations);
        Task ReceiveUpdatedStatus(List<Reservations> reservations);
    }
}
