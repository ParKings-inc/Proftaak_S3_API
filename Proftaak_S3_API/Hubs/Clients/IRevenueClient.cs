using Proftaak_S3_API.Models;

namespace Proftaak_S3_API.Hubs.Clients
{
    public interface IRevenueClient
    {

        Task ReceiveAvailableSpaces(decimal space);

    }
}
