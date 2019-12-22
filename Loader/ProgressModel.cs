using System;
using System.Collections.Immutable;

namespace Components
{
    internal struct ProgressModel<T> where T : IConvertible
    {
        private ProgressModel(
            ProgressConfig<T> config,
            DateTime time,
            double progress,
            OperationState state,
            (Action<Action> subscribe, Action<Action> unsubscribe) startSubscription,
            (Action<Action> subscribe, Action<Action> unsubscribe) pauseSubscription,
            Action<ProgressModel<T>> stateHandler,
            ImmutableList<double> instantSpeeds,
            ProgresslessOperation? currentSubOperation = null
        )
        {
            this.Config = config;
            this.Time = time;
            this.Progress = progress;
            this.OperationState = state;
            this.StartSubscription = startSubscription;
            this.PauseSubscription = pauseSubscription;
            this.StateHandler = stateHandler;
            this.CurrentSubOperation = currentSubOperation;
            this.InstantSpeeds = instantSpeeds;
        }

        internal ProgressModel(ProgressModel<T> model, ProgresslessOperation? currentSubOperation)
        {
            this = model;
            this.CurrentSubOperation = currentSubOperation;
        }

        internal ProgressModel(ProgressModel<T> model, double progress)
        {
            this = model;
            this.Progress = progress;
        }

        internal ProgressModel(ProgressModel<T> model, ImmutableList<double> instantSpeeds)
        {
            this = model;
            this.InstantSpeeds = instantSpeeds;
        }

        internal ProgressModel(ProgressModel<T> model, OperationState state)
        {
            this = model;
            this.OperationState = state;
        }

        internal ProgressModel(ProgressModel<T> model, DateTime time)
        {
            this = model;
            this.Time = time;
        }

        public ProgressConfig<T> Config { get; }

        public DateTime Time { get; }

        public double Progress { get; }

        public OperationState OperationState { get; }

        public ProgresslessOperation? CurrentSubOperation { get; }

        public (Action<Action> subscribe, Action<Action> unsubscribe) StartSubscription { get; }

        public (Action<Action> subscribe, Action<Action> unsubscribe) PauseSubscription { get; }

        public Action<ProgressModel<T>> StateHandler { get; }

        public ImmutableList<double> InstantSpeeds { get; }

        public static ProgressModel<TP> Create<TP>(
            ProgressConfig<TP> operation,
            (Action<Action> subscribe, Action<Action> unsubscribe) startSubscription,
            (Action<Action> subscribe, Action<Action> unsubscribe) pauseSubscription,
            Action<ProgressModel<TP>> stateHandler
        ) where TP : IConvertible
        {
            return new ProgressModel<TP>(operation, DateTime.Now, 0, OperationState.Initial, startSubscription,
                pauseSubscription, stateHandler, ImmutableList<double>.Empty);
        }
    }
}
