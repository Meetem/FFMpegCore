using System.IO.Pipes;
using FFMpegCore.Pipes;

namespace FFMpegCore.Arguments
{
    /// <summary>
    /// Represents input parameter for a named pipe
    /// </summary>
    public class InputPipeArgument : PipeArgument, IInputArgument
    {
        public readonly IPipeSource Writer;

        public InputPipeArgument(IPipeSource writer, int inBufferSize = 16777216, int outBufferSize = 16777216, PipeOptions options = PipeOptions.Asynchronous, PipeTransmissionMode transmission = PipeTransmissionMode.Byte) 
            : base(PipeDirection.Out, inBufferSize, outBufferSize, options, transmission)
        {
            Writer = writer;
        }

        public override string Text => $"{Writer.GetStreamArguments()} -i \"{PipePath}\"";

        protected override async Task ProcessDataAsync(FFMpegContext? ctx)
        {
            await Pipe.WaitForConnectionAsync(ctx?.cancellation ?? CancellationToken.None).ConfigureAwait(false);
            if (!Pipe.IsConnected)
            {
                throw new OperationCanceledException();
            }

            await Writer.WriteAsync(ctx, Pipe).ConfigureAwait(false);
        }
    }
}
