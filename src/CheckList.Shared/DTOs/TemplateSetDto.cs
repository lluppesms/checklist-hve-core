namespace CheckList.Shared.DTOs;

public record TemplateSetDto(
    int Id,
    string SetName,
    string? SetDscr,
    int SortOrder
);
