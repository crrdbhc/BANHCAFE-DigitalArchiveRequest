using System.Security.Claims;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Ports;

/// <summary>
/// access user claims from JWT
/// </summary>
public interface IUserContext
{
    public ClaimsPrincipal User { get; }
}
