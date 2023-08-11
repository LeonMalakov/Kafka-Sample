using Confluent.Kafka;
using ConsumerCS.Interfaces;
using System;
using System.Threading;

namespace ConsumerCS {
    internal class Consumer : IDisposable {
        private readonly string _topic;
        private readonly IConsumer<Null, string> _consumer;
        private readonly IMessageQueue _queue;
        private readonly CancellationTokenSource _cts;

        public Consumer(string host, string topic, IMessageQueue queue) {
            _topic = topic;
            _queue = queue;

            var config = new ConsumerConfig() {
                BootstrapServers = host,
                GroupId = Constants.GROUP,
                AutoOffsetReset = AutoOffsetReset.Latest,
                //EnableAutoCommit = true
            };

            _cts = new CancellationTokenSource();

            _consumer = new ConsumerBuilder<Null, string>(config)
                .Build();

            _consumer.Subscribe(_topic);
        }

        public void Start(CancellationToken cancellation) {
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellation, _cts.Token);
            Loop(cts.Token);
        }

        private void Loop(CancellationToken cancellation) {
            try {
                while (!cancellation.IsCancellationRequested) {
                    var result = _consumer.Consume(cancellation);

                    _queue.Enqueue(result.Message.Value);

                    /* try {
                      //   _consumer.Commit();
                       //  _consumer.StoreOffset(result);
                     } catch(KafkaException ex) {
                         Console.WriteLine( $"Kafka Ex: ${ex.Message}");
                     }*/
                }
            } catch (Exception ex) {
                Console.WriteLine($"EX: {ex}");
            } finally {
                Console.WriteLine($"EXIT");
                _consumer.Unsubscribe();
                _consumer.Close();
                //_consumer.Dispose();
            }
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                //if (disposing) { }

                _cts?.Cancel();
                _consumer.Dispose();
                disposedValue = true;
            }
        }

        ~Consumer() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}