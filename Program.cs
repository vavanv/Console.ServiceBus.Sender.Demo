using Azure.Messaging.ServiceBus;

const string serviceBusConnectionString = "Endpoint=sb://az-vv-service-bus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=4xWjFxr+6Lzo+AExVyp5luBy9s/3GFugw+ASbCeFN68=";
const string queueName = "az-vvv-queue-1";
const int maxNumberOfMessages = 3;

ServiceBusClient client = new ServiceBusClient(serviceBusConnectionString);
ServiceBusSender sender = client.CreateSender(queueName);

using ServiceBusMessageBatch batch = await sender.CreateMessageBatchAsync();
for (int i = 1; i <= maxNumberOfMessages; i++)
{
    if (!batch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
    {
        Console.WriteLine($"The message {i} was not added to the batch");
    }
}

try
{
    await sender.SendMessagesAsync(batch);
    Console.WriteLine("Messages Sent");
}
catch (Exception ex)
{
    Console.WriteLine($"Send batch error {ex.Message}");
}
finally
{
    await sender.DisposeAsync();
    await client.DisposeAsync();
}   