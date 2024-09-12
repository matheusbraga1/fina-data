using FinaData.Core.Handlers;
using FinaData.Core.Requests.Account;
using FinaData.Web.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinaData.Web.Pages.Identity;

public partial class LoginPage : ComponentBase
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
    
    #region Properties
    
    public bool IsBusy { get; set; } = false;
    public LoginRequest InputModel { get; set; } = new();
    
    #endregion
    
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        
        if (user.Identity is not null && user.Identity.IsAuthenticated)
            NavigationManager.NavigateTo("/");
    }

    #endregion
    
    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        
        try
        {
            var result = await Handler.LoginAsync(InputModel);

            if (result.IsSuccess)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                AuthenticationStateProvider.NotifyAuthenticationStateChanged();
                NavigationManager.NavigateTo("/");
            }
            else
                Snackbar.Add(result.Message, Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
}