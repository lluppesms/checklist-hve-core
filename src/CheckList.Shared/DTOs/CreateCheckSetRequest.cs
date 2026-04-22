namespace CheckList.Shared.DTOs;

public record CreateCheckSetRequest(
    string SetName,
    string? SetDscr,
    int? TemplateSetId
);
