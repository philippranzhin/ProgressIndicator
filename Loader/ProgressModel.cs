using System;
using System.Collections.Immutable;

namespace Components
{
    public class ProgressModel<T> where T : IConvertible
    {
        private ProgressModel(
            ProgressConfig<T> config,
            DateTime time,
            double progress,
            OperationState state,
            ImmutableList<(Action<Action> subscribe, Action<Action> unsubscribe)> startSubscriptions,
            ImmutableList<(Action<Action> subscribe, Action<Action> unsubscribe)> pauseSubscriptions,
            ImmutableList<Action<ProgressModel<T>>> stateHandlers,
            ImmutableList<ProgressModel<T>> passesdStates,
            ProgresslessOperation? currentSubOperation = null)
        {
            this.Config = config;
            this.Time = time;
            this.Progress = progress;
            this.OperationState = state;
            this.StartSubscriptions = startSubscriptions;
            this.PauseSubscriptions = pauseSubscriptions;
            this.StateHandlers = stateHandlers;
            this.CurrentSubOperation = currentSubOperation;
            this.PassedStates = passesdStates;
        }

        public ProgressConfig<T> Config { get; }

        public DateTime Time { get; }

        public double Progress { get; }

        public OperationState OperationState { get; }

        public ProgresslessOperation? CurrentSubOperation { get; }

        public ImmutableList<(Action<Action> subscribe, Action<Action> unsubscribe)> StartSubscriptions { get; }

        public ImmutableList<(Action<Action> subscribe, Action<Action> unsubscribe)> PauseSubscriptions { get; }

        public ImmutableList<Action<ProgressModel<T>>> StateHandlers { get; }

        public ImmutableList<ProgressModel<T>> PassedStates { get; }

        public static ProgressModel<TP> Create<TP>(ProgressConfig<TP> operation) where TP : IConvertible
        {
            return new ProgressModel<TP>(
                operation,
                DateTime.Now,
                0,
                OperationState.Initial,
                ImmutableList<(Action<Action>, Action<Action>)>.Empty,
                ImmutableList<(Action<Action>, Action<Action>)>.Empty,
                ImmutableList<Action<ProgressModel<TP>>>.Empty,
                ImmutableList<ProgressModel<TP>>.Empty);
        }

        public static ProgressModel<TP> Create<TP>(
            ProgressConfig<TP> operation,
            DateTime time,
            double progress,
            OperationState state,
            ImmutableList<(Action<Action> subscribe, Action<Action> unsubscribe)> startSubscriptions,
            ImmutableList<(Action<Action> subscribe, Action<Action> unsubscribe)> pauseSubscriptions,
            ImmutableList<Action<ProgressModel<TP>>> stateHandlers,
            ImmutableList<ProgressModel<TP>> states,
            ProgresslessOperation? currentSubOperation = null)
             where TP : IConvertible
        {
            return new ProgressModel<TP>(
                operation,
                time,
                progress,
                state,
                startSubscriptions,
                pauseSubscriptions,
                stateHandlers,
                states,
                currentSubOperation);
        }
    }
}
