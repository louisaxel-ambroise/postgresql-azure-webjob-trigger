using Npgsql;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Listeners;

namespace PostgreSql.AzureWebjob.Trigger.Notification
{ 
    public class PgsqlNotificationListener : IListener
    {
        private ITriggeredFunctionExecutor _executor;
        private NpgsqlConnection _connection;
        private Task _task;
        private string _channel;

        public PgsqlNotificationListener(ITriggeredFunctionExecutor executor, NpgsqlConnection connection, string channel)
        {
            _executor = executor;
            _channel = channel;

            _connection = connection;
            _connection.Notification += OnNotification;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _task = BeginListen(_connection, cancellationToken);

            return Task.FromResult(true);
        }

        private Task BeginListen(NpgsqlConnection connection, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(async () =>
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"LISTEN {_channel};";
                    await command.ExecuteNonQueryAsync();

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        await connection.WaitAsync();
                    }
                }
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();

            return Task.FromResult(true);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public void Cancel()
        {
            // TODO: cancel any outstanding tasks initiated by this listener
        }

        private void OnNotification(object sender, NpgsqlNotificationEventArgs e)
        {
            TriggeredFunctionData input = new TriggeredFunctionData
            {
                TriggerValue = e.AdditionalInformation
            };

            _executor.TryExecuteAsync(input, CancellationToken.None).RunSynchronously();
        }
    }
}
