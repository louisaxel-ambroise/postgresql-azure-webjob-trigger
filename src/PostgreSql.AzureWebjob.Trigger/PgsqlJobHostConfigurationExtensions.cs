using Microsoft.Azure.WebJobs;
using System;

namespace PostgreSql.AzureWebjob.Trigger
{
    public static class PgsqlJobHostConfigurationExtensions
    {
        /// <summary>
        /// Enables the webjob to use PostgreSQL's extension trigger
        /// </summary>
        /// <param name="config">The Webjob configuration</param>
        /// <param name="settings">The PostgreSQL configuration</param>
        public static void UsePostgresql(this JobHostConfiguration config, PgsqlSettings settings)
        {
            if (config == null) throw new ArgumentNullException("config");

            config.RegisterExtensionConfigProvider(new PostgresqlExtensionConfig(settings));
        }
    }
}
