namespace CheckList.Shared.DTOs;

public record CheckListDto(
    int Id,
    int SetId,
    string ListName,
    string? ListDscr,
    string ActiveInd,
    int SortOrder
);
