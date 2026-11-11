using System.Security.Claims;
namespace VakantiePark.API
{
    public static class ApiMessages
    {
        public const string Unexpected = "Er was een onverwachte fout.";
        public const string NotLoggedIn = "U bent niet ingelogd.";
        public const string Forbidden = "U heeft geen toegang tot deze functie.";
        public const string InvalidRequest = "Dit verzoek is invalide.";
    }


    public static class ClaimsPrincipalExtensions
    {
        public static bool TryGetUserId(this ClaimsPrincipal user, out Guid userId) =>
            Guid.TryParse(user.FindFirstValue("sub") ?? user.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
    }
}
