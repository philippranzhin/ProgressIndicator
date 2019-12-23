namespace Components
{
    using System;

    /// <summary>
    /// Describes progressless operation config.
    /// </summary>
    public class ProgresslessOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgresslessOperation"/> struct.
        /// </summary>
        /// <param name="title">Operation title. Will be displayed at the top of progress bar while this operation running.</param>
        /// <param name="startSubscription">The pair of actions which can subscribe and unsubscribe the control to a start operation event.</param>
        /// <param name="hideWholeProgress">Value, indicating whether information row under progress bar should be hidden.</param>
        public ProgresslessOperation(
            string title,
            (Action<Action> subscribe, Action<Action> unsubscribe) startSubscription,
            bool hideWholeProgress = true
        )
        {
            this.Title = title;

            this.StartSubscription = startSubscription;
            this.HideWholeProgress = hideWholeProgress;
            this.HideWholeProgress = hideWholeProgress;
        }

        /// <summary>
        /// Gets a operation title. Will be displayed at the top of progress bar while this operation running.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets a value, indicating whether information row under progress bar should be hidden.
        /// </summary>
        public bool HideWholeProgress { get; }

        internal (Action<Action> subscribe, Action<Action> unsubscribe) StartSubscription { get; }
    }
}
