namespace CheckList.Api.Endpoints;

using CheckList.Api.Repositories.Interfaces;
using CheckList.Shared.DTOs;

public static class CheckListsEndpoints
{
    public static IEndpointRouteBuilder MapCheckLists(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sets/{setId:int}/lists").RequireAuthorization();

        group.MapGet("/", async (int setId, ICheckListRepository repo) =>
        {
            var lists = await repo.GetBySetAsync(setId);
            var dtos = lists.Select(l => new CheckListDto(l.Id, l.SetId, l.ListName, l.ListDscr, l.ActiveInd, l.SortOrder));
            return Results.Ok(dtos);
        });

        group.MapGet("/{id:int}", async (int setId, int id, ICheckListRepository repo) =>
        {
            var list = await repo.GetByIdAsync(id);
            if (list is null || list.SetId != setId) return Results.NotFound();
            return Results.Ok(new CheckListDto(list.Id, list.SetId, list.ListName, list.ListDscr, list.ActiveInd, list.SortOrder));
        });

        return app;
    }
}
