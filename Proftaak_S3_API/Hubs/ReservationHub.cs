using Microsoft.AspNetCore.SignalR;
using Proftaak_S3_API.Hubs.Clients;

namespace Proftaak_S3_API.Hubs
{
    public class ReservationHub : Hub<IReservationClient>
    {
    }
}
