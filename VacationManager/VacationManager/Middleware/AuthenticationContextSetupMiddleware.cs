using Microsoft.AspNetCore.Identity;
using VacationManager.Core.Authentication.Abstractions;
using VacationManager.Data.Models;

namespace VacationManager.Middleware;

public class AuthenticationContextSetupMiddleware(RequestDelegate next)
{
    private RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    public async Task InvokeAsync(HttpContext httpContext)
    {
        UserManager<ApplicationUser> userManager = httpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
        ApplicationUser? user = await userManager.GetUserAsync(httpContext.User);
        Console.WriteLine("🔑 Middleware executed");

        if (user is not null)
        {
            Console.WriteLine($"User found: {user.UserName}, Role: {user.Role}");
            IAuthenticationContext authContext = httpContext.RequestServices.GetRequiredService<IAuthenticationContext>();
            authContext.Authenticate(user);
        }

        await this._next(httpContext);
    }
}
