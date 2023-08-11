using ConsumerCS;
using System;
using System.Threading;
using System.Threading.Tasks;

bool shutdown = false;
var closing = new AutoResetEvent(false);

Console.WriteLine("Started");

string? kafkaHosts = Environment.GetEnvironmentVariable(Constants.HOSTS_ENV);

if (kafkaHosts == null) {
    Console.WriteLine($"Environment variable {Constants.HOSTS_ENV} not set");
    return;
}

Console.WriteLine($"Kafka hosts: {kafkaHosts}");
var messageQueue = new MessageQueue();
var consumerHost = new ConsumerHost(messageQueue, kafkaHosts);
var messageProcessorHost = new MessageProcessorHost(messageQueue);

await Task.Delay(10_000);
Start();


Console.CancelKeyPress += OnCancelKeyPress;
AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
closing.WaitOne();
Console.WriteLine("Shutdown");

void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e) {
    Shutdown();
    closing.Set();
    Environment.Exit(0);
};

void OnProcessExit(object? sender, EventArgs e) {
    Shutdown();
}

void Start() {
    consumerHost.Start();
    messageProcessorHost.Start();
}

void Shutdown() {
    if (shutdown) {
        return;
    }
    shutdown = true;

    consumerHost.Stop();
    messageProcessorHost.Stop();
}