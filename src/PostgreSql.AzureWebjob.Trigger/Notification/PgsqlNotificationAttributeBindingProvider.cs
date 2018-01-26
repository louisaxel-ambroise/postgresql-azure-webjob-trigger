using Microsoft.Azure.WebJobs.Host.Triggers;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PostgreSql.AzureWebjob.Trigger.Notification
{
    public class PgsqlNotificationAttributeBindingProvider : ITriggerBindingProvider
    {
        private PgsqlSettings _settings;

        public PgsqlNotificationAttributeBindingProvider(PgsqlSettings settings)
        {
            _settings = settings;
        }

        public Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            var parameter = context.Parameter;
            var attribute = parameter.GetCustomAttribute<PgsqlNotificationAttribute>(inherit: false);
            if (attribute == null)
            {
                return Task.FromResult<ITriggerBinding>(null);
            }

            var allowedTypes = new Type[] { typeof(string), typeof(PgsqlNotification) };

            if (!allowedTypes.Contains(parameter.ParameterType))
            {
                throw new InvalidOperationException($"Can't bind PgsqlNotification to type '{parameter.ParameterType}'.");
            }

            return Task.FromResult<ITriggerBinding>(new PgsqlNotificationTriggerBinding(context.Parameter, _settings, attribute));
        }
    }
}
