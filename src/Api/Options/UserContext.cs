using System.Security.Claims;
using Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Ports;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Api.Options;

public class UserContext : IUserContext
{
    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        User = httpContextAccessor.HttpContext!.User;
    }

    public ClaimsPrincipal User { get; }
}
