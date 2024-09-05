using System.Security.Claims;

namespace Banhcafe.Microservices.DigitalArchiveRequest.Core.Common.Extensions;

public static class GeneralExtensions
{
    public static int GetUserId(this ClaimsPrincipal currentUser)
    {
        return int.Parse(currentUser.Claims.Single(c => c.Type.Equals("Id")).Value);
    }

    public static string GetUserName(this ClaimsPrincipal currentUser)
    {
        return currentUser.Claims.Single(c => c.Type.Equals("UserName")).Value;
    }

    public static int GetTerminalId(this ClaimsPrincipal currentUser)
    {
        return int.Parse(
            currentUser.Claims.Single(c => c.Type.ToLower().Equals("terminalid")).Value
        );
    }

    public static int GetClientId(this ClaimsPrincipal currentUser)
    {
        return int.Parse(currentUser.Claims.Single(c => c.Type.ToLower().Equals("clientid")).Value);
    }
}
