using System.Threading.Tasks;
using Proftaak_S3_API.Models;

namespace Proftaak_S3_API.Hubs.Clients
{
    public interface ISpaceClient
    {
        Task ReceiveAvailableSpaces(List<Space> space);
    }
}
