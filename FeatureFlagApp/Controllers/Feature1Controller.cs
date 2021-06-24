using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureFlagApp.Controllers
{
    public class Feature1Controller : Controller
    {
        IFeatureManager _featureManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public Feature1Controller(IFeatureManager featureManager, IHttpContextAccessor httpContextAccessor)
        {
            _featureManager = featureManager;
            this.httpContextAccessor = httpContextAccessor;
            if (!httpContextAccessor.HttpContext.Session.Keys.Contains("StartDateTime"))
                httpContextAccessor.HttpContext.Session.SetString("StartDateTime", DateTime.Now.ToString("ddMMyyyyhhmm"));
        }

        [FeatureGate("TimeLimit")]
        public async Task<IActionResult> Index()
        {
            var startTime = DateTime.ParseExact(httpContextAccessor.HttpContext.Session.GetString("StartDateTime"), "ddMMyyyyhhmm", null);
            var elapsedTime = DateTime.Now.Subtract(startTime).Seconds;

            ViewBag.Date = elapsedTime;
            if (await _featureManager.IsEnabledAsync("TimeLimit"))
            {
                return View();
            }
            return NotFound();
        }
    }
}
