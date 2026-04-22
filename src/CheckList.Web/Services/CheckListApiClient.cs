namespace CheckList.Web.Services;

using System.Net.Http.Json;
using CheckList.Shared.DTOs;

public class CheckListApiClient(HttpClient httpClient)
{
    // ---- Check Sets --------------------------------------------------------
    public async Task<List<CheckSetDto>> GetSetsAsync()
        => await httpClient.GetFromJsonAsync<List<CheckSetDto>>("/api/sets") ?? [];

    public async Task<CheckSetDto?> GetSetAsync(int id)
        => await httpClient.GetFromJsonAsync<CheckSetDto>($"/api/sets/{id}");

    public async Task<CheckSetDto?> CreateSetAsync(CreateCheckSetRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("/api/sets", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CheckSetDto>();
    }

    public async Task DeleteSetAsync(int id)
        => (await httpClient.DeleteAsync($"/api/sets/{id}")).EnsureSuccessStatusCode();

    // ---- Check Lists -------------------------------------------------------
    public async Task<List<CheckListDto>> GetListsAsync(int setId)
        => await httpClient.GetFromJsonAsync<List<CheckListDto>>($"/api/sets/{setId}/lists") ?? [];

    // ---- Check Actions / Categories ----------------------------------------
    // Note (WI-04): The API's category endpoint is keyed by categoryId, not listId.
    // GetCategoriesAsync returns an empty list for MVP; full implementation is follow-on work.
    public async Task<List<CheckCategoryDto>> GetCategoriesAsync(int listId)
    {
        await Task.CompletedTask;
        return [];
    }

    public async Task<ActionUpdateDto?> ToggleActionAsync(int actionId)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/actions/{actionId}/complete", new { });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ActionUpdateDto>();
    }

    // ---- Templates ---------------------------------------------------------
    public async Task<List<TemplateSetDto>> GetTemplatesAsync()
        => await httpClient.GetFromJsonAsync<List<TemplateSetDto>>("/api/templates") ?? [];

    public async Task<TemplateSetDto?> GetTemplateAsync(int id)
        => await httpClient.GetFromJsonAsync<TemplateSetDto>($"/api/templates/{id}");
}
