using Microsoft.AspNetCore.Components.Authorization;

namespace FinaData.Web.Security;

public interface ICookieAuthenticationStateProvider
{
    Task<bool> CheckAuthenticatedAsync();
    Task<AuthenticationState> GetAuthenticationStateAsync();
    void NotifyAuthenticationStateChanged();
}