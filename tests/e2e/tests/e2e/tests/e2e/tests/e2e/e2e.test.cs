using System.Net;
using Xunit;

public class E2ETests
{
    private readonly HttpClient _client = new HttpClient();

    [Fact]
    public async Task HomePage_Should_Return_Status_200()
    {
        // Replace with your QA URL
        var url = "https://bpapp-qa-gjekcge6dbbqarae.westeurope-01.azurewebsites.net/";

        var response = await _client.GetAsync(url);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
