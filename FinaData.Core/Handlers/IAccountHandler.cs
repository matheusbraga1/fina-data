using FinaData.Core.Requests;
using FinaData.Core.Requests.Account;
using FinaData.Core.Responses;

namespace FinaData.Core.Handlers;

public interface IAccountHandler
{
    Task<Response<string>> LoginAsync(LoginRequest request);
    Task<Response<string>> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
}