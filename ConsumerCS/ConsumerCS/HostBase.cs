using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ConsumerCS {
    internal abstract class HostBase {
        private CancellationTokenSource? _cts;

        internal void Start() {
            if (_cts != null) {
                throw new InvalidOperationException();
            }

            _cts = new CancellationTokenSource();

            Task.Factory.StartNew(() => SafeExecuteAsync(_cts.Token), 
                _cts.Token, 
                TaskCreationOptions.LongRunning, 
                TaskScheduler.Default);
        }

        internal void Stop() {
            _cts?.Cancel();
        }


        private Task SafeExecuteAsync(CancellationToken token) {
            return ExecuteAsync(token).ContinueWith(new Action<Task>(ExceptionHandler), TaskContinuationOptions.OnlyOnFaulted);
        }

        private void ExceptionHandler(Task task) {
            AggregateException exception = task.Exception!;
            DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
            defaultInterpolatedStringHandler.AppendLiteral("LongRunningBackgroundService crashed: ");
            defaultInterpolatedStringHandler.AppendFormatted(exception);
            Console.WriteLine(defaultInterpolatedStringHandler.ToStringAndClear());
        }

        private protected abstract Task ExecuteAsync(CancellationToken cancellation);
    }
}
