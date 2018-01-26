using System;

namespace PostgreSql.AzureWebjob.Trigger.Notification
{
    /// <summary>
    /// Triggers the webjob whenever a new notification is received on the specified channel name
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class PgsqlNotificationAttribute : Attribute
    {
        public PgsqlNotificationAttribute(string channel)
        {
            Channel = channel;
        }

        /// <summary>
        /// The name of the channel to listen on
        /// </summary>
        public string Channel { get; private set; }
    }
}
