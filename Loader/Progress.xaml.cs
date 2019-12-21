﻿using System.Windows.Controls;

namespace Components
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for Loader.xaml
    /// </summary>
    public partial class Progress : UserControl
    {

        public static readonly DependencyProperty ShowButtonOnFinishProperty
            = DependencyProperty.Register(
                "ShowButtonOnFinish",
                typeof(bool),
                typeof(Progress),
                new PropertyMetadata(false)
            );

        public static readonly DependencyProperty StartTextProperty
            = DependencyProperty.Register(
                "StartText",
                typeof(string),
                typeof(Progress),
                new PropertyMetadata("START")
            );

        public static readonly DependencyProperty DownloadedTextProperty
            = DependencyProperty.Register(
                "DownloadedText",
                typeof(string),
                typeof(Progress),
                new PropertyMetadata("Downloaded")
            );

        public static readonly DependencyProperty SpeedTextProperty
            = DependencyProperty.Register(
                "SpeedText",
                typeof(string),
                typeof(Progress),
                new PropertyMetadata("speed")
            );

        public static readonly DependencyProperty TimeTextProperty
            = DependencyProperty.Register(
                "TimeText",
                typeof(string),
                typeof(Progress),
                new PropertyMetadata("time")
            );

        public static readonly DependencyProperty PauseTextProperty
            = DependencyProperty.Register(
                "PauseText",
                typeof(string),
                typeof(Progress),
                new PropertyMetadata("PAUSE")
            );

        public static readonly DependencyProperty PausedTextProperty
            = DependencyProperty.Register(
                "PausedText",
                typeof(string),
                typeof(Progress),
                new PropertyMetadata("Paused")
            );

        public static readonly DependencyProperty ContinueTextProperty
            = DependencyProperty.Register(
                "ContinueText",
                typeof(string),
                typeof(Progress),
                new PropertyMetadata("CONTINUE")
            );

        public static readonly DependencyProperty RestartTextProperty
            = DependencyProperty.Register(
                "RestartText",
                typeof(string),
                typeof(Progress),
                new PropertyMetadata("RESTART")
            );

        public static readonly DependencyProperty FinishedTextProperty
            = DependencyProperty.Register(
                "FinishedText",
                typeof(string),
                typeof(Progress),
                new PropertyMetadata("Finished")
            );

        public Progress()
        {
            this.InitializeComponent();
        }

        public bool ShowButtonOnFinish
        {
            get => (bool)this.GetValue(ShowButtonOnFinishProperty);
            set => this.SetValue(ShowButtonOnFinishProperty, value);
        }

        public string StartText
        {
            get => (string) this.GetValue(StartTextProperty);
            set => this.SetValue(StartTextProperty, value);
        }

        public string PauseText
        {
            get => (string)this.GetValue(PauseTextProperty);
            set => this.SetValue(PauseTextProperty, value);
        }

        public string ContinueText
        {
            get => (string)this.GetValue(ContinueTextProperty);
            set => this.SetValue(ContinueTextProperty, value);
        }

        public string RestartText
        {
            get => (string)this.GetValue(RestartTextProperty);
            set => this.SetValue(RestartTextProperty, value);
        }

        public string FinishedText
        {
            get => (string)this.GetValue(FinishedTextProperty);
            set => this.SetValue(FinishedTextProperty, value);
        }

        public string PausedText
        {
            get => (string)this.GetValue(PausedTextProperty);
            set => this.SetValue(PausedTextProperty, value);
        }

        public string DownloadedText
        {
            get => (string)this.GetValue(DownloadedTextProperty);
            set => this.SetValue(DownloadedTextProperty, value);
        }

        public string SpeedText
        {
            get => (string)this.GetValue(SpeedTextProperty);
            set => this.SetValue(SpeedTextProperty, value);
        }

        public string TimeText
        {
            get => (string)this.GetValue(TimeTextProperty);
            set => this.SetValue(TimeTextProperty, value);
        }
    }
}