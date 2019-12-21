using System.Windows;

namespace LoaderSample
{
    using System.Threading;
    using Components;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {

            var mock = new DownloadMe();
            var cts = new CancellationTokenSource();
            void start(int i)
            {
                cts = new CancellationTokenSource();
                mock.StartDownload(cts.Token, i);
            }

            void pause()
            {
                cts.Cancel();
            }

            var operation = new ProgressConfig<int>(
                    "Process",
                    mock.TotalBytesToDownload,
                    start,
                    pause,
                    ((e) => mock.BytesReceived += e, (e) => mock.BytesReceived -= e),
                    ((e) => mock.Finished += e, (e) => mock.Finished -= e),
                    ((e) => mock.Paused += e, (e) => mock.Paused -= e))
                .WithSpeedConverter((e) => $"{(e/1000):0.#}mb/s")
                .WithProgressConverter((e) => $"{((double)e/1_000_000):0.#}mb")
                .WithSubOperation(new ProgresslessOperation("Connecting", ((e) => mock.Connecting += e, (e) => mock.Connecting -= e)))
                .WithSubOperation(new ProgresslessOperation("Connected", ((e) => mock.Connected += e, (e) => mock.Connected -= e)))
                .WithSubOperation(new ProgresslessOperation(
                    "Finishing", 
                    ((e) => mock.Finishing += e, (e) => mock.Finishing -= e),
                    false));

            this.Progress.DataContext = new ProgressViewModel<int>(operation);
        }
    }
}
