using ConsumerCS.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsumerCS {
    internal class MessageProcessorHost : HostBase {
        private readonly IMessageQueue _queue;

        public MessageProcessorHost(IMessageQueue queue) {
            _queue = queue;
        }

        private protected override async Task ExecuteAsync(CancellationToken cancellation) {
            while (!cancellation.IsCancellationRequested) {
                if (_queue.TryDequeue(out var message)) {
                    Console.WriteLine($"Message: {message}");
                } else {
                    await Task.Delay(100, cancellation);
                }
            }
        }
    }
}
