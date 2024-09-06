using FinaData.Api.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FinaData.Api.Common.Api;

public static class AppExtension
{
    public static void UseSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    public static void ConfigureDevEnvironment(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapSwagger().RequireAuthorization();
    }
}
