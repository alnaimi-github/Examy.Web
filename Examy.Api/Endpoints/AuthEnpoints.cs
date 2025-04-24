using Examy.Api.Services;
using Examy.Shared.DTO;

namespace Examy.Api.Endpoints
{
    public static class AuthEnpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/auth/login", async (AuthService authService, LoginDto dto) =>

            Results.Ok(await authService.LoginAsync(dto))
            );

            app.MapPost("/api/auth/register", async (AuthService authService, RegisterDto dto) =>
            {

                return Results.Ok(await authService.RegisterAsync(dto));
            });

            return app;
        }
    }
}
