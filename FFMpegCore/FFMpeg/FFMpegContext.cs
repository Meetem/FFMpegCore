namespace FFMpegCore
{
    public class FFMpegContext
    {
        public CancellationTokenSource cancelProcessing { get; protected set; }
        public CancellationTokenSource killProcess { get; protected set; }
        public CancellationToken cancellation { get; internal set; }
        public CancellationToken processExited { get; internal set; }

        public FFMpegContext(CancellationTokenSource cancelProcessing, CancellationTokenSource killProcess)
        {
            this.cancelProcessing = cancelProcessing;
            this.killProcess = killProcess;
        }
    }
}
