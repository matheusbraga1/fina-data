using FinaData.Api.Common.Api;
using FinaData.Api.Models;
using FinaData.Core.Handlers;
using FinaData.Core.Requests.Account;
using Microsoft.AspNetCore.Identity;

namespace FinaData.Api.Endpoints.Identity
{
    public class RegisterEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/register", HandleAsync)
                .WithName("Register")
                .WithSummary("Registra um novo usuário")
                .WithDescription("Registra um novo usuário")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

        private static async Task<IResult> HandleAsync(
            RegisterRequest request,
            UserManager<User> userManager,
            IEmailHandler emailHandler)
        {
            if (request.Password != request.ConfirmPassword)
                return Results.BadRequest("A senha e a confirmação de senha não coincidem.");

            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return Results.BadRequest("E-mail já está sendo utilizado.");

            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = false
            };

            var createResult = await userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(e => e.Description);
                return Results.BadRequest(errors);
            }

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = $"http://localhost:5135/v1/emails/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";

            await emailHandler.SendAsync(
                to: user.Email,
                subject: "Confirme seu e-mail",
                confirmationLink
            );

            return Results.Ok("Usuário cadastrado com sucesso! Verifique seu e-mail para confirmar a conta.");
        }
    }
}
