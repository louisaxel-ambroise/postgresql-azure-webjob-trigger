using Microsoft.Azure.WebJobs.Host.Protocols;
using System;
using System.Collections.Generic;

namespace PostgreSql.AzureWebjob.Trigger.Notification
{
    public class PostgresqlNotificationParameterDescriptor : TriggerParameterDescriptor
    {
        public override string GetTriggerReason(IDictionary<string, string> arguments)
        {
            return string.Format("Postgresql Notification trigger fired at {0}", DateTime.Now.ToString("o"));
        }
    }
}
