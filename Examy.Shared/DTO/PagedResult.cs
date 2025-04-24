namespace Examy.Shared.DTO;

public record PagedResult<TRecord>(TRecord[] Records, int TotalCount);