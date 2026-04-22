namespace CheckList.Api.Endpoints;

using CheckList.Api.Data.Models;
using CheckList.Api.Repositories.Interfaces;
using CheckList.Shared.DTOs;

public static class CheckSetsEndpoints
{
    public static IEndpointRouteBuilder MapCheckSets(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sets").RequireAuthorization();

        group.MapGet("/", async (ICheckSetRepository repo, HttpContext ctx) =>
        {
            var owner = ctx.User.Identity?.Name ?? string.Empty;
            var sets = await repo.GetByOwnerAsync(owner);
            var dtos = sets.Select(s => new CheckSetDto(s.Id, s.SetName, s.SetDscr, s.OwnerName, s.ActiveInd, s.SortOrder, s.CreateDateTime));
            return Results.Ok(dtos);
        });

        group.MapGet("/{id:int}", async (int id, ICheckSetRepository repo) =>
        {
            var set = await repo.GetByIdAsync(id);
            if (set is null) return Results.NotFound();
            var dto = new CheckSetDto(set.Id, set.SetName, set.SetDscr, set.OwnerName, set.ActiveInd, set.SortOrder, set.CreateDateTime);
            return Results.Ok(dto);
        });

        group.MapPost("/", async (CreateCheckSetRequest request, ICheckSetRepository repo, HttpContext ctx) =>
        {
            var owner = ctx.User.Identity?.Name ?? string.Empty;
            var entity = new CheckSet
            {
                SetName = request.SetName,
                SetDscr = request.SetDscr,
                OwnerName = owner,
                TemplateSetId = request.TemplateSetId
            };
            var created = await repo.CreateAsync(entity);
            return Results.Created($"/api/sets/{created.Id}", new CheckSetDto(created.Id, created.SetName, created.SetDscr, created.OwnerName, created.ActiveInd, created.SortOrder, created.CreateDateTime));
        });

        group.MapPut("/{id:int}", async (int id, CreateCheckSetRequest request, ICheckSetRepository repo) =>
        {
            var existing = await repo.GetByIdAsync(id);
            if (existing is null) return Results.NotFound();
            existing.SetName = request.SetName;
            existing.SetDscr = request.SetDscr;
            var updated = await repo.UpdateAsync(existing);
            if (updated is null) return Results.NotFound();
            return Results.Ok(new CheckSetDto(updated.Id, updated.SetName, updated.SetDscr, updated.OwnerName, updated.ActiveInd, updated.SortOrder, updated.CreateDateTime));
        });

        group.MapDelete("/{id:int}", async (int id, ICheckSetRepository repo) =>
        {
            var deleted = await repo.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });

        return app;
    }
}
