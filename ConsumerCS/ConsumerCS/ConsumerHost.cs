using ConsumerCS.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace ConsumerCS {
    internal class ConsumerHost : HostBase {
        private readonly IMessageQueue _queue;
        private readonly string _hosts;

        public ConsumerHost(IMessageQueue queue, string hosts) {
            _queue = queue;
            _hosts = hosts;
        }

        private protected override Task ExecuteAsync(CancellationToken cancellation) {
            using (var consumer = new Consumer(_hosts, Constants.TOPIC, _queue)) {
                consumer.Start(cancellation);
            }
            return Task.CompletedTask;
        }
    }
}
