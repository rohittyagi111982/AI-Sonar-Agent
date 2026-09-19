using MCPServerAPI.Model;
using MCPServerAPI.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MCPServerAPI;

public class SonarService
{
    private readonly HttpClient _client;

    public SonarService(IConfiguration configuration)
    {
        var url = configuration["SonarQube:Url"];
        var token = configuration["SonarQube:Token"];

        if (string.IsNullOrWhiteSpace(url))
            throw new InvalidOperationException(
                "SonarQube:Url is missing.");

        if (string.IsNullOrWhiteSpace(token))
            throw new InvalidOperationException(
                "SonarQube:Token is missing.");

        _client = new HttpClient
        {
            BaseAddress = new Uri(url)
        };

        var auth = Convert.ToBase64String(
            Encoding.ASCII.GetBytes($"{token}:"));

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", auth);
    }

    public async Task<SonarIssueResponse> GetIssues(string projectKey)
    {
        var response = await _client.GetAsync(
            $"/api/issues/search?componentKeys={projectKey}");

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<SonarIssueResponse>()
            ?? new SonarIssueResponse();
    }

    public async Task<MeasuresResponse?> GetMeasures(string projectKey)
    {
        var response = await _client.GetAsync(
            "/api/measures/component" +
            $"?component={projectKey}" +
            "&metricKeys=" +
            "coverage," +
            "duplicated_lines_density," +
            "reliability_rating," +
            "security_rating," +
            "sqale_rating");

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<MeasuresResponse>();
    }

    public async Task<ProjectStatusResponse?> GetQualityGates(
        string projectKey)
    {
        var response = await _client.GetAsync(
            $"/api/qualitygates/project_status?projectKey={projectKey}");

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<ProjectStatusResponse>();
    }
}