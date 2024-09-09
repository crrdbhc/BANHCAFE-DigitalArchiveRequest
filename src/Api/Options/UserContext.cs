using System.Security.Claims;
using Banhcafe.Microservices.ComparerCoreVsAD.Core.Common.Ports;

namespace Banhcafe.Microservices.ComparerCoreVsAD.Api.Options;

public class UserContext : IUserContext
{
    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        User = httpContextAccessor.HttpContext!.User;
    }

    public ClaimsPrincipal User { get; }
}
