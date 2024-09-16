using FinaData.Core.Handlers;
using FinaData.Core.Models;
using FinaData.Core.Requests.Categories;
using FinaData.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinaData.Web.Pages.Transactions;

public partial class EditTransactionPage : ComponentBase
{
    #region Properties

    [Parameter]
    public string Id { get; set; } = string.Empty;
    public bool IsBusy { get; set; } =  false;
    public UpdateTransactionRequest InputModel { get; set; } = new();
    public List<Category> Categories { get; set; } = [];

    #endregion
    
    #region Services

    [Inject]
    private ITransactionHandler TransactionHandlerHandler { get; set; } = null!;
    
    [Inject]
    private ICategoryHandler CategoryHandler { get; set; } = null!;
    
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;
    
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    #endregion
    
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;

        await GetTransactionsAsync();
        await GetCategoriesAsync();
        
        IsBusy = false;
    }

    #endregion

    #region Public Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await TransactionHandlerHandler.UpdateAsync(InputModel);

            if (result.IsSuccess)
            {
                Snackbar.Add($"Lançamento atualizado", Severity.Success);
                NavigationManager.NavigateTo("/releases/history");
            }
            else
            {
                Snackbar.Add(result.Message, Severity.Error);
            }
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

    #region Private Methods

    private async Task GetTransactionsAsync()
    {
        IsBusy = true;

        try
        {
            var request = new GetTransactionByIdRequest { Id = long.Parse(Id) };
            var result = await TransactionHandlerHandler.GetByIdAsync(request);

            if (result.IsSuccess && result.Data is not null)
            {
                InputModel = new UpdateTransactionRequest
                {
                    CategoryId = result.Data.CategoryId,
                    PaidOrReceivedAt = result.Data.PaidOrReceivedAt,
                    Title = result.Data.Title,
                    Type = result.Data.Type,
                    Amount = result.Data.Amount,
                    Id = result.Data.Id
                };
            }
                
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
    
    private async Task GetCategoriesAsync()
    {
        IsBusy = true;

        try
        {
            var request = new GetAllCategoriesRequest();
            var result = await CategoryHandler.GetAllAsync(request);

            if (result.IsSuccess)
            {
                Categories = result.Data ?? [];
                InputModel.CategoryId = Categories.FirstOrDefault()?.Id ?? 0;
            }
                
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