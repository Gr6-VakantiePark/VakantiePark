using System.Net;
using System.Net.Http.Headers;

namespace VakantiePark.UI.Authentication
{
    public class AuthHandler : DelegatingHandler
    {
        private readonly AuthStateProvider _authStateProvider;

        public AuthHandler(AuthStateProvider authStateProvider)
        {
            _authStateProvider = authStateProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string? token = await _authStateProvider.GetTokenAsync();

            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized && !string.IsNullOrWhiteSpace(token))
                await _authStateProvider.MarkUserAsLoggedOutAsync();

            return response;
        }
    }
}
