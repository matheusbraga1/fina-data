using FinaData.Api.Data;
using FinaData.Core.Common.Extensions;
using FinaData.Core.Enums;
using FinaData.Core.Handlers;
using FinaData.Core.Models;
using FinaData.Core.Requests.Transactions;
using FinaData.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace FinaData.Api.Handlers;

public class TransactionHandler(AppDbContext context) : ITransactionHandler
{
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        if (request.Type == ETransactionType.Withdraw && request.Amount >= 0)
            request.Amount *= -1;
        
        try
        {
            var transaction = new Transaction
            {
                UserId = request.UserId,
                Title = request.Title,
                Type = request.Type,
                Amount = request.Amount,
                CategoryId = request.CategoryId,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                CreatedAt = DateTime.Now
            };

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, 201, "Transação criada com sucesso");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Não foi possível criar a transação");
        }
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transação não encontrada");

            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, message: "Transação excluída com sucesso");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Não foi possível excluir a transação");
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var transaction = await context.Transactions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            return transaction is null
                ? new Response<Transaction?>(null, 404, "Transação não encontrada")
                : new Response<Transaction?>(transaction, 200, "Transação encontrada com sucesso");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Erro ao buscar transação");
        }
    }

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransctionsByPeriodRequest request)
    {
        try
        {
            request.StartDate ??= DateTime.Now.GetFirstDay();
            request.EndDate ??= DateTime.Now.GetLastDay();
        }
        catch
        {
            return new PagedResponse<List<Transaction>?>(null, 500, "Não foi possível determinar a data de início ou término");
        }

        try
        {
            var query = context
            .Transactions
            .AsNoTracking()
            .Where(x =>
            x.PaidOrReceivedAt >= request.StartDate &&
            x.PaidOrReceivedAt <= request.EndDate &&
            x.UserId == request.UserId)
            .OrderBy(x => x.PaidOrReceivedAt);

            var transactions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<List<Transaction>?>(transactions, count, request.PageNumber, request.PageSize);
        }
        catch
        {
            return new PagedResponse<List<Transaction>?>(null, 500, "Não foi possível consultar as transações");
        }
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        if (request.Type == ETransactionType.Withdraw && request.Amount >= 0)
            request.Amount *= -1;
        
        try
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (transaction is null)
                return new Response<Transaction?>(null, 404, "Transação não encontrada");

            transaction.Title = request.Title;
            transaction.Type = request.Type;
            transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;
            transaction.Amount = request.Amount;
            transaction.CategoryId = request.CategoryId;

            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(transaction, 200, "Transação atualizada com sucesso");
        }
        catch
        {
            return new Response<Transaction?>(null, 500, "Não foi possível atualizar a transação");
        }
    }
}
