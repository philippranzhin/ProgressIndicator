using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Components
{
    public static class ProgressModelExtensions
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

            void changeState(ProgressModel<T> model)
            {
                self.Config.FinishSubscription.unsubscribe(finish);
                self.Config.PauseSubscription.unsubscribe(pause);
                self.Config.ProgressSubscription.unsubscribe(progress);
                self.PauseSubscriptions.ForEach((s) => s.unsubscribe(self.Config.Pause));
                self.StartSubscriptions.ForEach((s) => s.unsubscribe(start));
                unsubscribeSubOperations();

                var nextState = model
                    .WithCarrentTime()
                    .WithPassedState(self)
                    .Bind();

                nextState.StateHandlers.ForEach((hanldeState) => {
                    hanldeState(nextState);
                });
            }

            void start()
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

                changeState(startedState);

                var convertedProgress = (T) Convert.ChangeType(startedState.Progress, typeof(T));

                Task.Run(() => startedState.Config.Start(convertedProgress));
            }

            void pause()
            {
                if (self.OperationState == OperationState.Paused)
                {
                    return;
                }

                changeState(self
                    .WithOperationState(OperationState.Paused)
                    .WithoutSubOperation());
            }

            void finish()
            {
                if (self.OperationState == OperationState.Finished)
                {
                    return;
                }

                changeState(self
                    .WithOperationState(OperationState.Finished)
                    .WithoutSubOperation());
            }

            void progress(T p)
            {
                changeState(self
                    .WithProgress(self.Progress + p.ToDouble(CultureInfo.CurrentCulture))
                    .WithoutSubOperation());
            }

            self.PauseSubscriptions.ForEach((s) => s.subscribe(self.Config.Pause));
            self.StartSubscriptions.ForEach((s) => s.subscribe(start));
            self.Config.PauseSubscription.subscribe(pause);
            self.Config.FinishSubscription.subscribe(finish);
            self.Config.ProgressSubscription.subscribe(progress);
            self.Config.SubOperations.ForEach((operation) => {
                void handle()
                {
                    changeState(ProgressModel<T>.Create(
                            self.Config,
                            self.Time,
                            self.Progress,
                            self.OperationState,
                            self.StartSubscriptions,
                            self.PauseSubscriptions,
                            self.StateHandlers,
                            self.PassedStates,
                            operation
                        ));
                }
                var old = unsubscribeSubOperations;

                unsubscribeSubOperations = () =>
                {
                    old();
                    operation.StartSubscription.unsubscribe(handle);
                };

                operation.StartSubscription.subscribe(handle);
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

        public static ProgressModel<T> WithCarrentTime<T>(this ProgressModel<T> self)
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
                        ImmutableList<ProgressModel<T>>.Empty
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

            if (speeds.Count() == 0)
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
 