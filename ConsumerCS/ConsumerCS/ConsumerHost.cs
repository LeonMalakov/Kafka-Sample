using ConsumerCS.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace ConsumerCS {
    internal class ConsumerHost : HostBase {
        private readonly IMessageQueue _queue;

        public ConsumerHost(IMessageQueue queue) {
            _queue = queue;
        }

        private protected override Task ExecuteAsync(CancellationToken cancellation) {
            using (var consumer = new Consumer(Constants.HOSTS, Constants.TOPIC, _queue)) {
                consumer.Start(cancellation);
            }
            return Task.CompletedTask;
        }
    }
}
