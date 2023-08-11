using ProducerCS;
using System;
using System.Threading.Tasks;

Console.WriteLine("Started");

using (var producer = new Producer(Constants.HOSTS, Constants.TOPIC)) {
    for (int i = 0; i < 5; i++) {
        string message = $"Item {i}";
        await producer.Send(message);
        await Task.Delay(1000);
        Console.WriteLine(message);
    }
}

Console.WriteLine("Shutdown");