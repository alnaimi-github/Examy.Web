using Microsoft.AspNetCore.Mvc;
using Examy.Api.Services;
using Examy.Shared;

namespace Examy.Api.Endpoints;

public static class AdminEndPoints
{
    public static IEndpointRouteBuilder MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admin/home-data", async (AdminService adminService) =>

            Results.Ok(await adminService.GetHomeDataAsync()));
            

        var group = app.MapGroup("/api/users")
            .RequireAuthorization(p => p.RequireRole(nameof(UserRole.Admin)));

        group.MapGet("", async ([FromQuery] UserApprovedFilter filter, [FromQuery] int startIndex, [FromQuery] int pageSize, AdminService userService) =>
        {
            return Results.Ok(await userService.GetUsersAsync(filter, startIndex, pageSize));
        });

        group.MapPatch("{userId}/toggle-status", async (int userId, AdminService userService) =>
        {
            await userService.ToggleUserApprovedStatus(userId);
            return Results.Ok();
        });


        return app;
    }
}