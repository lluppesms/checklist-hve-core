namespace CheckList.Api.Endpoints;

using CheckList.Api.Repositories.Interfaces;
using CheckList.Shared.DTOs;

public static class TemplateSetsEndpoints
{
    public static IEndpointRouteBuilder MapTemplateSets(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/templates").RequireAuthorization();

        group.MapGet("/", async (ITemplateSetRepository repo) =>
        {
            var sets = await repo.GetAllAsync();
            var dtos = sets.Select(s => new TemplateSetDto(s.Id, s.SetName, s.SetDscr, s.SortOrder));
            return Results.Ok(dtos);
        });

        group.MapGet("/{id:int}", async (int id, ITemplateSetRepository repo) =>
        {
            var set = await repo.GetByIdWithChildrenAsync(id);
            if (set is null) return Results.NotFound();
            return Results.Ok(new TemplateSetDto(set.Id, set.SetName, set.SetDscr, set.SortOrder));
        });

        return app;
    }
}
