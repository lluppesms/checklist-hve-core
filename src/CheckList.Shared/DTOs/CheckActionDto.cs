namespace CheckList.Shared.DTOs;

public record CheckActionDto(
    int Id,
    int CategoryId,
    int ListId,
    int SetId,
    string ActionText,
    string? ActionDscr,
    string CompleteInd,
    string? CompletedBy,
    DateTime? CompletedAt,
    int SortOrder
);
