using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Components
{
    internal static class ProgressModelExtensions
    {
        public static ProgressModel<T> WithStart<T>(
            this ProgressModel<T> self,
            (Action<Action> subscribe, Action<Action> unsubscribe) subscribeStart)
            where T : IConvertible
        {
            return ProgressModel<T>.Create(
                self.Config,
                self.Time,
                self.Progress,
                self.OperationState,
                self.StartSubscriptions.Add(subscribeStart),
                self.PauseSubscriptions,
                self.StateHandlers,
                self.PassedStates,
                self.CurrentSubOperation);
        }

        public static ProgressModel<T> WithCancel<T>(
            this ProgressModel<T> self,
            (Action<Action> subscribe, Action<Action> unsubscribe) subscribeCancel)
            where T : IConvertible
        {
            return ProgressModel<T>.Create(
                self.Config,
                self.Time,
                self.Progress,
                self.OperationState,
                self.StartSubscriptions,
                self.PauseSubscriptions.Add(subscribeCancel),
                self.StateHandlers,
                self.PassedStates,
                self.CurrentSubOperation);
        }

        public static ProgressModel<T> WithStateChangedHandler<T>(
            this ProgressModel<T> self,
            Action<ProgressModel<T>> handler)
           where T : IConvertible
        {
            return ProgressModel<T>.Create(
                self.Config,
                self.Time,
                self.Progress,
                self.OperationState,
                self.StartSubscriptions,
                self.PauseSubscriptions,
                self.StateHandlers.Add(handler),
                self.PassedStates,
                self.CurrentSubOperation);
        }

        public static ProgressModel<T> Bind<T>(this ProgressModel<T> self)
           where T : IConvertible
        {

            Action unsubscribeSubOperations = () => { };

            void ChangeState(ProgressModel<T> model)
            {
                self.Config.FinishSubscription.unsubscribe(Finish);
                self.Config.PauseSubscription.unsubscribe(Pause);
                self.Config.ProgressSubscription.unsubscribe(Progress);
                self.PauseSubscriptions.ForEach((s) => s.unsubscribe(self.Config.Pause));
                self.StartSubscriptions.ForEach((s) => s.unsubscribe(Start));
                unsubscribeSubOperations();

                var nextState = model
                    .WithCurrentTime()
                    .WithPassedState(self)
                    .Bind();

                nextState.StateHandlers.ForEach((hanldeState) => {
                    hanldeState(nextState);
                });
            }

            void Start()
            {
                if (self.OperationState == OperationState.Started)
                {
                    return;
                }

                var startedState = self.OperationState == OperationState.Finished
                    ? self
                        .WithOperationState(OperationState.Started)
                        .WithoutSubOperation()
                        .WithoutPassedStates()
                        .WithProgress(0)
                    : self
                        .WithOperationState(OperationState.Started);

                ChangeState(startedState);

                var convertedProgress = (T) Convert.ChangeType(startedState.Progress, typeof(T));

                Task.Run(() => startedState.Config.Start(convertedProgress));
            }

            void Pause()
            {
                if (self.OperationState == OperationState.Paused)
                {
                    return;
                }

                ChangeState(self
                    .WithOperationState(OperationState.Paused)
                    .WithoutSubOperation());
            }

            void Finish()
            {
                if (self.OperationState == OperationState.Finished)
                {
                    return;
                }

                ChangeState(self
                    .WithOperationState(OperationState.Finished)
                    .WithoutSubOperation());
            }

            void Progress(T p)
            {

                var startedState = self.OperationState == OperationState.Finished
                    ? self
                        .WithProgress(p.ToDouble(CultureInfo.CurrentCulture))
                        .WithOperationState(OperationState.Started)
                        .WithoutSubOperation()
                        .WithoutPassedStates()
                    : self
                        .WithProgress(self.Progress + p.ToDouble(CultureInfo.CurrentCulture))
                        .WithoutSubOperation()
                        .WithOperationState(OperationState.Started);

                ChangeState(startedState);
            }

            self.PauseSubscriptions.ForEach((s) => s.subscribe(self.Config.Pause));
            self.StartSubscriptions.ForEach((s) => s.subscribe(Start));
            self.Config.PauseSubscription.subscribe(Pause);
            self.Config.FinishSubscription.subscribe(Finish);
            self.Config.ProgressSubscription.subscribe(Progress);
            self.Config.SubOperations.ForEach((operation) => {
                void Handle()
                {
                    var startedState = self.OperationState == OperationState.Finished
                        ? self
                            .WithProgress(0)
                            .WithOperationState(OperationState.Started)
                            .WithSubOperation(operation)
                            .WithoutPassedStates()
                        : self
                            .WithSubOperation(operation);

                    ChangeState(startedState);
                }
                var old = unsubscribeSubOperations;

                unsubscribeSubOperations = () =>
                {
                    old();
                    operation.StartSubscription.unsubscribe(Handle);
                };

                operation.StartSubscription.subscribe(Handle);
            });

            return self;
        }

        public static ProgressModel<T> WithOperationState<T>(this ProgressModel<T> self, OperationState state)
        where T : IConvertible
        {
            return ProgressModel<T>.Create(
                        self.Config,
                        self.Time,
                        self.Progress,
                        state,
                        self.StartSubscriptions,
                        self.PauseSubscriptions,
                        self.StateHandlers,
                        self.PassedStates,
                        self.CurrentSubOperation
                    );
        }

        public static ProgressModel<T> WithProgress<T>(this ProgressModel<T> self, double progress)
           where T : IConvertible
        {
            return ProgressModel<T>.Create(
                        self.Config,
                        self.Time,
                        progress,
                        self.OperationState,
                        self.StartSubscriptions,
                        self.PauseSubscriptions,
                        self.StateHandlers,
                        self.PassedStates,
                        self.CurrentSubOperation
                    );
        }

        public static ProgressModel<T> WithCurrentTime<T>(this ProgressModel<T> self)
           where T : IConvertible
        {
            return ProgressModel<T>.Create(
                        self.Config,
                        DateTime.Now,
                        self.Progress,
                        self.OperationState,
                        self.StartSubscriptions,
                        self.PauseSubscriptions,
                        self.StateHandlers,
                        self.PassedStates,
                        self.CurrentSubOperation
                    );
        }

        public static ProgressModel<T> WithPassedState<T>(this ProgressModel<T> self, ProgressModel<T> passedState)
           where T : IConvertible
        {
            return ProgressModel<T>.Create(
                        self.Config,
                        self.Time,
                        self.Progress,
                        self.OperationState,
                        self.StartSubscriptions,
                        self.PauseSubscriptions,
                        self.StateHandlers,
                        self.PassedStates.Add(passedState),
                        self.CurrentSubOperation
                    );
        }

        public static ProgressModel<T> WithoutSubOperation<T>(this ProgressModel<T> self)
           where T : IConvertible
        {
            return ProgressModel<T>.Create(
                        self.Config,
                        self.Time,
                        self.Progress,
                        self.OperationState,
                        self.StartSubscriptions,
                        self.PauseSubscriptions,
                        self.StateHandlers,
                        self.PassedStates
                    );
        }

        public static ProgressModel<T> WithSubOperation<T>(this ProgressModel<T> self, ProgresslessOperation operation)
            where T : IConvertible
        {
            return ProgressModel<T>.Create(
                self.Config,
                self.Time,
                self.Progress,
                self.OperationState,
                self.StartSubscriptions,
                self.PauseSubscriptions,
                self.StateHandlers,
                self.PassedStates,
                operation
            );
        }

        public static ProgressModel<T> WithoutPassedStates<T>(this ProgressModel<T> self)
           where T : IConvertible
        {
            return ProgressModel<T>.Create(
                        self.Config,
                        self.Time,
                        self.Progress,
                        self.OperationState,
                        self.StartSubscriptions,
                        self.PauseSubscriptions,
                        self.StateHandlers,
                        ImmutableList<ProgressModel<T>>.Empty,
                        self.CurrentSubOperation
                    );
        }

        public static double? Speed<T>(this ProgressModel<T> self)
           where T : IConvertible
        {
            if (self.OperationState == OperationState.Initial)
            {
                return null;
            }

            var speeds = self.PassedStates.Aggregate(ImmutableList<double>.Empty, (acc, state) =>
            {
                if (state.OperationState == OperationState.Started)
                {
                    var previous = state.PassedStates.LastOrDefault();

                    if (previous == null || state.Progress <= previous.Progress)
                    {
                        return acc;
                    }

                    return acc.Add((state.Progress - previous.Progress) / (state.Time - previous.Time).TotalMilliseconds);
                }

                return acc;
            }).TakeLast(3);

            if (!speeds.Any())
            {
                return null;
            }

            return speeds.Average();
        }

        public static string ConvertedSpeed<T>(this ProgressModel<T> self)
           where T : IConvertible
        {
            var speed = self.Speed();

            return speed == null ? string.Empty : self.Config.SpeedConverter((double) speed);
        }

        public static string ConvertedProgress<T>(this ProgressModel<T> self)
            where T : IConvertible
        {
            return self.Config.ProgressConverter((T)Convert.ChangeType(self.Progress, typeof(T)));
        }

        public static string ConvertedFinishValue<T>(this ProgressModel<T> self)
            where T : IConvertible
        {
            return self.Config.ProgressConverter((T)Convert.ChangeType(self.Config.FinishValue, typeof(T)));
        }

        public static string ConvertedTime<T>(this ProgressModel<T> self)
            where T : IConvertible
        {
            return self.Config.TimeConverter(self.Time());
        }

        public static TimeSpan? Time<T>(this ProgressModel<T> self)
           where T : IConvertible
        {
            var speed = self.Speed();
            if (speed != null)
            {
                return TimeSpan.FromMilliseconds((self.Config.FinishValue - self.Progress) / (double)speed);
            }
            return null;
        }

        public static double Percent<T>(this ProgressModel<T> self)
            where T : IConvertible
        {
            if (self.Progress == 0)
            {
                return 0;
            }

            return self.Progress/(self.Config.FinishValue / 100);
        }
    }
}
 