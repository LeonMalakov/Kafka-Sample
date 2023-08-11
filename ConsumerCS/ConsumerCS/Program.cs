using ConsumerCS;
using System;
using System.Threading;

bool shutdown = false;
var closing = new AutoResetEvent(false);

var messageQueue = new MessageQueue();
var consumerHost = new ConsumerHost(messageQueue);
var messageProcessorHost = new MessageProcessorHost(messageQueue);

Start();

Console.WriteLine("Started");

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