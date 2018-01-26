using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace PostgreSql.AzureWebjob.Trigger.Notification
{
    public class PgsqlNotificationTriggerBinding : ITriggerBinding
    {
        private readonly ParameterInfo _parameter;
        private readonly PgsqlSettings _settings;
        private readonly IReadOnlyDictionary<string, Type> _bindingContract;
        private readonly PgsqlNotificationAttribute _attribute;

        public PgsqlNotificationTriggerBinding(ParameterInfo parameter, PgsqlSettings settings, PgsqlNotificationAttribute attribute)
        {
            _parameter = parameter;
            _settings = settings;
            _bindingContract = CreateBindingDataContract();
            _attribute = attribute;
        }

        public IReadOnlyDictionary<string, Type> BindingDataContract => _bindingContract;
        public Type TriggerValueType => typeof(string);

        public Task<ITriggerData> BindAsync(object value, ValueBindingContext context)
        {
            var triggerValue = value as string;
            IValueBinder valueBinder = new PostgresqlNotificationValueBinder(_parameter, triggerValue);
            return Task.FromResult<ITriggerData>(new TriggerData(valueBinder, GetBindingData(triggerValue)));
        }

        public Task<IListener> CreateListenerAsync(ListenerFactoryContext context)
        {
            var connection = OpenConnection(_settings);

            return Task.FromResult<IListener>(new PgsqlNotificationListener(context.Executor, connection, _attribute.Channel));
        }

        private NpgsqlConnection OpenConnection(PgsqlSettings settings)
        {
            var connection = new NpgsqlConnection(_settings.ConnectionString);
            connection.Open();

            return connection;
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new PostgresqlNotificationParameterDescriptor
            {
                Name = _parameter.Name,
                DisplayHints = new ParameterDisplayHints
                {
                    Prompt = "Postgresql Notification",
                    Description = "Postgresql Notification trigger fired",
                    DefaultValue = "Postgresql Notification"
                }
            };
        }

        private IReadOnlyDictionary<string, object> GetBindingData(string value)
        {
            Dictionary<string, object> bindingData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            bindingData.Add("Notification", value);

            return bindingData;
        }

        private IReadOnlyDictionary<string, Type> CreateBindingDataContract()
        {
            Dictionary<string, Type> contract = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            contract.Add("Notification", typeof(string));

            return contract;
        }
    }
}
