using FinaData.Core.Handlers;
using FinaData.Core.Models.Reports;
using FinaData.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FinaData.Web.Components.Reports;

public partial class FinancialSummaryChartComponent : ComponentBase
{
    #region Properties

    public bool ShowValues { get; set; } = true;
    public FinancialSummary? Summary { get; set; }

    #endregion

    #region Services

    [Inject]
    public IReportHandler Handler { get; set; } = null!;
 
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        var request = new GetFinancialSummaryRequest();
        var result = await Handler.GetFinancialSummaryReportAsync(request);

        if (result.IsSuccess)
            Summary = result.Data;
    }

    #endregion

    #region Methods

    public void ToggleShowValues() => ShowValues = !ShowValues;

    #endregion
}