using FinaData.Api.Common.Api;
using FinaData.Api.Models;
using FinaData.Core.Requests.Account;
using Microsoft.AspNetCore.Identity;

namespace FinaData.Api.Endpoints.Identity
{
    public class LoginEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/login", HandleAsync)
                .WithTags("Identity")
                .WithName("Login")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

        private static async Task<IResult> HandleAsync(
            LoginRequest request,
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Results.BadRequest("Credenciais inválidas.");

            //if (!user.EmailConfirmed)
            //    return Results.BadRequest("E-mail não confirmado.");

            var result = await signInManager.PasswordSignInAsync(user, request.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
                return Results.BadRequest("Credenciais inválidas.");

            return Results.Ok("Login realizado com sucesso.");
        }
    }
}
