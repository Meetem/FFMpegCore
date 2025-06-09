using System.Diagnostics;
using System.IO.Pipes;
using FFMpegCore.Pipes;

namespace FFMpegCore.Arguments
{
    public abstract class PipeArgument
    {
        private string PipeName { get; }
        public string PipePath => PipeHelpers.GetPipePath(PipeName);
        public PipeOptions Options { get; protected set; } = PipeOptions.Asynchronous;
        public PipeTransmissionMode Mode { get; protected set; } = PipeTransmissionMode.Byte;
        public int InBufferSize { get; protected set; } = 16777216;
        public int OutBufferSize { get; protected set; } = 16777216;
        
        protected NamedPipeServerStream Pipe { get; private set; } = null!;
        private readonly PipeDirection _direction;

        protected PipeArgument(PipeDirection direction, int inBufferSize = 16777216, int outBufferSize = 16777216, PipeOptions options = PipeOptions.Asynchronous, PipeTransmissionMode transmission = PipeTransmissionMode.Byte)
        {
            Options = options;
            Mode = transmission;
            InBufferSize = inBufferSize;
            OutBufferSize = outBufferSize;
            
            PipeName = PipeHelpers.GetUnqiuePipeName();
            _direction = direction;
        }

        public void Pre()
        {
            if (Pipe != null)
            {
                throw new InvalidOperationException("Pipe already has been opened");
            }

            Pipe = new NamedPipeServerStream(PipeName, _direction, 1, Mode, Options, InBufferSize, OutBufferSize);
        }

        public void Post()
        {
            Debug.WriteLine($"Disposing NamedPipeServerStream on {GetType().Name}");
            Pipe?.Dispose();
            Pipe = null!;
        }

        public async Task During(FFMpegContext? ctx = null)
        {
            try
            {
                await ProcessDataAsync(ctx).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine($"ProcessDataAsync on {GetType().Name} cancelled");
            }
            finally
            {
                Debug.WriteLine($"Disconnecting NamedPipeServerStream on {GetType().Name}");
                if (Pipe is { IsConnected: true })
                {
                    Pipe.Disconnect();
                }
            }
        }

        protected abstract Task ProcessDataAsync(FFMpegContext? ctx);
        public abstract string Text { get; }
    }
}
