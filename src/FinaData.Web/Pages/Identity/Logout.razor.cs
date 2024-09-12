using FinaData.Core.Handlers;
using FinaData.Web.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinaData.Web.Pages.Identity;

public partial class LogoutPage : ComponentBase
{
    #region Dependencies

    [Inject] 
    private IAccountHandler Handler { get; set; } = null!;

    [Inject] 
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject] 
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject] 
    private ICookieAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    #endregion
    
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        if (await AuthenticationStateProvider.CheckAuthenticatedAsync())
        {
            await Handler.LogoutAsync();
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            AuthenticationStateProvider.NotifyAuthenticationStateChanged();
        }
        
        await base.OnInitializedAsync();
    }

    #endregion
}