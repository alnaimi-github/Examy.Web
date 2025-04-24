using System.Text.Json.Serialization;

namespace Examy.Shared.DTO;

public record class AuthResponseDto(LoggedInUser User, string? ErrorMessage = null)
{
    [JsonIgnore]
    public bool HasError => ErrorMessage != null;
}