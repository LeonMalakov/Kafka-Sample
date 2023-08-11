using System.Diagnostics.CodeAnalysis;

namespace ConsumerCS.Interfaces {
    internal interface IMessageQueue {
        void Enqueue(string message);
        bool TryDequeue([MaybeNullWhen(false)] out string message);
    }
}
