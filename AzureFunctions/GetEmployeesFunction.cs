using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

public static class GetEmployeesFunction
{
    [FunctionName("GetEmployees")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employees")] HttpRequest req,
        ILogger log)
    {
        var client = new HttpClient();
        var response = await client.GetAsync("https://webapp-azurelearning-003.azurewebsites.net/api/Employees");

        var content = await response.Content.ReadAsStringAsync();
        return new ContentResult
        {
            Content = content,
            ContentType = "application/json",
            StatusCode = (int)response.StatusCode
        };
    }
}
