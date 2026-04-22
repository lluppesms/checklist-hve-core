namespace CheckList.Shared.DTOs;

public record CheckSetDto(
    int Id,
    string SetName,
    string? SetDscr,
    string? OwnerName,
    string ActiveInd,
    int SortOrder,
    DateTime CreateDateTime
);
