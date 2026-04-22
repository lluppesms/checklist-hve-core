namespace CheckList.Api.Endpoints;

using CheckList.Api.Hubs;
using CheckList.Api.Repositories.Interfaces;
using CheckList.Shared.DTOs;
using Microsoft.AspNetCore.SignalR;

public static class CheckActionsEndpoints
{
    public static IEndpointRouteBuilder MapCheckActions(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/actions").RequireAuthorization();

        group.MapGet("/category/{categoryId:int}", async (int categoryId, ICheckActionRepository repo) =>
        {
            var actions = await repo.GetByCategoryAsync(categoryId);
            var dtos = actions.Select(a => new CheckActionDto(
                a.Id, a.CategoryId, a.ListId, a.SetId,
                a.ActionText, a.ActionDscr,
                a.CompleteInd, a.CompletedBy, a.CompletedAt, a.SortOrder));
            return Results.Ok(dtos);
        });

        group.MapPut("/{id:int}/complete", async (
            int id,
            ICheckActionRepository repo,
            IHubContext<CheckListHub, ICheckListHubClient> hubContext,
            HttpContext ctx) =>
        {
            var userId = ctx.User.Identity?.Name ?? string.Empty;
            var action = await repo.ToggleCompleteAsync(id, userId);
            if (action is null) return Results.NotFound();

            var update = new ActionUpdateDto(
                SetId: action.SetId,
                ListId: action.ListId,
                ActionId: action.Id,
                CompleteInd: action.CompleteInd,
                CompletedBy: action.CompletedBy,
                CompletedAt: action.CompletedAt
            );

            await hubContext.Clients.Group($"set-{update.SetId}").ReceiveActionUpdate(update);
            return Results.Ok(update);
        });

        return app;
    }
}
