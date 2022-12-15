using Proftaak_S3_API.Hubs.Clients;
using Microsoft.AspNetCore.SignalR;

namespace Proftaak_S3_API.Hubs
{
    public class SpaceHub : Hub<ISpaceClient>
    {
    }
}
