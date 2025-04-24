using Examy.Shared.DTO;
using Examy.Shared;
using Refit;

[Headers("Authorization: Bearer")]
public interface IAdminApi
{
    [Get("/api/users")]
    Task<PagedResult<UserDto>> GetUsersAsync(UserApprovedFilter filter, int startIndex, int pageSize);

    [Patch("/api/users/{userId}/toggle-status")]
    Task ToggleUserApprovedStatus(int userId);

    [Get("/api/admin/home-data")]
    Task<AdminHomeDateDto> GetHomeDataAsync();
}
