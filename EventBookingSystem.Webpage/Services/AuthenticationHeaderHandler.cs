using Blazored.LocalStorage;

namespace EventBookingSystem.Webpage.Services
{
    public class AuthenticationHeaderHandler : DelegatingHandler
    {
        private readonly IClientAuthService _authService;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationHeaderHandler(IClientAuthService authService, ILocalStorageService localStorage)
        {
            _authService = authService;
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Try to get the expiration time
            var expiryDate = await _localStorage.GetItemAsync<DateTime>("tokenExpiration");
            var token = await _localStorage.GetItemAsync<string>("authToken");

            // If token is about to expire (within 2 minutes), refresh it
            if (!string.IsNullOrEmpty(token) && expiryDate < DateTime.Now.AddMinutes(2))
            {
                var refreshResult = await _authService.RefreshTokenAsync();
                if (!refreshResult.Success)
                {
                    // If refresh fails, handle accordingly (could redirect to login)
                    await _authService.LogoutAsync();
                }
            }

            // Get the token again (it might have been refreshed)
            token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
