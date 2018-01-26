namespace PostgreSql.AzureWebjob.Trigger.Notification
{
    /// <summary>
    /// A notification received by the PgsqlNotification trigger
    /// </summary>
    public class PgsqlNotification
    {
        /// <summary>
        /// The payload sent with the NOTIFY command
        /// </summary>
        public string Payload { get; set; }
    }
}
