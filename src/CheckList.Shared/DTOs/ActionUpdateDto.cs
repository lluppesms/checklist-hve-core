namespace CheckList.Shared.DTOs;

public record ActionUpdateDto(
    int SetId,
    int ListId,
    int ActionId,
    string CompleteInd,
    string? CompletedBy,
    DateTime? CompletedAt
);
