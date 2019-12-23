namespace Components
{
    using System;
    using System.Collections.Immutable;
    using System.Globalization;

    /// <summary>
    /// The progress config struct. Describes objects, which can be used as the <see cref="Progress"/> control data model.
    /// </summary>
    /// <typeparam name="T">
    /// The type of a progress payload.
    /// For example int, double, float.
    /// </typeparam>
    public struct ProgressConfig<T> where T : IConvertible
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressConfig"/> struct.
        /// </summary>
        /// <param name="title">The main operation title. Will be displayed at the top of a progress.</param>
        /// <param name="finishValue">Indicates the final progress value of operation.</param>
        /// <param name="start">The operation start callback.</param>
        /// <param name="pause">The operation pause callback.</param>
        /// <param name="progressSubscription">The pair of actions which can subscribe and unsubscribe the control to a progress event.</param>
        /// <param name="finishSubscription">The pair of actions which can subscribe and unsubscribe the control to a finish event.</param>
        /// <param name="pauseSubscription">The pair of actions which can subscribe and unsubscribe the control to a pause event.</param>
        public ProgressConfig(
            string title,
            T finishValue,
            Action<T> start,
            Action pause,
            (Action<Action<T>> subscribe, Action<Action<T>> unsubscribe) progressSubscription,
            (Action<Action> subscribe, Action<Action> unsubscribe) finishSubscription,
            (Action<Action> subscribe, Action<Action> unsubscribe) pauseSubscription
        )
        {
            this.Start = start;
            this.Pause = pause;
            this.Title = title;
            this.FinishValue = (double) Convert.ChangeType(finishValue, typeof(double));

            this.ProgressSubscription = progressSubscription;
            this.FinishSubscription = finishSubscription;
            this.PauseSubscription = pauseSubscription;

            this.SpeedConverter = (e) => e.ToString(CultureInfo.InvariantCulture);
            this.ProgressConverter = (e) => e.ToString(CultureInfo.InvariantCulture);
            this.TimeConverter = (e) =>
            {
                if (e == null) return string.Empty;

                return ((TimeSpan) e).ToString(@"m\:s");
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

        /// <summary>
        /// Gets the main operation title. Will be displayed at the top of a progress. 
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the value indicating the final progress value of operation.
        /// </summary>
        public double FinishValue { get; }

        public ImmutableList<ProgresslessOperation> SubOperations { get; }

        internal Action<T> Start { get; }

        internal Action Pause { get; }

        internal Func<double, string> SpeedConverter { get; }

        internal Func<T, string> ProgressConverter { get; }

        internal Func<TimeSpan?, string> TimeConverter { get; }

        internal (Action<Action<T>> subscribe, Action<Action<T>> unsubscribe) ProgressSubscription { get; }

        internal (Action<Action> subscribe, Action<Action> unsubscribe) FinishSubscription { get; }

        internal (Action<Action> subscribe, Action<Action> unsubscribe) PauseSubscription { get; }

        public ProgressConfig<T> WithSubOperation(ProgresslessOperation operation)
        {
            return new ProgressConfig<T>(this, operation);
        }

        /// <summary>
        /// Creates new instance of config with given speed converter.
        /// </summary>
        /// <param name="speedConverter">
        /// The action, which should convert speed (progress point/millisecond) speed to the any user friendly speed string.
        /// </param>
        /// <returns>New instance of config with given speed converter</returns>
        public ProgressConfig<T> WithSpeedConverter(Func<double, string> speedConverter)
        {
            return new ProgressConfig<T>(this, speedConverter);
        }

        /// <summary>
        /// Creates new instance of config with given speed converter.
        /// </summary>
        /// <param name="progressConverter">
        /// The action, which should convert progress(progress points) to the any user friendly progress string.
        /// (for example 100mb)
        /// </param>
        /// <returns>New instance of config with given progress converter</returns>
        public ProgressConfig<T> WithProgressConverter(Func<T, string> progressConverter)
        {
            return new ProgressConfig<T>(this, progressConverter);
        }

        /// <summary>
        /// Creates new instance of config with given speed converter.
        /// </summary>
        /// <param name="timeConverter">
        /// The action, which should convert <see cref="ProgressConfig"/> to the any user friendly time string.
        /// (5m:2s)
        /// </param>
        /// <returns>New instance of config with given time converter</returns>
        public ProgressConfig<T> WithTimeConverter(Func<TimeSpan?, string> timeConverter)
        {
            return new ProgressConfig<T>(this, timeConverter);
        }
    }
}
