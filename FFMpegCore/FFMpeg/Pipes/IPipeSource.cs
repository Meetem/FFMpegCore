namespace FFMpegCore.Pipes
{
    /// <summary>
    /// Interface for ffmpeg pipe source data IO
    /// </summary>
    public interface IPipeSource
    {
        string GetStreamArguments();
        Task WriteAsync(FFMpegContext? ctx, Stream outputStream);
    }
}
