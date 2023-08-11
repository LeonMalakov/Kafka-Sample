using ProducerCS;
using System;
using System.Threading;
using System.Threading.Tasks;

var closing = new AutoResetEvent(false);
var cts = new CancellationTokenSource();
Console.CancelKeyPress += OnCancelKeyPress;

Console.WriteLine("Started");

string? kafkaHosts = Environment.GetEnvironmentVariable(Constants.HOSTS_ENV);

if(kafkaHosts == null) {
    Console.WriteLine($"Environment variable {Constants.HOSTS_ENV} not set");
    return;
}

Console.WriteLine($"Kafka hosts: {kafkaHosts}");

var task = Task.Run(async () => {
    using (var producer = new Producer(kafkaHosts, Constants.TOPIC)) {
        int i = 0;
        while (!cts.IsCancellationRequested) {
            string message = $"Item {i}";
            i++;
            await producer.Send(message);
            Console.WriteLine(message);
            await Task.Delay(5000, cts.Token);
        }
    }
});

closing.WaitOne();

Console.WriteLine("Shutdown");

void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e) {
    cts.Cancel();
    closing.Set();
    Environment.Exit(0);
};