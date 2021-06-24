using Microsoft.AspNetCore.Http;
using Microsoft.FeatureManagement;
using System;
using System.Threading.Tasks;

namespace FeatureFlagApp.Infrastructure
{
    [FilterAlias("ShortTimeFeature")]
    public class SecondsFeatureFilter : Microsoft.FeatureManagement.IFeatureFilter
    {
        private readonly IMinuteFeaturePropertyAccessor minuteFeatureContextAccessor;

        public SecondsFeatureFilter(IMinuteFeaturePropertyAccessor minuteFeatureContextAccessor)
        {
            this.minuteFeatureContextAccessor = minuteFeatureContextAccessor;
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var EnabledSeconds = int.Parse(context.Parameters.GetSection("EnabledSeconds").Value);
            if (DateTime.Now.Subtract(minuteFeatureContextAccessor.GetStartTime()).TotalSeconds > EnabledSeconds)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
    }

    public class MinuteFeturePropertyAccessor : IMinuteFeaturePropertyAccessor
    {
        readonly DateTime startTime;
        public MinuteFeturePropertyAccessor()
        {
            startTime = DateTime.Now;
        }

        public DateTime GetStartTime()
        {
            return startTime;
        }
    }

    public interface IMinuteFeaturePropertyAccessor
    {
        DateTime GetStartTime();
    }
   
}
