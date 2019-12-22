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
        private static readonly DependencyPropertyKey DisabledProgressBrushPropertyKey
            = DependencyProperty.RegisterReadOnly(
                "DisabledProgressBrush",
                typeof(Brush), typeof(Progress),
                new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.None));

        private static readonly DependencyPropertyKey ForegroundLightBrushPropertyKey
            = DependencyProperty.RegisterReadOnly(
                "ForegroundLightBrush",
                typeof(Brush), typeof(Progress),
                new FrameworkPropertyMetadata(Brushes.DimGray, FrameworkPropertyMetadataOptions.None));

        private static readonly DependencyPropertyKey ProgressBackgroundBrushPropertyKey
            = DependencyProperty.RegisterReadOnly(
                "ProgressBackgroundBrush",
                typeof(Brush), typeof(Progress),
                new FrameworkPropertyMetadata(Brushes.LightGray, FrameworkPropertyMetadataOptions.None));

        private static readonly DependencyPropertyKey ProgressBrushPropertyKey
            = DependencyProperty.RegisterReadOnly(
                "ProgressBrush",
                typeof(Brush), typeof(Progress),
                new FrameworkPropertyMetadata(Brushes.Green, FrameworkPropertyMetadataOptions.None));

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
                typeof(Color), typeof(Progress),
                new PropertyMetadata(Colors.Green, ProgressColorChanged));

        public static readonly DependencyProperty ProgressBackgroundColorProperty =
            DependencyProperty.Register("ProgressBackgroundColor",
                typeof(Color), typeof(Progress),
                new PropertyMetadata(Colors.GhostWhite, ProgressBackgroundColorChanged));

        public static readonly DependencyProperty DisabledProgressColorProperty =
            DependencyProperty.Register("DisabledProgressColor",
                typeof(Color), typeof(Progress),
                new PropertyMetadata(Colors.Black, DisabledProgressColorChanged));

        public static readonly DependencyProperty ForegroundLightColorProperty =
            DependencyProperty.Register("ForegroundLightColor",
                typeof(Color), typeof(Progress),
                new PropertyMetadata(Colors.Gray, ForegroundLightColorChanged));

        public static readonly DependencyProperty DisabledProgressBrushProperty = DisabledProgressBrushPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ForegroundLightBrushProperty = ForegroundLightBrushPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ProgressBackgroundBrushProperty = ProgressBackgroundBrushPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ProgressBrushProperty = ProgressBrushPropertyKey.DependencyProperty;

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

        public Color ProgressColor
        {
            get => (Color) this.GetValue(ProgressColorProperty);
            set => this.SetValue(ProgressColorProperty, value);
        }

        public Color ProgressBackgroundColor
        {
            get => (Color)this.GetValue(ProgressBackgroundColorProperty);
            set => this.SetValue(ProgressBackgroundColorProperty, value);
        }

        public Color DisabledProgressColor
        {
            get => (Color)this.GetValue(DisabledProgressColorProperty);
            set => this.SetValue(DisabledProgressColorProperty, value);
        }

        public Color ForegroundLightColor
        {
            get => (Color)this.GetValue(ForegroundLightColorProperty);
            set => this.SetValue(ForegroundLightColorProperty, value);
        }

        internal Brush DisabledProgressBrush
        {
            get => (Brush)GetValue(DisabledProgressBrushProperty);
            private set => this.SetValue(DisabledProgressBrushPropertyKey, value);
        }

        internal Brush ForegroundLightBrush
        {
            get => (Brush)this.GetValue(ForegroundLightBrushProperty);
            private set => this.SetValue(ForegroundLightBrushPropertyKey, value);
        }

        internal Brush ProgressBackgroundBrush
        {
            get => (Brush)this.GetValue(ProgressBackgroundBrushProperty);
            private set => this.SetValue(ProgressBackgroundBrushPropertyKey, value);
        }

        internal Brush ProgressBrush
        {
            get => (Brush)this.GetValue(ProgressBrushProperty);
            private set => this.SetValue(ProgressBrushPropertyKey, value);
        }


        private static void DisabledProgressColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Progress) d).DisabledProgressBrush = new SolidColorBrush((Color)e.NewValue);
        }

        private static void ForegroundLightColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Progress)d).ForegroundLightBrush = new SolidColorBrush((Color)e.NewValue);
        }

        private static void ProgressBackgroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Progress)d).ProgressBackgroundBrush = new SolidColorBrush((Color)e.NewValue);
        }

        private static void ProgressColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Progress)d).ProgressBrush = new SolidColorBrush((Color)e.NewValue);
        }

    }
}
