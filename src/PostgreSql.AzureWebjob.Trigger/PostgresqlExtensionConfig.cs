using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;
using PostgreSql.AzureWebjob.Trigger.Notification;
using System;

namespace PostgreSql.AzureWebjob.Trigger
{
    public class PostgresqlExtensionConfig : IExtensionConfigProvider
    {
        private readonly PgsqlSettings _settings;

        public PostgresqlExtensionConfig(PgsqlSettings settings)
        {
            _settings = settings;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            context.Config.RegisterBindingExtensions(new PgsqlNotificationAttributeBindingProvider(_settings));
        }
    }
}
