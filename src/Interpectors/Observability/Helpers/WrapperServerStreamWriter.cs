using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace PlusUltra.GrpcWebServer.Interpectors.Observability.Helpers
{
    /// <summary>
    /// Wrapper for <see cref="IServerStreamWriter{T}"/>. 
    /// Adds possibility to execute action after each message sent through the stream.
    /// </summary>
    /// <typeparam name="T">Model object for message sent through the stream</typeparam>
    public class WrapperServerStreamWriter<T> : IServerStreamWriter<T>
    {
        private readonly IServerStreamWriter<T> _writer;
        private readonly Action _onMessage;

        /// <summary>
        /// Constructor for <see cref="IServerStreamWriter{T}"/> wrapper
        /// </summary>
        /// <param name="writer">Stream writer that should be wrapped by this class</param>
        /// <param name="onMessage">Action that should be executed on each message sent through the stream</param>
        public WrapperServerStreamWriter(IServerStreamWriter<T> writer, Action onMessage)
        {
            _writer = writer;
            _onMessage = onMessage;
        }

        public Task WriteAsync(T message)
        {
            Task result = _writer.WriteAsync(message);

            result.ContinueWith(task =>
            {
                _onMessage.Invoke();
            });

            return result;
        }

        public WriteOptions WriteOptions
        {
            get => _writer.WriteOptions;
            set => _writer.WriteOptions = value;
        }
    }
}