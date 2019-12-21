﻿using System.ComponentModel;

namespace Components
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using Infra;

    public class ProgressViewModel<T> : INotifyPropertyChanged
        where T : IConvertible
    {
        private double _value = 0;
        private string? _progress;
        private string? _speed;
        private string? _time;
        private string? _title;
        private bool _subOperationRunning = false;
        private bool _showProgressInfo = true;
        private OperationState _state;
        private bool _showTime;

        public ProgressViewModel(ProgressConfig<T> config)
        {
            var model = ProgressModel<T>
                .Create(config)
                .WithStart(((e) => Started += e, (e) => Started -= e))
                .WithCancel(((e) => Canceled += e, (e) => Canceled -= e))
                .WithStateChangedHandler(HandleState)
                .Bind();

            this.HandleState(model);
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand StartCommand => new Command((_) =>
        {
            if (this.State == OperationState.Started)
            {
                this.Canceled?.Invoke();
            }
            else
            {
                this.Started?.Invoke();
            }
        });

        public double Value
        {
            get => this._value;
            set
            {
                this._value = value;
                this.OnPropertyChanged();
            }
        }

        public string? Progress
        {
            get => this._progress;
            set
            {
                this._progress = value;
                this.OnPropertyChanged();
            }
        }

        public string? Speed
        {
            get => this._speed;
            set
            {
                this._speed = value;
                this.OnPropertyChanged();
            }
        }

        public string? Time
        {
            get => this._time;
            set
            {
                this._time = value;
                this.OnPropertyChanged();
            }
        }

        public string? Title
        {
            get => this._title;
            set
            {
                this._title = value;
                this.OnPropertyChanged();
            }
        }

        public bool SubOperationRunning
        {
            get => this._subOperationRunning;
            set
            {
                this._subOperationRunning = value;
                this.OnPropertyChanged();
            }
        }

        public bool ShowProgressInfo
        {
            get => this._showProgressInfo;
            set
            {
                this._showProgressInfo = value;
                this.OnPropertyChanged();
            }
        }

        public OperationState State
        {
            get => this._state;
            set
            {
                this._state = value;
                this.OnPropertyChanged();
            }
        }

        public bool ShowTime
        {
            get => this._showTime;
            set
            {
                this._showTime = value;
                this.OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void HandleState(ProgressModel<T> model)
        {
            this.SubOperationRunning = false;
            this.ShowProgressInfo = model.Progress > 0;
            this.State = model.OperationState;
            this.ShowTime = true;
            this.Value = model.Percent();

            switch (model.OperationState)
            {
                case OperationState.Started:
                    if (model.CurrentSubOperation == null)
                    {
                        this.Progress = $"{model.ConvertedProgress()}/{model.ConvertedFinishValue()}";
                        this.Speed = model.ConvertedSpeed();
                        this.Time = model.ConvertedTime();
                        this.Title = model.Config.Title;
                    }
                    else
                    {
                        this.Value = 0;
                        this.ShowTime = false;
                        this.Title = model.CurrentSubOperation.Title;
                        this.SubOperationRunning = true;
                        this.ShowProgressInfo = !model.CurrentSubOperation.HideWholeProgress && model.Progress > 0;
                    }
                    return;
                case OperationState.Finished:
                    this.ShowTime = false;
                    return;
                case OperationState.Paused:
                    return;
                case OperationState.Initial:
                    return;
            }
        }

        private event Action? Started;
        private event Action? Canceled;
    }
}
