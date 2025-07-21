using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class GetEmployeesFunction
{
    private readonly HttpClient _client;

    public GetEmployeesFunction(IHttpClientFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Function("GetEmployees")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employees")] HttpRequestData req)
    {
        var response = await _client.GetAsync("https://webapp-azurelearning-003.azurewebsites.net/api/employees");
        var content = await response.Content.ReadAsStringAsync();

        var res = req.CreateResponse(response.StatusCode);
        await res.WriteStringAsync(content);
        return new ContentResult
        {
            Content = content,
            ContentType = "application/json",
            StatusCode = (int)response.StatusCode
        };
    }
}
