using System.Security.Claims;
namespace VakantiePark.API
{
    public static class ApiMessages
    {
        public const string Unexpected = "Er was een onverwachte fout.";
        public const string NotLoggedIn = "U bent niet ingelogd.";
        public const string Forbidden = "U heeft geen toegang tot deze functie.";
        public const string InvalidRequest = "Dit verzoek is invalide.";
        public const string TooManyRequests = "Te veel verzoeken. Probeer het over een minuut opnieuw.";
    }


    public static class ClaimsPrincipalExtensions
    {
        public static bool TryGetUserId(this ClaimsPrincipal? user, out Guid userId)
        {
            userId = Guid.Empty;
            if (user is null)
                return false;

            return Guid.TryParse(user.FindFirstValue("sub") ?? user.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
        }
    }
}
