using System.IO.Pipes;
using FFMpegCore.Pipes;

namespace FFMpegCore.Arguments
{
    public class OutputPipeArgument : PipeArgument, IOutputArgument
    {
        public readonly IPipeSink Reader;

        public OutputPipeArgument(IPipeSink reader, int inBufferSize = 16777216, int outBufferSize = 16777216, PipeOptions options = PipeOptions.Asynchronous, PipeTransmissionMode transmission = PipeTransmissionMode.Byte) 
            : base(PipeDirection.In, inBufferSize, outBufferSize, options, transmission)
        {
            Reader = reader;
        }

        public override string Text => $"\"{PipePath}\" -y";

        protected override async Task ProcessDataAsync(FFMpegContext? ctx)
        {
            await Pipe.WaitForConnectionAsync(ctx?.cancellation ?? CancellationToken.None).ConfigureAwait(false);
            if (!Pipe.IsConnected)
            {
                throw new TaskCanceledException();
            }

            await Reader.ReadAsync(ctx, Pipe).ConfigureAwait(false);
        }
    }
}
