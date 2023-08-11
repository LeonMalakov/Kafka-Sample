using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerCS {
    internal class Producer : IDisposable {
        private readonly string _topic;
        private readonly IProducer<Null, string> _producer;

        public Producer(string hosts, string topic) {
            _topic = topic;

            var config = new ProducerConfig() {
                BootstrapServers = hosts
            };

            _producer = new ProducerBuilder<Null, string>(config)
                .Build();
        }

        public void Dispose() {
            _producer.Dispose();
        }

        public async Task Send(string content, CancellationToken cancellation = default) {
            var message = new Message<Null, string>() {
                Value = content
            };
            await _producer.ProduceAsync(_topic, message, cancellation);
        }
    }
}
