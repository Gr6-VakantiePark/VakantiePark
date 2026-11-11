using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace VakantiePark.UI.Authentication
{
    public class AuthStateProvider : AuthenticationStateProvider, IDisposable
    {
        private const string TokenKey = "authToken";

        private readonly SessionStorageService _sessionStorageService;

        private readonly AuthenticationState _anonymous =
            new(new ClaimsPrincipal(new ClaimsIdentity()));

        private AuthenticationState? _currentState;
        private Timer? _expiryTimer;

        public AuthStateProvider(SessionStorageService sessionStorageService)
        {
            _sessionStorageService = sessionStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_currentState != null)
                return _currentState;

            try
            {
                string? token = await _sessionStorageService.GetItemAsync(TokenKey);

                if (string.IsNullOrWhiteSpace(token))
                {
                    _currentState = _anonymous;
                    return _currentState;
                }

                List<Claim> claims = ParseJWT.GetClaims(token);
                DateTimeOffset? expiresAt = ExpiresAt(claims);

                if (expiresAt is null || expiresAt <= DateTimeOffset.UtcNow)
                {
                    await _sessionStorageService.RemoveItemAsync(TokenKey);
                    _currentState = _anonymous;
                    return _currentState;
                }

                SetState(claims, expiresAt.Value);
                return _currentState!;
            }
            catch
            {
                _currentState = _anonymous;
                return _currentState;
            }
        }

        public async Task MarkUserAsAuthenticatedAsync(string token)
        {
            await _sessionStorageService.SetItemAsync(TokenKey, token);

            List<Claim> claims = ParseJWT.GetClaims(token);
            SetState(claims, ExpiresAt(claims) ?? DateTimeOffset.UtcNow);

            NotifyAuthenticationStateChanged(Task.FromResult(_currentState!));
        }

        public async Task MarkUserAsLoggedOutAsync()
        {
            await _sessionStorageService.RemoveItemAsync(TokenKey);

            _expiryTimer?.Dispose();
            _expiryTimer = null;
            _currentState = _anonymous;

            NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _sessionStorageService.GetItemAsync(TokenKey);
        }

        private void SetState(List<Claim> claims, DateTimeOffset expiresAt)
        {
            _currentState = new AuthenticationState(BuildUser(claims));

            _expiryTimer?.Dispose();
            TimeSpan due = expiresAt - DateTimeOffset.UtcNow;
            _expiryTimer = new Timer(_ => _ = MarkUserAsLoggedOutAsync(), null, due < TimeSpan.Zero ? TimeSpan.Zero : due, Timeout.InfiniteTimeSpan);
        }

        private static DateTimeOffset? ExpiresAt(List<Claim> claims)
        {
            Claim? exp = claims.FirstOrDefault(c => c.Type == "exp");

            if (exp == null || !long.TryParse(exp.Value, out long seconds))
                return null;

            return DateTimeOffset.FromUnixTimeSeconds(seconds);
        }

        private static ClaimsPrincipal BuildUser(List<Claim> claims)
        {
            string roleClaimType =
                claims.Any(c => c.Type == ClaimTypes.Role)
                    ? ClaimTypes.Role
                    : "role";

            string nameClaimType =
                claims.Any(c => c.Type == ClaimTypes.Name)
                    ? ClaimTypes.Name
                    : "name";

            return new ClaimsPrincipal(
                new ClaimsIdentity(
                    claims,
                    "jwt",
                    nameClaimType,
                    roleClaimType));
        }

        public void Dispose()
        {
            _expiryTimer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
