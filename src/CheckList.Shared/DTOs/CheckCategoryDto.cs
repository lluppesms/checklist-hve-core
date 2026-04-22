namespace CheckList.Shared.DTOs;

public record CheckCategoryDto(
    int Id,
    int ListId,
    string CategoryText,
    string? CategoryDscr,
    int SortOrder,
    IList<CheckActionDto> Actions
);
