namespace FFMpegCore.Pipes
{
    public interface IPipeSink
    {
        Task ReadAsync(FFMpegContext? ctx, Stream inputStream);
        string GetFormat();
    }
}
