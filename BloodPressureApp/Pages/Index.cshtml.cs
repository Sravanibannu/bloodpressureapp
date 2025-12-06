using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BloodPressureApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly TelemetryClient _telemetry;

        public IndexModel(TelemetryClient telemetry)
        {
            _telemetry = telemetry;
        }

        [BindProperty] public int Systolic { get; set; }
        [BindProperty] public int Diastolic { get; set; }
        public string? CategoryResult { get; set; }
        public string? Trend { get; set; }
        public string? CategoryColor { get; set; }
        public string? HealthTip { get; set; }

        public void OnGet()
        {
            Trend = HttpContext.Session.GetString("bp.trend");
        }

        public void OnPost()
        {
            var prevS = int.TryParse(HttpContext.Session.GetString("bp.systolic"), out var ps) ? ps : (int?)null;
            var prevD = int.TryParse(HttpContext.Session.GetString("bp.diastolic"), out var pd) ? pd : (int?)null;

            CategoryResult = GetCategory(Systolic, Diastolic);

            // Assign color + health tip based on category
            if (CategoryResult == "High Blood Pressure")
            {
                CategoryColor = "red";
                HealthTip = "Consider lifestyle changes and consult your doctor regularly.";
            }
            else if (CategoryResult == "Low Blood Pressure")
            {
                CategoryColor = "goldenrod";
                HealthTip = "Stay hydrated and monitor symptoms like dizziness.";
            }
            else if (CategoryResult == "Optimal Blood Pressure")
            {
                CategoryColor = "green";
                HealthTip = "Great job! Keep up your healthy habits.";
            }
            else
            {
                CategoryColor = "black";
                HealthTip = "Unable to classify. Please recheck your values.";
            }

            if (prevS is int && prevD is int)
            {
                var deltaS = Systolic - prevS.Value;
                var deltaD = Diastolic - prevD.Value;
                Trend = (Math.Abs(deltaS) + Math.Abs(deltaD)) <= 4 ? "similar"
                    : (deltaS + deltaD) > 0 ? "higher" : "lower";
                HttpContext.Session.SetString("bp.trend", Trend!);
            }

            HttpContext.Session.SetString("bp.systolic", Systolic.ToString());
            HttpContext.Session.SetString("bp.diastolic", Diastolic.ToString());

            // 🔹 Track telemetry event
            _telemetry.TrackEvent("BP_Calculation", new Dictionary<string, string>
            {
                { "Systolic", Systolic.ToString() },
                { "Diastolic", Diastolic.ToString() },
                { "Category", CategoryResult ?? "Unknown" },
                { "Trend", Trend ?? "none" }
            });
        }

        public string GetCategory(int systolic, int diastolic)
        {
            if (systolic < 90 || diastolic < 60)
                return "Low Blood Pressure";
            else if (systolic < 120 && diastolic < 80)
                return "Optimal Blood Pressure";
            else if (systolic >= 120 || diastolic >= 80)
                return "High Blood Pressure";
            else
                return "Unclassified Blood Pressure";
        }
    }
}