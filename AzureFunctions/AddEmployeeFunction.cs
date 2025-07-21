using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class AddEmployeeFunction
{
    private readonly HttpClient _client;

    public AddEmployeeFunction(IHttpClientFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Function("AddEmployee")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "employees")] HttpRequestData req)
    {
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var response = await _client.PostAsync(
            "https://webapp-azurelearning-003.azurewebsites.net/api/Employees",
            new StringContent(requestBody, Encoding.UTF8, "application/json")
        );

        var content = await response.Content.ReadAsStringAsync();
        var result = req.CreateResponse(response.StatusCode);
        await result.WriteStringAsync(content);
        return new ContentResult
        {
            Content = content,
            ContentType = "application/json",
            StatusCode = (int)response.StatusCode
        };
    }
}
