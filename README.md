# postgresql-azure-webjob-trigger

Azure Webjob extension that allows to use the PostgreSQL's `LISTEN` command as trigger.

## Usage

The first step to be able to use this trigger is to call the `UsePostgresql` extension method on the `JobHostConfiguration` class, as shown below:

    public class Program
    {
    	static void Main(string[] args)
    	{
    		var configuration = new JobHostConfiguration();
    		configuration.UsePostgresql(new PgsqlSettings
    		{
    			ConnectionString = "your_connection_string"
    		});
    
    		var host = new JobHost(configuration);
    		host.RunAndBlock();
    	}
    }

Then, you can use the `PgsqlNotification` attribute on your webjob methods. This attribute takes one parameter, the name of the channel to listen on:

	public class Functions
	{
		public async Task OnNotification([PgsqlNotification("%channel%")] string payload)
		{
			// Your logic here
		}
	}

The parameter can be bound to either `string` or `PgsqlNotification` (that only contains one `Payload` property.

## Contact

This repository is under GPLv3 licence. Created and maintained by Louis-Axel Ambroise.
For more details, you can contact me on the mail address specified in my Github profile.