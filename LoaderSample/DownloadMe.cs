namespace LoaderSample
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class DownloadMe
    {
        //Exposes count of received bytes from the last BytesReceived event, i.e. bytes count increment
        public event Action<int>? BytesReceived;
        public event Action? Connecting;
        public event Action? Connected;
        public event Action? Finishing;
        public event Action? Finished;
        public event Action? Paused;
        public int TotalBytesToDownload { get; }

        private readonly int _averageSpeedBytesPerSec;
        private readonly Random _random = new Random();

        public DownloadMe()
        {
            this.TotalBytesToDownload = this._random.Next(30) * 1_000_000;
            this._averageSpeedBytesPerSec = this._random.Next(3_000_000);
        }

        //Continue download from the given initialPosition
        public void StartDownload(CancellationToken cancellationToken, int initialPosition = 0)
        {
            var position = initialPosition;
            this.Connect();
            while (position < this.TotalBytesToDownload && !cancellationToken.IsCancellationRequested)
            {
                //Simulate network delay
                Task.Delay(this._random.Next(1000 / 20)).Wait();
                var value = Math.Min(this.TotalBytesToDownload - position, this._random.Next(this._averageSpeedBytesPerSec / 20));
                position += value;
                this.OnProgress(value);
            }
            if (!cancellationToken.IsCancellationRequested)
                this.Finish();
            else
                this.Paused?.Invoke();
        }

        private void OnProgress(int increment) => this.BytesReceived?.Invoke(increment);

        //Simulate finalizing like checking hash, saving to disk etc...
        private void Finish()
        {
            this.Finishing?.Invoke();
            Task.Delay(2000).Wait();
            this.Finished?.Invoke();
        }

        //Simulate connecting to server...
        private void Connect()
        {
            this.Connecting?.Invoke();
            Task.Delay(2000).Wait();
            this.Connected?.Invoke();
        }
    }
}
