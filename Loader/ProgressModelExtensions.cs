using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Components
{
    internal static class ProgressModelExtensions
    {
        public static ProgressModel<T> Bind<T>(this ProgressModel<T> self)
           where T : IConvertible
        {

            Action unsubscribeSubOperations = () => { };

            void ChangeState(ProgressModel<T> model)
            {
                self.Config.FinishSubscription.unsubscribe(Finish);
                self.Config.PauseSubscription.unsubscribe(Pause);
                self.Config.ProgressSubscription.unsubscribe(Progress);
                self.PauseSubscription.unsubscribe(self.Config.Pause);
                self.StartSubscription.unsubscribe(Start);
                unsubscribeSubOperations();

                var nextState = model
                    .WithCurrentTime()
                    .WithPassedState(self)
                    .Bind();

                nextState.StateHandler(nextState);
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

            self.PauseSubscription.subscribe(self.Config.Pause);
            self.StartSubscription.subscribe(Start);
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
            return new ProgressModel<T>(self, state);
        }

        public static ProgressModel<T> WithCurrentTime<T>(this ProgressModel<T> self)
           where T : IConvertible
        {
            return new ProgressModel<T>(self, DateTime.Now);
        }

        public static ProgressModel<T> WithPassedState<T>(this ProgressModel<T> self, ProgressModel<T> passedState)
           where T : IConvertible
        {
            return new ProgressModel<T>(self, self.PassedStates.Add(passedState));

        }

        public static ProgressModel<T> WithoutSubOperation<T>(this ProgressModel<T> self)
           where T : IConvertible
        {
            return new ProgressModel<T>(self, (ProgresslessOperation?)null);
        }

        public static ProgressModel<T> WithSubOperation<T>(this ProgressModel<T> self, ProgresslessOperation operation)
            where T : IConvertible
        {
            return new ProgressModel<T>(self, operation);
        }

        public static ProgressModel<T> WithoutPassedStates<T>(this ProgressModel<T> self)
           where T : IConvertible
        {
            return new ProgressModel<T>(self, ImmutableList<ProgressModel<T>>.Empty);
        }

        public static ProgressModel<T> WithProgress<T>(this ProgressModel<T> self, double progress)
            where T : IConvertible
        {
            return new ProgressModel<T>(self, progress);
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
                    

                    if (!state.PassedStates.Any() || state.Progress <= state.PassedStates.Last().Progress)
                    {
                        return acc;
                    }
                    var previous = state.PassedStates.Last();
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
            if (self.Progress <= 0)
            {
                return 0;
            }

            return self.Progress/(self.Config.FinishValue / 100);
        }
    }
}
 