using ConsumerCS.Interfaces;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace ConsumerCS {
    internal sealed class MessageQueue : IMessageQueue {
        private readonly ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();

        public void Enqueue(string message) {
            _queue.Enqueue(message);
        }

        public bool TryDequeue([MaybeNullWhen(false)] out string message) {
            return _queue.TryDequeue(out message);
        }
    }
}
