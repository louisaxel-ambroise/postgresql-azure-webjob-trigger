using Microsoft.Azure.WebJobs.Extensions.Bindings;
using System.Reflection;
using System.Threading.Tasks;

namespace PostgreSql.AzureWebjob.Trigger.Notification
{
    public class PostgresqlNotificationValueBinder : ValueBinder
    {
        private readonly object _value;

        public PostgresqlNotificationValueBinder(ParameterInfo parameter, string value) : base(parameter.ParameterType)
        {
            _value = value;
        }

        public override Task<object> GetValueAsync()
        {
            if (Type == typeof(string))
            {
                return Task.FromResult<object>(_value.ToString());
            }
            if (Type == typeof(PgsqlNotification))
            {
                return Task.FromResult<object>(new PgsqlNotification { Payload = _value.ToString() });
            }

            return Task.FromResult(_value);
        }

        public override string ToInvokeString()
        {
            return "Notification";
        }
    }
}
