using System.Net.Http.Json;
using FinaData.Core.Common.Extensions;
using FinaData.Core.Handlers;
using FinaData.Core.Models;
using FinaData.Core.Requests.Transactions;
using FinaData.Core.Responses;

namespace FinaData.Web.Handlers;

public class TransactionHandler(IHttpClientFactory httpClientFactory) : ITransactionHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(Configuration.HttpClientName);
    
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/transactions", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() 
               ?? new Response<Transaction?>(null, 400, "Falha ao criar uma transação");
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        var result = await _client.PutAsJsonAsync($"v1/transactions/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() 
               ?? new Response<Transaction?>(null, 400, "Falha ao atualizar categoria");
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        var result = await _client.DeleteAsync($"v1/transactions/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() 
               ?? new Response<Transaction?>(null, 400, "Falha ao excluir categoria");
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request) =>
        await _client.GetFromJsonAsync<Response<Transaction?>>("v1/transactions/" + request.Id) 
            ?? new Response<Transaction?>(null, 400, "Não foi possível obter a transação");  

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransctionsByPeriodRequest request)
    {
        const string format = "yyyy-MM-dd";
        var startDate = request.StartDate is not null
            ? request.StartDate.Value.ToString(format)
            : DateTime.Now.GetFirstDay().ToString(format);
        
        var endDate = request.EndDate is not null
            ? request.EndDate.Value.ToString(format)
            : DateTime.Now.GetLastDay().ToString(format);
        
        var url = $"v1/transactions?startDate={startDate}&endDate={endDate}";

        return await _client.GetFromJsonAsync<PagedResponse<List<Transaction>?>>(url) 
            ?? new PagedResponse<List<Transaction>?>(null, 400, "Não foi possível obter as transações");
    }
}