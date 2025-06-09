namespace FFMpegCore.Arguments
{
    public interface IInputOutputArgument : IArgument
    {
        void Pre();
        Task During(FFMpegContext? ctx);
        void Post();
    }
}
