using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public class GetEmployeesFunction
{
    private readonly HttpClient _client;

    public GetEmployeesFunction(IHttpClientFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Function("GetEmployees")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employees")] HttpRequestData req)
    {
        var response = await _client.GetAsync("https://webapp-azurelearning-003.azurewebsites.net/api/Employees");

        if (!response.IsSuccessStatusCode)
        {
            return new ObjectResult("Failed to fetch employees") { StatusCode = (int)response.StatusCode };
        }

        var json = await response.Content.ReadAsStringAsync();

        // Optional: deserialize to dynamic or model for better type safety
        var jsonObject = JsonSerializer.Deserialize<object>(json);

        return new OkObjectResult(jsonObject); // ✅ automatically returns application/json
    }
}
