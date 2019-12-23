using System.Windows.Controls;

namespace Components
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for Loader.xaml
    /// </summary>
    public partial class Progress : UserControl
    {
        public static readonly DependencyProperty ShowButtonOnFinishProperty =
            DependencyProperty.Register("ShowButtonOnFinish", typeof(bool),
                typeof(Progress),
                new PropertyMetadata(false));

        public static readonly DependencyProperty FinishButtonCommandProperty =
            DependencyProperty.Register("FinishButtonCommand", typeof(ICommand),
                typeof(Progress));

        public static readonly DependencyProperty FinishButtonTextProperty =
            DependencyProperty.Register("FinishButton", typeof(string),
                typeof(Progress),
                new PropertyMetadata("RESTART"));

        public static readonly DependencyProperty StartTextProperty =
            DependencyProperty.Register("StartText", typeof(string),
                typeof(Progress), new PropertyMetadata("START"));

        public static readonly DependencyProperty DownloadedTextProperty =
            DependencyProperty.Register("DownloadedText",
                typeof(string), typeof(Progress),
                new PropertyMetadata("Downloaded"));

        public static readonly DependencyProperty SpeedTextProperty =
            DependencyProperty.Register("SpeedText", typeof(string),
                typeof(Progress), new PropertyMetadata("speed"));

        public static readonly DependencyProperty TimeTextProperty =
            DependencyProperty.Register("TimeText", typeof(string),
                typeof(Progress), new PropertyMetadata("time"));

        public static readonly DependencyProperty PauseTextProperty =
            DependencyProperty.Register("PauseText", typeof(string),
                typeof(Progress), new PropertyMetadata("PAUSE"));

        public static readonly DependencyProperty PausedTextProperty =
            DependencyProperty.Register("PausedText", typeof(string),
                typeof(Progress), new PropertyMetadata("Paused"));

        public static readonly DependencyProperty ContinueTextProperty =
            DependencyProperty.Register("ContinueText",
                typeof(string), typeof(Progress),
                new PropertyMetadata("CONTINUE"));

        public static readonly DependencyProperty FinishedTextProperty =
            DependencyProperty.Register("FinishedText",
                typeof(string), typeof(Progress),
                new PropertyMetadata("Finished"));

        public static readonly DependencyProperty ProgressColorProperty =
            DependencyProperty.Register("ProgressColor",
                typeof(Brush), typeof(Progress),
                new PropertyMetadata(new SolidColorBrush(Colors.Green)));

        public static readonly DependencyProperty ProgressBackgroundColorProperty =
            DependencyProperty.Register("ProgressBackgroundColor",
                typeof(Brush), typeof(Progress),
                new PropertyMetadata(new SolidColorBrush(Colors.GhostWhite)));

        public static readonly DependencyProperty DisabledProgressColorProperty =
            DependencyProperty.Register("DisabledProgressColor",
                typeof(Brush), typeof(Progress),
                new PropertyMetadata(new SolidColorBrush(Colors.DimGray)));

        public static readonly DependencyProperty ForegroundLightColorProperty =
            DependencyProperty.Register("ForegroundLightColor",
                typeof(Brush), typeof(Progress),
                new PropertyMetadata(new SolidColorBrush(Colors.Gray)));

        public Progress()
        {
            this.InitializeComponent();
        }

        public bool ShowButtonOnFinish
        {
            get => (bool) this.GetValue(ShowButtonOnFinishProperty);
            set => this.SetValue(ShowButtonOnFinishProperty, value);
        }

        public string StartText
        {
            get => (string) this.GetValue(StartTextProperty);
            set => this.SetValue(StartTextProperty, value);
        }

        public string PauseText
        {
            get => (string) this.GetValue(PauseTextProperty);
            set => this.SetValue(PauseTextProperty, value);
        }

        public string ContinueText
        {
            get => (string) this.GetValue(ContinueTextProperty);
            set => this.SetValue(ContinueTextProperty, value);
        }

        public string FinishedText
        {
            get => (string) this.GetValue(FinishedTextProperty);
            set => this.SetValue(FinishedTextProperty, value);
        }

        public string PausedText
        {
            get => (string) this.GetValue(PausedTextProperty);
            set => this.SetValue(PausedTextProperty, value);
        }

        public string DownloadedText
        {
            get => (string) this.GetValue(DownloadedTextProperty);
            set => this.SetValue(DownloadedTextProperty, value);
        }

        public string SpeedText
        {
            get => (string) this.GetValue(SpeedTextProperty);
            set => this.SetValue(SpeedTextProperty, value);
        }

        public string TimeText
        {
            get => (string) this.GetValue(TimeTextProperty);
            set => this.SetValue(TimeTextProperty, value);
        }

        public string FinishButtonText
        {
            get => (string) this.GetValue(FinishButtonTextProperty);
            set => this.SetValue(FinishButtonTextProperty, value);
        }

        public ICommand FinishButtonCommand
        {
            get => (ICommand) this.GetValue(FinishButtonCommandProperty);
            set => this.SetValue(FinishButtonCommandProperty, value);
        }

        public Brush ProgressColor
        {
            get => (Brush) this.GetValue(ProgressColorProperty);
            set => this.SetValue(ProgressColorProperty, value);
        }

        public Brush ProgressBackgroundColor
        {
            get => (Brush)this.GetValue(ProgressBackgroundColorProperty);
            set => this.SetValue(ProgressBackgroundColorProperty, value);
        }

        public Brush DisabledProgressColor
        {
            get => (Brush)this.GetValue(DisabledProgressColorProperty);
            set => this.SetValue(DisabledProgressColorProperty, value);
        }

        public Brush ForegroundLightColor
        {
            get => (Brush)this.GetValue(ForegroundLightColorProperty);
            set => this.SetValue(ForegroundLightColorProperty, value);
        }
    }
}
