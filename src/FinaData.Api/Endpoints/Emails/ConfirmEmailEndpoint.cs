using FinaData.Api.Common.Api;
using FinaData.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace FinaData.Api.Endpoints.Emails
{
    public class ConfirmEmailEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/confirm-email", HandleAsync)
                .WithName("Confirma Email")
                .WithTags("Emails")
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);

        private static async Task<IResult> HandleAsync(
            long userId, 
            string token, 
            UserManager<User> userManager)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return Results.BadRequest("Usuário não encontrado.");

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return Results.BadRequest(new { Errors = errors });
            }

            return Results.Ok("E-mail confirmado com sucesso!");
        }
    }
}
