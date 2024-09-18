using System.Security.Claims;
using FinaData.Api.Common.Api;
using FinaData.Core.Handlers;
using FinaData.Core.Models.Reports;
using FinaData.Core.Requests.Reports;
using FinaData.Core.Responses;

namespace FinaData.Api.Endpoints.Reports;

public class GetExpensesByCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/expenses", HandleAsync).Produces<Response<List<ExpensesByCategory>?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IReportHandler handler)
    {
        var request = new GetExpensesByCategoryRequest
        {
            UserId = user.Identity?.Name ?? string.Empty
        };
        
        var result = await handler.GetExpensesByCategoryReportAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest(result);
    }
}