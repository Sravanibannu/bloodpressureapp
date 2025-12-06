using Xunit;
using BloodPressureApp.Pages;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

public class BloodPressureTests
{
    private IndexModel CreateModel()
    {
        // Create a safe TelemetryClient for testing
        var config = TelemetryConfiguration.CreateDefault();
        var telemetry = new TelemetryClient(config);
        return new IndexModel(telemetry);
    }

    [Theory]
    [InlineData(85, 55, "Low Blood Pressure")]          // clearly low
    [InlineData(89, 59, "Low Blood Pressure")]          // just below threshold
    [InlineData(90, 60, "Optimal Blood Pressure")]      // exact boundary for optimal
    [InlineData(119, 79, "Optimal Blood Pressure")]     // just below high
    [InlineData(120, 80, "High Blood Pressure")]        // exact high boundary
    [InlineData(139, 89, "High Blood Pressure")]        // still high by your logic
    [InlineData(140, 90, "High Blood Pressure")]        // clearly high
    [InlineData(150, 95, "High Blood Pressure")]        // very high
    public void GetCategory_ReturnsExpectedResult(int systolic, int diastolic, string expected)
    {
        var model = CreateModel();
        var result = model.GetCategory(systolic, diastolic);
        Assert.Equal(expected, result);
    }
}