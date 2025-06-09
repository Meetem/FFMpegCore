namespace FFMpegCore.Pipes
{
    /// <summary>
    /// Implementation of <see cref="IPipeSource"/> used for stream redirection
    /// </summary>
    public class StreamPipeSource : IPipeSource
    {
        public Stream Source { get; }
        public int BlockSize { get; } = 4096;
        public string StreamFormat { get; } = string.Empty;

        public StreamPipeSource(Stream source)
        {
            Source = source;
        }

        public string GetStreamArguments() => StreamFormat;

        public Task WriteAsync(FFMpegContext? ctx, Stream outputStream) => Source.CopyToAsync(outputStream, BlockSize, ctx?.cancellation ?? CancellationToken.None);
    }
}
