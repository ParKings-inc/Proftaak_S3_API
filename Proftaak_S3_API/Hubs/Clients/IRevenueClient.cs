using Proftaak_S3_API.Models;
using System.Threading.Tasks;

namespace Proftaak_S3_API.Hubs.Clients
{
    public interface IRevenueClient
    {

        Task ReceiveRevenue(decimal revenue);

    }
}
