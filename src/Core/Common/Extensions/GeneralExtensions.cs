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
}
