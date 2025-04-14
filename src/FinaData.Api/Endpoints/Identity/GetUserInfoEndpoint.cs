using System.Security.Claims;
using FinaData.Api.Common.Api;
using FinaData.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace FinaData.Api.Endpoints.Identity
{
    public class GetUserInfoEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/manage/info", HandleAsync)
                .RequireAuthorization()
                .WithName("Identity: UserInfo")
                .Produces<User>();

        private static Task<IResult> HandleAsync(
            ClaimsPrincipal claimsPrincipal,
            UserManager<User> userManager)
        {
            var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Task.FromResult(Results.Unauthorized());

            return GetUser();

            async Task<IResult> GetUser()
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user is null) return Results.Unauthorized();

                var dto = new FinaData.Core.Models.Account.User
                {
                    Email = user.Email ?? string.Empty,
                    IsEmailConfirmed = user.EmailConfirmed,
                    Claims = claimsPrincipal.Claims.ToDictionary(c => c.Type, c => c.Value)
                };

                return Results.Ok(dto);
            }
        }
    }
}
