namespace PostgreSql.AzureWebjob.Trigger
{
    /// <summary>
    /// Class that contains all the settings used for PostgreSQL extension triggers
    /// </summary>
    public class PgsqlSettings
    {
        /// <summary>
        /// The connection string for accessing PostgreSQL server
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
