namespace KeqingNiuza.Launcher
{
    class ProgressChangedEventArgs
    {
        public long TotalSize { get; }

        public long DownloadedSize { get; }

        public long Speed { get; }

        public ProgressChangedEventArgs(long totalSize, long downloadedSize, long speed)
        {
            TotalSize = totalSize;
            DownloadedSize = downloadedSize;
            Speed = speed;
        }
    }
}
