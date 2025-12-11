using Xunit;
using System.Net.Http;

public class E2ETest
{
    [Fact]
    public void HomePageLoads()
    {
        var http = new HttpClient();
        var response = http.GetAsync("https://bpapp-qa.azurewebsites.net").Result;
        Assert.True(response.IsSuccessStatusCode);
    }
}
