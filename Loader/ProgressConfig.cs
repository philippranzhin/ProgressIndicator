using System;
using System.Collections.Immutable;

namespace Components
{
    using System.Globalization;

    public class ProgressConfig<T> where T : IConvertible
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
            this.TimeConverter = (e) => e == null ? string.Empty : e?.ToString(@"m\:s");

            this.SubOperations = ImmutableList<ProgresslessOperation>.Empty;
        }

        private ProgressConfig(
            string title,
            double finishValue,
            Action<T> start,
            Action pause,
            Func<double, string> speedConverter,
            Func<T, string> progressConverter,
            Func<TimeSpan?, string> timeConverter,
            ImmutableList<ProgresslessOperation> subOperations,
            (Action<Action<T>> subscribe, Action<Action<T>> unsubscribe) progressSubscription,
            (Action<Action> subscribe, Action<Action> unsubscribe) finishSubscription,
            (Action<Action> subscribe, Action<Action> unsubscribe) pauseSubscription)
        {
            this.Start = start;
            this.Pause = pause;
            this.Title = title;
            this.FinishValue = finishValue;

            this.ProgressSubscription = progressSubscription;
            this.FinishSubscription = finishSubscription;
            this.PauseSubscription = pauseSubscription;

            this.SpeedConverter = speedConverter;
            this.ProgressConverter = progressConverter;
            this.TimeConverter = timeConverter;

            this.SubOperations = subOperations;
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
            return new ProgressConfig<T>(
                this.Title,
                this.FinishValue,
                this.Start,
                this.Pause,
                this.SpeedConverter,
                this.ProgressConverter,
                this.TimeConverter,
                this.SubOperations.Add(operation),
                this.ProgressSubscription,
                this.FinishSubscription,
                this.PauseSubscription
                );
        }

        public ProgressConfig<T> WithSpeedConverter(Func<double, string> speedConverter)
        {
            return new ProgressConfig<T>(
                this.Title,
                this.FinishValue,
                this.Start,
                this.Pause,
                speedConverter,
                this.ProgressConverter,
                this.TimeConverter,
                this.SubOperations,
                this.ProgressSubscription,
                this.FinishSubscription,
                this.PauseSubscription
                );
        }

        public ProgressConfig<T> WithProgressConverter(Func<T, string> progressConverter)
        {
            return new ProgressConfig<T>(
                this.Title,
                this.FinishValue,
                this.Start,
                this.Pause,
                this.SpeedConverter,
                progressConverter,
                this.TimeConverter,
                this.SubOperations,
                this.ProgressSubscription,
                this.FinishSubscription,
                this.PauseSubscription
            );
        }

        public ProgressConfig<T> WithTimeConverter(Func<TimeSpan?, string> timeConverter)
        {
            return new ProgressConfig<T>(
                this.Title,
                this.FinishValue,
                this.Start,
                this.Pause,
                this.SpeedConverter,
                this.ProgressConverter,
                timeConverter,
                this.SubOperations,
                this.ProgressSubscription,
                this.FinishSubscription,
                this.PauseSubscription
            );
        }
    }
}
