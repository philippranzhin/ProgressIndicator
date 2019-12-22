using System;
using System.Collections.Immutable;

namespace Components
{
    using System.Globalization;

    public struct ProgressConfig<T> where T : IConvertible
    {
        public ProgressConfig(
            string title,
            T finishValue,
            Action<T> start,
            Action pause,
            (Action<Action<T>> subscribe, Action<Action<T>> unsubscribe) progressSubscription,
            (Action<Action> subscribe, Action<Action> unsubscribe) finishSubscription,
            (Action<Action> subscribe, Action<Action> unsubscribe) pauseSubscription)
        {
            this.Start = start;
            this.Pause = pause;
            this.Title = title;
            this.FinishValue = (double)Convert.ChangeType(finishValue, typeof(double));

            this.ProgressSubscription = progressSubscription;
            this.FinishSubscription = finishSubscription;
            this.PauseSubscription = pauseSubscription;

            this.SpeedConverter = (e) => e.ToString(CultureInfo.InvariantCulture);
            this.ProgressConverter = (e) => e.ToString(CultureInfo.InvariantCulture);
            this.TimeConverter = (e) =>
            {
                if (e == null)
                {
                    return string.Empty;
                }

                return ((TimeSpan)e).ToString(@"m\:s");
            };

            this.SubOperations = ImmutableList<ProgresslessOperation>.Empty;
        }

        private ProgressConfig(ProgressConfig<T> config, ProgresslessOperation operation)
        {
            this = config;
            this.SubOperations = this.SubOperations.Add(operation);
        }
        
        private ProgressConfig(ProgressConfig<T> config, Func<double, string> speedConverter)
        {
            this = config;
            this.SpeedConverter = speedConverter;
        }

        private ProgressConfig(ProgressConfig<T> config, Func<T, string> progressConverter)
        {
            this = config;
            this.ProgressConverter = progressConverter; 
        }

        private ProgressConfig(ProgressConfig<T> config, Func<TimeSpan?, string> timeConverter)
        {
            this = config;
            this.TimeConverter = timeConverter;
        }

        public Action<T> Start { get; }

        public Action Pause { get; }

        public string Title { get; }

        public double FinishValue { get; }

        public Func<double, string> SpeedConverter { get; }

        public Func<T, string> ProgressConverter { get; }

        public Func<TimeSpan?, string> TimeConverter { get; }

        public ImmutableList<ProgresslessOperation> SubOperations { get; }

        public (Action<Action<T>> subscribe, Action<Action<T>> unsubscribe) ProgressSubscription { get; }

        public (Action<Action> subscribe, Action<Action> unsubscribe) FinishSubscription { get; }

        public (Action<Action> subscribe, Action<Action> unsubscribe) PauseSubscription { get; }

        public ProgressConfig<T> WithSubOperation(ProgresslessOperation operation)
        {
            return new ProgressConfig<T>(this, operation);
        }

        public ProgressConfig<T> WithSpeedConverter(Func<double, string> speedConverter)
        {
            return new ProgressConfig<T>(this, speedConverter);
        }

        public ProgressConfig<T> WithProgressConverter(Func<T, string> progressConverter)
        {
            return new ProgressConfig<T>(this, progressConverter);
        }

        public ProgressConfig<T> WithTimeConverter(Func<TimeSpan?, string> timeConverter)
        {
            return new ProgressConfig<T>(this, timeConverter);
        }
    }
}
