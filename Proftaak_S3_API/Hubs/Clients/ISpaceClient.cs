using System.Threading.Tasks;
using Proftaak_S3_API.Models;

namespace Proftaak_S3_API.Hubs.Clients
{
    public interface ISpaceClient
    {
        Task ReceiveSpaces(List<Space> space);
    }
}
